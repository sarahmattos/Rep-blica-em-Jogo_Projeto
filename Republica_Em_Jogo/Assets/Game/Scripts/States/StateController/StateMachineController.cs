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
        private State currentState;
        public event Action<int> estadoMuda;

        public State GetState(int index)
        {
            return states[index];
        }
        public State GetCurrentState()
        {
            return currentState;
        }

        public void ResetMachineState()
        {
            ChangeStateServerRpc(-1);
        }


        public void Initialize(List<State> statesOrdenados)
        {
            this.states = statesOrdenados;
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
            Tools.Logger.Instance.LogError(string.Concat("previous",previous," next", next));
            if (previous != -1) 
            { 
                currentState?.InvokeSaida();
            }
            if (next != -1)
            {
                currentState = states[next];
                currentState.InvokeEntrada();
            }
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
            indexState.Value = (indexState.Value + 1) % states.Count;

        }



    }
}
