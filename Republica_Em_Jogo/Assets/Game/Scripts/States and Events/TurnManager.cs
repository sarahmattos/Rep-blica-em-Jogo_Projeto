using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;
using UnityEngine;
using UnityEditor;

namespace Game {
    public class TurnManager : NetworkSingleton<TurnManager>
    {

        //Lembrar que: apenas Servers/Owners podem alterar NetworkVariables.
        //Para fazer isso via client, pode ser usado métodos ServerRpc, assim como é feito nesta classe
        private NetworkList<int> ordemPlayersID = new NetworkList<int>(new List<int>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> indexPlayerAtual = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> clientesCount = new NetworkVariable<int>();
        private NetworkVariable<int> playerAtual = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private int turnCount = 0;
        
        public int GetPlayerAtual => playerAtual.Value;
        public int UltimoPlayer => ordemPlayersID[ordemPlayersID.Count-1];   
        public bool UltimoIgualAtual => (GetPlayerAtual == UltimoPlayer);
        public int GetClientesCount => clientesCount.Value;
        public bool LocalIsCurrent => ((int)NetworkManager.Singleton.LocalClientId == GetPlayerAtual);
        public event Action<bool> vezDoPlayerLocal;
        public event Action<int> PlayerTurnMuda;

        private State InicializaState => GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO];

        public int TurnCount { get => turnCount; set => turnCount = value; }

        private void Start()
        {
            playerAtual.OnValueChanged += PlayerAtualMuda;
            InicializaState.Entrada += UpdateClientsCount;
            InicializaState.Entrada += DefineConfigIniciais;

            if (!IsServer) return;
            CoreLoopStateHandler.Instance.UltimoLoopState.Saida += NextTurnServerRpc;
        }


        public override void OnDestroy()
        {
            playerAtual.OnValueChanged += PlayerAtualMuda;
            InicializaState.Entrada-= UpdateClientsCount;
            InicializaState.Entrada -= DefineConfigIniciais;
            if(!IsServer) return;
            CoreLoopStateHandler.Instance.UltimoLoopState.Saida -= NextTurnServerRpc;
        }

        private void UpdateClientsCount()
        {
            if (!IsHost) return;
            clientesCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }

        private void PlayerAtualMuda(int previous, int next)
        {
            bool nextIgualLocalID = ((int)NetworkManager.Singleton.LocalClientId == next);
            vezDoPlayerLocal?.Invoke(nextIgualLocalID);
            PlayerTurnMuda?.Invoke(next);
            turnCount++;
        }



        private void GerarPlayerOrdem()
        {
            if (!IsHost) return;
            List<int> allClientID = new List<int>();
            for (int i = 0; i < GetClientesCount; i++)
            {
                allClientID.Add(i);
            }

            allClientID.Shuffle();
            for (int i = 0; i < allClientID.Count; i++)
            {
                ordemPlayersID.Add(allClientID[i]);

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
            playerAtual.Value = ordemPlayersID[indexPlayerAtual.Value];

        }


        [ServerRpc(RequireOwnership = false)]
        public void NextTurnServerRpc()
        {
            indexPlayerAtual.Value = (1 + indexPlayerAtual.Value) % (GetClientesCount);
            playerAtual.Value = ordemPlayersID[indexPlayerAtual.Value];
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

