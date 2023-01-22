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
        private Dictionary<CoreLoopState, State> statePairValues = new Dictionary<CoreLoopState, State>();
        private State currentState;
        public Action<CoreLoopState> estadoMuda;
        public State CurrentState => currentState;
       //para acessar os estados de loop do jogo, basta acessar StateParValue[CoreLoopState.ESTADO];
        public Dictionary<CoreLoopState, State> StatePairValues => statePairValues;
        public State UltimoLoopState => StatePairValues[(CoreLoopState)StatePairValues.Count-1];
        public bool CurrentStateIgualUltimoState => currentState == UltimoLoopState;

        private void SetLoopStatePairValues()
        {
            StatePairValues.Add(CoreLoopState.DISTRIBUICAO, GetComponent<DistribuicaoState>());
            StatePairValues.Add(CoreLoopState.AVANCO, GetComponent<AvancoState>());
            StatePairValues.Add(CoreLoopState.PROJETO, GetComponent<ProjetoState>());
            StatePairValues.Add(CoreLoopState.RECOMPENSA, GetComponent<RecompensaState>());
        }

        private void Awake()
        {
            SetLoopStatePairValues();
        }

        private void Start()
        {
            currentState = statePairValues[CoreLoopState.DISTRIBUICAO];
           
            coreLoopIndex.OnValueChanged += IndexEstadoLoopMuda;
        }
        public override void OnDestroy()
        {
            coreLoopIndex.OnValueChanged -= IndexEstadoLoopMuda;
        }



        private void IndexEstadoLoopMuda(int previousValue, int newValue)
        {
            currentState.InvokeSaida();
            currentState = statePairValues[(CoreLoopState)newValue];
            estadoMuda?.Invoke((CoreLoopState)newValue);

            currentState.InvokeEntrada();

        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateServerRpc(int state)
        {

            coreLoopIndex.Value = state;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextStateServerRpc()
        {
            coreLoopIndex.Value = (coreLoopIndex.Value+1) % (statePairValues.Count);
        }
    }

}
