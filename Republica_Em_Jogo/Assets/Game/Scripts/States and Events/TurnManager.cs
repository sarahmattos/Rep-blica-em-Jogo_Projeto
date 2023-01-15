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
        public State InicializaState => GameStateHandler.Instance.GameStatePairValue[GameState.INICIALIZACAO];
        private void Awake()
        {
            indexPlayerAtual.OnValueChanged += UpdatePlayerTurn;
            InicializaState.Entrada += UpdateClientscount;
            InicializaState.Entrada += DefineConfigIniciais;
        }

        public override void OnDestroy()
        {
            indexPlayerAtual.OnValueChanged -= UpdatePlayerTurn;
            InicializaState.Entrada-= UpdateClientscount;
            InicializaState.Entrada -= DefineConfigIniciais;

        }
        private void UpdateClientscount()
        {
            if (!IsHost) return;
            connectedClientCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }


        private void UpdatePlayerTurn(int previousValue, int nextValue)
        {
            bool value = ((int)NetworkManager.Singleton.LocalClientId == ordemPlayerID[nextValue]);

            isLocalPlayerTurn?.Invoke(value);
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
                ordemPlayerID.Add(allClientID[i]);

            }

        }

        private void DefineConfigIniciais()
        {
            GeneratePlayerOrder();
            NextTurnServerRpc();

        }

        [ServerRpc(RequireOwnership =false)]
        public void ChangePlayerTurnServerRpc(int clientId)
        {
            indexPlayerAtual.Value = clientId;

            Logger.Instance.LogInfo("turno atualizado. Player atual: " + GetCurrentPlayer);
            Logger.Instance.LogInfo("SUA VEZ?: " + LocalIsCurrent);

        }


        [ServerRpc(RequireOwnership = false)]
        public void NextTurnServerRpc()
        {
            indexPlayerAtual.Value = (1 + indexPlayerAtual.Value) % (GetConnectedClientCount);
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

            playerAtual.Value = ordemPlayerID[indexPlayerAtual.Value];
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

