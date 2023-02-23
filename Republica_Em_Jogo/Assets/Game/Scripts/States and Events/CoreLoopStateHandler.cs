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

    public class CoreLoopStateHandler : NetworkSingleton<CoreLoopStateHandler>
    {
        private NetworkVariable<int> coreLoopIndex = new NetworkVariable<int>(-1);
        private Dictionary<CoreLoopState, State> statePairValues;
        private State currentState;
        public State DesenvolvimentoState =>
            GameStateHandler.Instance.StatePairValue[GameState.DESENVOLVIMENTO];
        public Action<CoreLoopState> estadoMuda;

        public State CurrentState => currentState;

        //para acessar os estados de loop do jogo, basta acessar StateParValue[CoreLoopState.ESTADO];
        public Dictionary<CoreLoopState, State> StatePairValues => statePairValues;
        public State UltimoLoopState => StatePairValues[(CoreLoopState)StatePairValues.Count - 1];
        public bool CurrentStateIgualUltimoState => currentState == UltimoLoopState;

        private void SetLoopStatePairValues()
        {
            StatePairValues.Add(CoreLoopState.DISTRIBUICAO, GetComponentInChildren<DistribuicaoState>());
            StatePairValues.Add(CoreLoopState.AVANCO, GetComponentInChildren<AvancoState>());
            StatePairValues.Add(CoreLoopState.PROJETO, GetComponentInChildren<ProjetoState>());
            StatePairValues.Add(CoreLoopState.RECOMPENSA, GetComponentInChildren<RecompensaState>());
        }

        private void Awake()
        {
            statePairValues = new Dictionary<CoreLoopState, State>();
            SetLoopStatePairValues();
        }

        private void Start()
        {
            // currentState = statePairValues[CoreLoopState.DISTRIBUICAO];
            coreLoopIndex.OnValueChanged += IndexEstadoLoopMuda;
            if (!IsHost) return;
            DesenvolvimentoState.Entrada += () =>
            {
                ChangeStateServerRpc(0);
            };
        }

        public override void OnDestroy()
        {
            coreLoopIndex.OnValueChanged -= IndexEstadoLoopMuda;
            if (!IsHost) return;
            DesenvolvimentoState.Entrada -= () => ChangeStateServerRpc(0);
            coreLoopIndex.Dispose();
        }

        private void IndexEstadoLoopMuda(int previousValue, int newValue)
        {
            currentState?.InvokeSaida();
            currentState = statePairValues[(CoreLoopState)newValue];
            currentState.InvokeEntrada();
            estadoMuda?.Invoke((CoreLoopState)newValue);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateServerRpc(int state)
        {
            coreLoopIndex.Value = state;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextStateServerRpc()
        {
            coreLoopIndex.Value = (coreLoopIndex.Value + 1) % (statePairValues.Count);
        }
    }
}
