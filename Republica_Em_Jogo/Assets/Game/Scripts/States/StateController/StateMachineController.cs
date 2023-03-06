using System;
using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class StateMachineController : NetworkBehaviour
    {
        private NetworkVariable<int> indexState = new NetworkVariable<int>();
        private List<State> states;
        [SerializeField] private State currentState;
        public event Action<int> estadoMuda;

        public State GetState(int index)
        {
            return states[index];
        }
        public State GetCurrentState()
        {
            return currentState;
        }

        public void ResetMachineState() {
            ChangeStateServerRpc(-1);
        }




        public void Initialize(List<State> statesOrdenados)
        {
            this.states = statesOrdenados;
            Debug.Log("initialize. State machine controller.");
            indexState.OnValueChanged += OnIndexStateMuda;

        }

        public void Finish()
        {
            indexState.OnValueChanged -= OnIndexStateMuda;
            if (!NetworkManager.Singleton.IsHost) return;
            indexState.Dispose();
        }

        private void OnIndexStateMuda(int previous, int next)
        {
            currentState?.InvokeSaida();
            currentState = states[next];
            currentState.InvokeEntrada();
            estadoMuda?.Invoke(next);

        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateServerRpc(int newIndex)
        {
            indexState.Value = newIndex;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextStateServerRpc()
        {
            indexState.Value = (indexState.Value +1) % states.Count;

        }



    }
}
