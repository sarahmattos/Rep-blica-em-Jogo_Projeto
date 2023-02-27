using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;
using UnityEngine;

namespace Game
{
    public class TurnManager : NetworkSingleton<TurnManager>
    {
        //Lembrar que: apenas Servers/Owners podem alterar NetworkVariables.
        //Para fazer isso via client, pode ser usado m�todos ServerRpc, assim como � feito nesta classe
        public NetworkList<int> ordemPlayersID;
        private int indexPlayerAtual = 0;
        private NetworkVariable<int> clientesCount = new NetworkVariable<int>();
        private int turnCount = 0;

        public int PlayerAtual => ordemPlayersID[indexPlayerAtual];
        public int UltimoPlayer => ordemPlayersID[ordemPlayersID.Count - 1];

        //public bool UltimoIgualAtual => (PlayerAtual == UltimoPlayer);
        public int GetClientesCount => clientesCount.Value;
        public bool LocalIsCurrent => ((int)NetworkManager.Singleton.LocalClientId == PlayerAtual);
        public event Action<bool> vezDoPlayerLocal;
        public event Action<int> turnoMuda;
        public bool nextIgualLocalID;
        private State InicializaState =>
            GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO];
        
        private State DistribuicaoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.DISTRIBUICAO];

        public int TurnCount
        {
            get => turnCount;
            set => turnCount = value;
        }

        private void Awake()
        {
            ordemPlayersID = new NetworkList<int>(
                new List<int>(),
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Server
            );
        }

        private void Start()
        {
            turnoMuda += OnTurnoMuda;
            DistribuicaoState.Entrada += NextTurn;

            if (!IsHost)
                return;
            InicializaState.Entrada += UpdateClientsCount;
            InicializaState.Entrada += DefineConfigIniciais;
        }

        public override void OnDestroy()
        {
            turnoMuda -= OnTurnoMuda;
            DistribuicaoState.Entrada -= NextTurn;

            if (!IsHost)
                return;
            ordemPlayersID.Dispose();
            clientesCount.Dispose();
            InicializaState.Entrada -= UpdateClientsCount;
            InicializaState.Entrada -= DefineConfigIniciais;
        }

        private void UpdateClientsCount()
        {
            clientesCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }

        private void OnTurnoMuda(int playerID)
        {
            Logger.Instance.LogWarning(string.Concat("vez do player: ", playerID));

            // nextIgualLocalID = ((int)NetworkManager.Singleton.LocalClientId == playerID);
            vezDoPlayerLocal?.Invoke(LocalIsCurrent);
            turnCount++;
        }

        private void DefineConfigIniciais()
        {
            GerarPlayerOrdem();
        }

        private void GerarPlayerOrdem()
        {
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

        public void SetIndexPlayerTurn(int index)
        {
            indexPlayerAtual = index;
            turnoMuda?.Invoke(PlayerAtual);
        }

        public void NextTurn()
        {
            indexPlayerAtual = (1 + indexPlayerAtual) % (GetClientesCount);
            turnoMuda?.Invoke(PlayerAtual);
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
