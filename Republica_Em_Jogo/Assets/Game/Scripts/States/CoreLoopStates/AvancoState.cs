using System;
using System.Collections.Generic;
using Unity.Netcode;
using Game.Territorio;
using Game.Player;
using Game.UI;
using Game.Tools;
using UnityEngine;

namespace Game
{
    public enum AvancoStatus
    {
        SELECT_BAIRRO,
        SELECT_VIZINHO,
        LANCA_DADOS,
        MIGRACAO
    }

    [RequireComponent(typeof(StateMachineController))]
    public class AvancoState : State
    {
        [SerializeField] private List<State> subStates;

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
        public string explicaTexto, explicaTextoCorpo;
        private UICoreLoop uiCore;

        private StateMachineController stateMachineController;
        public StateMachineController StateMachineController => stateMachineController;

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
            stateMachineController = GetComponent<StateMachineController>();
            stateMachineController.Initialize(subStates);
            stateMachineController.ResetMachineState();
            
            statePairValues = new Dictionary<AvancoStatus, State>();
            SetPairValues();
            dadosUiGeral = FindObjectOfType<DadosUiGeral>();
            uiCore = FindObjectOfType<UICoreLoop>();
        }

        public override void EnterState()
        {
            avancoData.ResetData();
            if (!TurnManager.Instance.LocalIsCurrent) return;
            stateMachineController.ChangeStateServerRpc(0);
            stateMachineController.saidaUltimoStado += AcrescentaRodada;
            //avancoStateIndex.OnValueChanged += AvancoIndexMuda;
            //SetAvancoStateServerRpc(0);
            uiCore.MostrarAvisoEstado(explicaTexto, explicaTextoCorpo);

        }

        public override void ExitState()
        {

            if (!TurnManager.Instance.LocalIsCurrent) return;
            stateMachineController.ChangeStateServerRpc(-1);
            //SetAvancoStateServerRpc(-1);
            stateMachineController.saidaUltimoStado -= AcrescentaRodada;
            bairrosPlayerAtual.MudarInteragivel(false);
            dadosUiGeral.resetaUiDadosServerRpc();
            dadosUiGeral.atualizaCorVizinhoDadoServerRpc(Color.white);

        }

        public override void OnNetworkDespawn()
        {
            stateMachineController.Finish();
        }


        //private void AvancoIndexMuda(int nextValue)
        //{
        //    //InvokeEventosStates(previousValue, nextValue);
        //    SaindoUltimoState();
        //}


        private void AcrescentaRodada()
        {
            //int ultimoAvancoStateIndex = (statePairValues.Count - 1);
            //if (previousValue == ultimoAvancoStateIndex)
            //{
                avancoData.ContagemRodada++;
                avancoData.ClearRodadaData();
            //}
        }

        //[ServerRpc(RequireOwnership = false)]
        //public void SetAvancoStateServerRpc(int index)
        //{
        //    avancoStateIndex.Value = index;
        //}

        //[ServerRpc(RequireOwnership = false)]
        //public void NextAvancoStateServerRpc()
        //{
        //    avancoStateIndex.Value = (avancoStateIndex.Value + 1) % (statePairValues.Count);
        //}


    }
}
