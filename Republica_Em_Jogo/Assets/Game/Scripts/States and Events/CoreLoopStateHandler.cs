using Game.Tools;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Game.Tools.Logger;

namespace Game
{
    public enum CoreLoopState
    {
        DISTRIBUICAO,
        AVANCO,
        PROJETO,
        RECOMPENSA
    }

    [RequireComponent(typeof(DistribuicaoState))]
    [RequireComponent(typeof(AvancoState))]
    [RequireComponent(typeof(ProjetoState))]
    [RequireComponent(typeof(RecompensaState))]
    public class CoreLoopStateHandler : NetworkSingleton<CoreLoopStateHandler>
    {
        public NetworkVariable<int> coreLoopIndex = new NetworkVariable<int>();
        private Dictionary<CoreLoopState, State> statePairValue = new Dictionary<CoreLoopState, State>();
        private State currentState;
        public Action<CoreLoopState> estadoMuda;
        public Dictionary<CoreLoopState, State> StatePairValue => statePairValue;
        private void Awake()
        {
            InitGameStateParValue();
            //trocar por entrar em distribuição quando entrar no game state desenvolvState
            currentState = statePairValue[CoreLoopState.DISTRIBUICAO];
            coreLoopIndex.OnValueChanged += IndexEstadoLoopMuda;

        }
        private void InitGameStateParValue()
        {
            StatePairValue.Add(CoreLoopState.DISTRIBUICAO, GetComponent<DistribuicaoState>());
            StatePairValue.Add(CoreLoopState.AVANCO, GetComponent<AvancoState>());
            StatePairValue.Add(CoreLoopState.PROJETO, GetComponent<ProjetoState>());
            StatePairValue.Add(CoreLoopState.RECOMPENSA, GetComponent<RecompensaState>());
        }

        public State DesenvState => GameStateHandler.Instance.GameStatePairValue[GameState.DESENVOLVIMENTO];

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public override void OnDestroy()
        {
            coreLoopIndex.OnValueChanged -= IndexEstadoLoopMuda;
        }

        public override void OnNetworkSpawn()
        {
            coreLoopIndex.OnValueChanged += IndexEstadoLoopMuda;
            DesenvState.Entrada += () =>{ ChangeDesenvStateServerRpc(0); };
        }

        public override void OnNetworkDespawn()
        {
            coreLoopIndex.OnValueChanged -= IndexEstadoLoopMuda;
            DesenvState.Entrada -= () => { ChangeDesenvStateServerRpc(0);};
        }



        private void IndexEstadoLoopMuda(int previousValue, int newValue)
        {
            currentState.InvokeSaida();
            currentState = statePairValue[(CoreLoopState)newValue];
            estadoMuda?.Invoke((CoreLoopState)newValue);
            currentState.InvokeEntrada();

        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeDesenvStateServerRpc(int state)
        {

            coreLoopIndex.Value = state;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextDesenvStateServerRpc()
        {
            if (coreLoopIndex.Value < statePairValue.Count-1)
            {
                coreLoopIndex.Value++;
            } else
            {

                //TODO: aqui não ta legal
                TurnManager.Instance.NextTurnServerRpc();

                coreLoopIndex.Value = 0;
            }
            Logger.Instance.LogWarning(string.Concat("Player ", TurnManager.Instance.GetCurrentPlayer, ", no Stado ", (CoreLoopState)coreLoopIndex.Value));


        }


    }

}
