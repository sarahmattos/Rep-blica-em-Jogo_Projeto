using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public enum AvancoStatus
    {
        SELECT_BAIRRO,
        SELECT_VIZINHO,
        PROCESSAMENTO
    }

    [RequireComponent(typeof(SelectBairroAvancoState))]
    [RequireComponent(typeof(SelecVizinhoAvancoState))]
    [RequireComponent(typeof(ProcessaAvancoState))]
    public class AvancoState : State
    {
        private NetworkVariable<int> avancoStateIndex = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );
        private Dictionary<AvancoStatus, State> statePairValues;

        public Dictionary<AvancoStatus, State> StatePairValues => statePairValues;
        private State currentState;
        private Action<AvancoStatus> estadoMuda;
        private int contagemAvancosRodada;

        private void Start()
        {
            statePairValues = new Dictionary<AvancoStatus, State>();
            SetPairValues();
            avancoStateIndex.OnValueChanged += AvancoIndexMuda;
        }

        private void OnDestroy()
        {
            avancoStateIndex.OnValueChanged -= AvancoIndexMuda;
        }

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: AVAN�O");
            SetAvancoStateServerRpc(0);
            contagemAvancosRodada = 0;
        }

        public override void ExitState() { }

        private void SetPairValues()
        {
            StatePairValues.Add(
                AvancoStatus.SELECT_BAIRRO,
                GetComponent<SelectBairroAvancoState>()
            );
            StatePairValues.Add(
                AvancoStatus.SELECT_VIZINHO,
                GetComponent<SelecVizinhoAvancoState>()
            );
            StatePairValues.Add(AvancoStatus.PROCESSAMENTO, GetComponent<ProcessaAvancoState>());
        }

        private void AvancoIndexMuda(int previousValue, int newValue)
        {
            currentState?.InvokeSaida();
            currentState = statePairValues[(AvancoStatus)newValue];
            estadoMuda?.Invoke((AvancoStatus)newValue);

            currentState.InvokeEntrada();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetAvancoStateServerRpc(int index)
        {
            avancoStateIndex.Value = index;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextAvancoStateServerRpc()
        {
            avancoStateIndex.Value = (avancoStateIndex.Value + 1) % (statePairValues.Count);
        }

        #region  para testar a troca de estado
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                NextAvancoStateServerRpc();
        }
        #endregion
   
    }
}
