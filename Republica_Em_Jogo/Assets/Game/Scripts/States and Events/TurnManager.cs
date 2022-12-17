using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;

namespace Game {
    public class TurnManager : NetworkSingleton<TurnManager>
    {

        //Lembrar que: apenas Servers/Owners podem alterar NetworkVariables.
        //Para fazer isso via client, pode ser usado métodos ServerRpc, assim como é feito nesta classe
        private NetworkList<int> playersOrder = new NetworkList<int>(new List<int>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> currentIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> connectedClientCount = new NetworkVariable<int>();
        private NetworkVariable<int> currentPlayer = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public int GetCurrentPlayer => currentPlayer.Value;
        public int GetConnectedClientCount => connectedClientCount.Value;
        public bool LocalIsCurrent => ((int)NetworkManager.Singleton.LocalClientId == GetCurrentPlayer);

        public event Action<bool> isLocalPlayerTurn;

        private void Start()
        {
            currentIndex.OnValueChanged += UpdatePlayerTurn;

            GameStateHandler.Instance.gameplaySceneLoad += UpdateClientscount;
            GameStateHandler.Instance.desenvolvimento += GeneratePlayerOrder;
        }


        public override void OnDestroy()
        {
            currentIndex.OnValueChanged -= UpdatePlayerTurn;
            GameStateHandler.Instance.gameplaySceneLoad -= UpdateClientscount;
            GameStateHandler.Instance.desenvolvimento -= GeneratePlayerOrder;

        }
        private void UpdateClientscount()
        {
            if (!IsHost) return;
            Logger.Instance.LogInfo("souo host e to atualizando clientcount");
            connectedClientCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;

        }

        //private void UpdateCurrentPlayer(int previousValue, int newValue)
        //{
        //    currentIndex.Value = newValue;
        //}


        //public override void OnDestroy()
        //{
        //    currentIndex.OnValueChanged -= UpdatePlayerTurn;
        //}

        private void UpdatePlayerTurn(int previousValue, int nextValue)
        {
            isLocalPlayerTurn?.Invoke(LocalIsCurrent);
        }

        private void GeneratePlayerOrder()
        {
            if (!IsHost) return;

            List<int> allClientID = new List<int>();
            for (int i = 0; i < GetConnectedClientCount; i++)
            {
                allClientID.Add(i);
            }

            allClientID.Shuffle();
            for (int i = 0; i < allClientID.Count; i++)
            {
                Logger.Instance.LogInfo("id: " + allClientID[i]);
                playersOrder.Add(allClientID[i]);

            }

        }

        [ServerRpc(RequireOwnership =false)]
        public void ChangePlayerTurnServerRpc(int value)
        {
            currentIndex.Value = value;

            Logger.Instance.LogInfo("turno atualizado. Player atual: " + GetCurrentPlayer);
            Logger.Instance.LogInfo("turno atualizado. SUA VEZ?: " + LocalIsCurrent);

        }

        [ServerRpc(RequireOwnership = false)]
        public void NextTurnServerRpc()
        {
            currentIndex.Value++;
            //currentIndex.Value =  (1 + currentIndex.Value) % GetConnectedClientCount;

            if (GetConnectedClientCount > 1)
            {
                if (currentIndex.Value < GetConnectedClientCount - 1)
                {
                    currentIndex.Value++;
                } else
                {
                    currentIndex.Value = 0;
                }

            }
            else
            {
                currentIndex.Value = 0;

            }

            currentPlayer.Value = playersOrder[currentIndex.Value];

            Logger.Instance.LogWarning(string.Concat("Player ", GetCurrentPlayer+" vai jogar."));



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

