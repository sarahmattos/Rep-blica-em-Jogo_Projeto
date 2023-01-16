using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;
using UnityEngine;

namespace Game {
    public class TurnManager : NetworkSingleton<TurnManager>
    {

        //Lembrar que: apenas Servers/Owners podem alterar NetworkVariables.
        //Para fazer isso via client, pode ser usado métodos ServerRpc, assim como é feito nesta classe
        private NetworkList<int> ordemPlayerID = new NetworkList<int>(new List<int>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> indexPlayerAtual = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> connectedClientCount = new NetworkVariable<int>();
        private NetworkVariable<int> playerAtual = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public int GetCurrentPlayer => playerAtual.Value;
        public int GetConnectedClientCount => connectedClientCount.Value;
        public bool LocalIsCurrent => ((int)NetworkManager.Singleton.LocalClientId == GetCurrentPlayer);

        public event Action<bool> isLocalPlayerTurn;
        private State InicializaState => GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO];
        private State RecompensaState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.RECOMPENSA];
       
        private void Start()
        {
            playerAtual.OnValueChanged += UpdatePlayerTurn;
            InicializaState.Entrada += UpdateClientsCount;
            InicializaState.Entrada += DefineConfigIniciais;
            
            RecompensaState.Saida += NextTurnServerRpc;
        }

        public override void OnDestroy()
        {
            playerAtual.OnValueChanged += UpdatePlayerTurn;
            InicializaState.Entrada-= UpdateClientsCount;
            InicializaState.Entrada -= DefineConfigIniciais;
            
            RecompensaState.Saida -= NextTurnServerRpc;
        }

        private void UpdateClientsCount()
        {
            if (!IsHost) return;
            connectedClientCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }

        private void UpdatePlayerTurn(int previous, int next)
        {
            bool nextIgualLocalID = ((int)NetworkManager.Singleton.LocalClientId == next);
            isLocalPlayerTurn?.Invoke(nextIgualLocalID);
        }



        private void GerarPlayerOrdem()
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
                ordemPlayerID.Add(allClientID[i]);

            }

        }

        private void DefineConfigIniciais()
        {
            GerarPlayerOrdem();
            SetIndexPlayerTurnServerRpc(0);
        }

        [ServerRpc(RequireOwnership =false)]
        public void SetIndexPlayerTurnServerRpc(int index)
        {
            indexPlayerAtual.Value = index;
            playerAtual.Value = ordemPlayerID[indexPlayerAtual.Value];

        }


        [ServerRpc(RequireOwnership = false)]
        public void NextTurnServerRpc()
        {
            indexPlayerAtual.Value = (1 + indexPlayerAtual.Value) % (GetConnectedClientCount);
            playerAtual.Value = ordemPlayerID[indexPlayerAtual.Value];
            #region logica alternativa
            //if (GetConnectedClientCount > 1)
            //{
            //    if (currentIndex.Value < GetConnectedClientCount - 1)
            //    {
            //        currentIndex.Value++;
            //    } else
            //    {
            //        currentIndex.Value = 0;
            //    }

            //}
            //else
            //{
            //    currentIndex.Value = 0;

            //}
            #endregion

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

