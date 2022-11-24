using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;

namespace Game.managers {
    public class TurnManager : NetworkSingleton<TurnManager>
    {

        //algum erro sinalizado pra esta networklist e networkvariable
        public NetworkList<int> playersOrder = new NetworkList<int>();

        public NetworkVariable<int> currentIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private int ConnectedClientCount => NetworkManager.Singleton.ConnectedClientsIds.Count;
        private int CurrentPlayer => playersOrder[currentIndex.Value];
        public bool IsCurrent => ((int)NetworkManager.LocalClientId == playersOrder[currentIndex.Value]);

        public event Action<bool> isLocalPlayerTurn;

        private void Start()
        {
            if (!NetworkManager.IsHost) return;
            GeneratePlayerOrder();
        }



        public override void OnNetworkSpawn()
        {

            currentIndex.OnValueChanged += PlayerTurn;
            currentIndex.OnValueChanged += (int p, int n) => { Logger.Instance.LogInfo(string.Concat("Vez do jogador:", CurrentPlayer)); };
            

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            currentIndex.OnValueChanged -= PlayerTurn;
            currentIndex.OnValueChanged -= (int p, int n) => { Logger.Instance.LogInfo(string.Concat("Vez do jogador:", CurrentPlayer)); };

        }

        private void PlayerTurn(int _, int _2)
        {
            isLocalPlayerTurn?.Invoke(IsCurrent);
        }

        private void GeneratePlayerOrder()
        {
            List<int> allClientID = new List<int>();
            for (int i = 0; i < ConnectedClientCount; i++)
            {
                allClientID.Add(i);
            }

            allClientID.Shuffle();

            for (int i = 0; i < allClientID.Count; i++)
            {
                playersOrder.Add(allClientID[i]);
                Logger.Instance.LogWarning(allClientID[i].ToString());
            }

            playersOrder = new NetworkList<int>(allClientID,
                NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
            );

        }



        [ServerRpc(RequireOwnership = false)]
        public void NextTurnServerRpc()
        {
            // this will cause a replication over the network
            // and ultimately invoke `OnValueChanged` on receivers
            if (currentIndex.Value < ConnectedClientCount - 1)
            {
                currentIndex.Value++;
            }
            else
            {
                currentIndex.Value = 0;
            }
        }


    }

}


//PARA LEMBRAR DEPOIS, CASO PRECISE
//[Serializable]
//public struct PlayerData: INetworkSerializable, IEquatable<PlayerData>
//{
//    //public FixedString32Bytes name;
//    public int playerIndexGame;
//    public int clientID;


//    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//    {
//        if (serializer.IsReader)
//        {
//            var reader = serializer.GetFastBufferReader();
//            reader.ReadValueSafe(out playerIndexGame);
//            reader.ReadValueSafe(out clientID);

//        }
//        else
//        {
//            var writer = serializer.GetFastBufferWriter();
//            writer.WriteValueSafe(playerIndexGame);
//            writer.WriteValueSafe(clientID);
//        }

//    }

//    public bool Equals(PlayerData other)
//    {

//        return other.Equals(this) &&  clientID == other.clientID;
//    }


//}

