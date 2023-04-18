using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Game.Territorio;
using Game.Player;
using Game.UI;

namespace Game
{
    public enum AvancoStatus
    {
        SELECT_BAIRRO,
        SELECT_VIZINHO,
        LANCA_DADOS,
        MIGRACAO
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
        private AvancoData avancoData = new AvancoData();
        public AvancoData AvancoData => avancoData;
        DadosUiGeral dadosUiGeral;
        public string explicaTexto,explicaTextoCorpo;
        private UICoreLoop uiCore;

        public List<Bairro> bairrosPlayerAtual => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;

        private void SetPairValues()
        {
            StatePairValues.Add(AvancoStatus.SELECT_BAIRRO, GetComponentInChildren<SelectBairroAvancoState>());
            StatePairValues.Add(AvancoStatus.SELECT_VIZINHO, GetComponentInChildren<SelecVizinhoAvancoState>());
            StatePairValues.Add(AvancoStatus.LANCA_DADOS, GetComponentInChildren<LancamentoDadosAvancoState>());
            StatePairValues.Add(AvancoStatus.MIGRACAO, GetComponentInChildren<MigraEleitorAvancoState>());
        }

        private void Start()
        {
            statePairValues = new Dictionary<AvancoStatus, State>();
            SetPairValues();
            dadosUiGeral=FindObjectOfType<DadosUiGeral>();
             uiCore = FindObjectOfType<UICoreLoop>();
        }

        public override void EnterState()
        {
            avancoData.ResetData();
            if (!TurnManager.Instance.LocalIsCurrent) return;
            avancoStateIndex.OnValueChanged += AvancoIndexMuda;
            SetAvancoStateServerRpc(0);
            uiCore.MostrarAvisoEstado(explicaTexto,explicaTextoCorpo);

        }

        public override void ExitState()
        {

            if (!TurnManager.Instance.LocalIsCurrent) return;
            avancoStateIndex.OnValueChanged -= AvancoIndexMuda;
            HabilitarBairrosPlayerAtual(false);
            SetAvancoStateServerRpc(-1);
            dadosUiGeral.resetaUiDadosServerRpc();


        }


        private void AvancoIndexMuda(int previousValue, int nextValue)
        {
            InvokeEventosStates(previousValue, nextValue);
            SaindoUltimoState(previousValue);
        }

        private void InvokeEventosStates(int previousValue, int nextValue)
        {
            currentState?.InvokeSaida();
            currentState = statePairValues[(AvancoStatus)nextValue];
            estadoMuda?.Invoke((AvancoStatus)nextValue);
            currentState.InvokeEntrada();
        }

        private void SaindoUltimoState(int previousValue)
        {
            int ultimoAvancoStateIndex = (statePairValues.Count - 1);
            if (previousValue == ultimoAvancoStateIndex)
            {
                avancoData.ContagemRodada++;
                avancoData.ClearRodadaData();
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

        public void HabilitarBairrosPlayerAtual(bool value)
        {
            foreach (Bairro bairro in bairrosPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }
    }
}
