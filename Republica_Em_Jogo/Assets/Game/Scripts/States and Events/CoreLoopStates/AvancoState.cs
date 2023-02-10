using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Game.Territorio;

namespace Game
{
    public enum AvancoStatus
    {
        SELECT_BAIRRO,
        SELECT_VIZINHO,
        PROCESSAMENTO
    }

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
        private AvancoData avancoData = new AvancoData();
        public AvancoData AvancoData => avancoData;

        private void SetPairValues()
        {
            StatePairValues.Add(
                AvancoStatus.SELECT_BAIRRO,
                GetComponentInChildren<SelectBairroAvancoState>()
            );
            StatePairValues.Add(
                AvancoStatus.SELECT_VIZINHO,
                GetComponentInChildren<SelecVizinhoAvancoState>()
            );
            StatePairValues.Add(
                AvancoStatus.PROCESSAMENTO,
                GetComponentInChildren<ProcessaAvancoState>()
            );
        }

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
            contagemAvancosRodada = 0;
            SetAvancoStateServerRpc(0);
        }

        public override void ExitState() { }

        private void AvancoIndexMuda(int previousValue, int newValue)
        {
            currentState?.InvokeSaida();
            currentState = statePairValues[(AvancoStatus)newValue];
            estadoMuda?.Invoke((AvancoStatus)newValue);

            currentState.InvokeEntrada();
            AcrescentaRodadaNaSaidaUltimoAvancoState(previousValue);
        }

        private void AcrescentaRodadaNaSaidaUltimoAvancoState(int previousvalue)
        {
            int ultimoAvancoStateIndex = (int)AvancoStatus.PROCESSAMENTO;
            if (previousvalue == ultimoAvancoStateIndex)
            {
                contagemAvancosRodada++;
                avancoData.ClearData();
            }
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
    }

}
