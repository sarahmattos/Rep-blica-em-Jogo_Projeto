using System.Collections.Generic;
using Unity.Netcode;
using Game.Tools;
using Logger = Game.Tools.Logger;
using System;
using Game.UI;
namespace Game
{
    public class TurnManager : NetworkSingleton<TurnManager>
    {
        //Lembrar que: apenas Servers/Owners podem alterar NetworkVariables.
        //Para fazer isso via client, pode ser usado m�todos ServerRpc, assim como � feito nesta classe
        private HudStatsJogador hs;
        public NetworkList<int> ordemPlayersID;
        private int indexPlayerAtual = -1;
        private NetworkVariable<int> clientesCount = new NetworkVariable<int>();
        //private NetworkVariable<int> clientesAtual = new NetworkVariable<int>();
       // public int playerNow;
        private int turnCount;
        public event Action FirstPlayerTurn;
        public int PlayerAtual => ordemPlayersID[indexPlayerAtual];
        // public int PlayerAtual2 ;
        public int GetClientesCount => clientesCount.Value;
        public bool LocalIsCurrent => ((int)NetworkManager.Singleton.LocalClientId == PlayerAtual);
        public bool CurrentIsUltimo => (PlayerAtual == ordemPlayersID[ordemPlayersID.Count-1]);

        public event Action<bool> vezDoPlayerLocal;
        public event Action<int, int> turnoMuda;
        private State InicializacaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO);
        private State RecompensaState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.RECOMPENSA];
        public int teste, teste2;
        public int aux;
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
            ordemPlayersID.Initialize(this);

            hs = FindObjectOfType<HudStatsJogador>();
            turnoMuda += OnTurnoMuda;
            RecompensaState.Saida += UpdateTurn;
            //Inscrever();

            if (!IsHost)
                return;
            InicializacaoState.Entrada += DefineConfigIniciais;
            //clientesAtual.Value = 1;
        }
         
        // private void OnOrdemIdChanged(NetworkListEvent<int> changeEvent)
        // {
        //         aux++;
        //         teste=changeEvent.Value;
        //         teste2=changeEvent.Index;
        //         hs.ordemId.Add(changeEvent.Value);
        //         if(aux== NetworkManager.Singleton.ConnectedClientsIds.Count){
        //             hs.testeCor();
        //         }
           
           
        // }
       // private void OnPlayerAtualChanged(int previousValue, int newValue)
        //{
       //    playerNow=newValue;
           
        //}
        // private void OnEnable()
        // {
        //     ordemPlayersID.OnListChanged += OnOrdemIdChanged;

        // }
        // private void OnDisable()
        // {
        //     ordemPlayersID.OnListChanged -= OnOrdemIdChanged;

        // }

        public override void OnDestroy()
        {
            turnoMuda -= OnTurnoMuda;
            RecompensaState.Saida -= UpdateTurn;


            if (!IsHost)
                return;
            ordemPlayersID.Dispose();
            clientesCount.Dispose();
            InicializacaoState.Entrada -= DefineConfigIniciais;

        }

        private void OnTurnoMuda(int _, int nextPlayer)
        {
            // Logger.Instance.LogWarning(string.Concat("Turno Atualizado. Vez do jogador: ", nextPlayer));
            vezDoPlayerLocal?.Invoke(LocalIsCurrent);

            if (nextPlayer == ordemPlayersID[0])
            {
                FirstPlayerTurn?.Invoke();
            }
        }

        private void DefineConfigIniciais()
        {
            UpdateClientsCount();
            GerarPlayerOrdem();

        }
        private void UpdateClientsCount()
        {
            clientesCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
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
                //hs.ordemId.Add(allClientID[i]);
                ordemPlayersID.Add(allClientID[i]);
            }
        }


        public void UpdateTurn()
        {
            int previousPlayer = (indexPlayerAtual == -1) ? 0 : PlayerAtual;
            if (turnCount == 0)
            {
                SetIndexPlayerTurn(0);
            }
            else
            {
                NextTurn();
            }
            Tools.Logger.Instance.LogWarning("Update turn");
            TurnCount++;
            turnoMuda?.Invoke(previousPlayer, PlayerAtual);
            //hs.testeCor();
            // PlayerAtual2 = hs.ordemId[indexPlayerAtual];
            // hs.respostaVisualOrdem(PlayerAtual2);

        }

        public void SetIndexPlayerTurn(int index)
        {
            indexPlayerAtual = index;
        }
        public void NextTurn()
        {
            indexPlayerAtual = (1 + indexPlayerAtual) % (GetClientesCount);
        }




    }


}
