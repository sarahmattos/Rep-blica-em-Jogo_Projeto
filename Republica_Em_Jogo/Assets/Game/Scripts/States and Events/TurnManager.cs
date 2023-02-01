using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;

namespace Game
{
    public class TurnManager : NetworkSingleton<TurnManager>
    {

        //Lembrar que: apenas Servers/Owners podem alterar NetworkVariables.
        //Para fazer isso via client, pode ser usado m�todos ServerRpc, assim como � feito nesta classe
        private NetworkList<int> ordemPlayersID ;
        private NetworkVariable<int> indexPlayerAtual = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> clientesCount = new NetworkVariable<int>();
        private NetworkVariable<int> playerAtual = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private int turnCount = 0;

        public NetworkVariable<int> PlayerAtual => playerAtual;

        public int GetPlayerAtual => playerAtual.Value;
        public int UltimoPlayer => ordemPlayersID[ordemPlayersID.Count - 1];
        public bool UltimoIgualAtual => (GetPlayerAtual == UltimoPlayer);
        public int GetClientesCount => clientesCount.Value;
        public bool LocalIsCurrent => ((int)NetworkManager.Singleton.LocalClientId == GetPlayerAtual);
        public event Action<bool> vezDoPlayerLocal;
        public event Action<int> PlayerTurnMuda;
        public bool nextIgualLocalID;
        public int idPlayer;

        private State InicializaState => GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO];

        public int TurnCount { get => turnCount; set => turnCount = value; }

        private void Awake()
        {
              ordemPlayersID = new NetworkList<int>(new List<int>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        }
           

        private void Start()
        {

            ordemPlayersID = new NetworkList<int>();
                          idPlayer=(int)NetworkManager.Singleton.LocalClientId;

            playerAtual.OnValueChanged += PlayerAtualMuda;
            if (!IsHost) return;
            InicializaState.Entrada += UpdateClientsCount;
            InicializaState.Entrada += DefineConfigIniciais;

            CoreLoopStateHandler.Instance.UltimoLoopState.Saida += NextTurnServerRpc;
        }


        public override void OnDestroy()
        {
            playerAtual.OnValueChanged -= PlayerAtualMuda;
            if (!IsHost) return;
            ordemPlayersID.Dispose();
            indexPlayerAtual.Dispose();
            clientesCount.Dispose();
            playerAtual.Dispose();
            InicializaState.Entrada -= UpdateClientsCount;
            InicializaState.Entrada -= DefineConfigIniciais;
            CoreLoopStateHandler.Instance.UltimoLoopState.Saida -= NextTurnServerRpc;

        }

        private void UpdateClientsCount()
        {
            clientesCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }

        private void PlayerAtualMuda(int previous, int next)
        {
            Logger.Instance.LogWarning(string.Concat("Player ", previous, " encerrou o turno!"));

            nextIgualLocalID = ((int)NetworkManager.Singleton.LocalClientId == next);
            vezDoPlayerLocal?.Invoke(nextIgualLocalID);
            PlayerTurnMuda?.Invoke(next);
            turnCount++;
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

        private void DefineConfigIniciais()
        {
            GerarPlayerOrdem();
            SetIndexPlayerTurnServerRpc(0);
        }

        [ServerRpc(RequireOwnership = false)]
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

