using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;

namespace Game
{
    public class ControlePassarState : NetworkBehaviour
    {
        public static ControlePassarState instance;
        public NetworkVariable<int> QuantidadePlayerRecompensa = new NetworkVariable<int>(0);
        public bool distribuicaoProjeto = true;
        private bool primeiraDistribuicao = false;
        private RecursosCartaManager rc;
        private HudStatsJogador hs;
        private State inicDistribState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DISTRIBUI_INICIAL);

        void Start()
        {
            instance = this;
            rc = FindObjectOfType<RecursosCartaManager>();
            hs = FindObjectOfType<HudStatsJogador>();
            primeiraDistribuicao = true;
            // inicDistribState.Entrada += () =>
            // {
            //     Tools.Logger.Instance.LogWarning("mudar primeiraDistribuicao ok");

            //     primeiraDistribuicao = true;
            // };
        }

        public override void OnDestroy()
        {
            inicDistribState.Entrada -= () =>
            {
                primeiraDistribuicao = true;
            };
        }


        [ServerRpc(RequireOwnership = false)]
        public void AumentaValServerRpc()
        {
            QuantidadePlayerRecompensa.Value++;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DiminuiValServerRpc()
        {
            QuantidadePlayerRecompensa.Value--;
        }
        [ServerRpc(RequireOwnership = false)]
        public void PassarProjetoServerRpc()
        {
            Tools.Logger.Instance.LogError("erro controle passar sstatae");
            CoreLoopStateHandler.Instance.NextStateServerRpc();
        }


        private void OnEnable()
        {
            //jogadores conectados
            QuantidadePlayerRecompensa.OnValueChanged += (int previousValue, int newValue) =>
            {
                if (newValue == 0)
                {
                    if (NetworkManager.Singleton.IsServer)
                    {
                        PassarProjetoServerRpc();
                    }
                }
            };

        }

        //button passar stado
        public void passarState()
        {

            Tools.Logger.Instance.LogInfo("primeira distribuição: " + primeiraDistribuicao);

            if (distribuicaoProjeto == true)
            {
                DiminuiValServerRpc();
                distribuicaoProjeto = false;
                return;
            }

            if (primeiraDistribuicao)
            {
                //Configuração do botão da interface pós distribuição Inicial do Game State geral.
                if (!TurnManager.Instance.LocalIsUltimo)
                {
                    Tools.Logger.Instance.LogWarning("NÃO É O ÚLTIMO ok");
                    //Resentando o state machine parar que a mudança de estado funcione corretamente.
                    GameStateHandler.Instance.StateMachineController.ResetMachineState();
                    GameStateHandler.Instance.StateMachineController.ChangeStateServerRpc((int)GameState.DISTRIBUI_INICIAL);
                }
                else
                {
                    Tools.Logger.Instance.LogWarning(" ÚLTIMO");
                    GameStateHandler.Instance.StateMachineController.ResetMachineState();
                    GameStateHandler.Instance.StateMachineController.ChangeStateServerRpc((int)GameState.DESENVOLVIMENTO);

                }
                primeiraDistribuicao = false;
                return;
            }
            if (rc.chamarDistribuicao == false)
            {
                if (rc.comTrocaTrue == false)
                {
                    Tools.Logger.Instance.LogInfo("não é a primeira distribuição. ");
                    CoreLoopStateHandler.Instance.NextStateServerRpc();
                }
                return;

            }

        }

        public void MudacomTroca()
        {
            //botao final distribuicao
            rc.comTrocaTrue = false;
        }
    }
}
