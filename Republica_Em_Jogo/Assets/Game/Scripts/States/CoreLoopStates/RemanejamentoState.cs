using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(StateMachineController))]
    public class RemanejamentoState : State
    {
        [SerializeField] private List<State> subStates;
        private StateMachineController stateMachineController;
        public StateMachineController StateMachineController => stateMachineController;
        private RemanejamentoData remanejamentoData = new RemanejamentoData();
        public RemanejamentoData RemanejamentoData => remanejamentoData;
        public string explicaTexto;
        private UICoreLoop uiCore;

        private void Start()
        {
            stateMachineController =  GetComponent<StateMachineController>();
            stateMachineController.Initialize(subStates);
            stateMachineController.ResetMachineState();
            uiCore = FindObjectOfType<UICoreLoop>();
        }

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter: Remanejamento");
            if(!TurnManager.Instance.LocalIsCurrent) return;

            remanejamentoData.ClearData();
            remanejamentoData.ArmazenarBairrosRemanejaveis();
            stateMachineController.ChangeStateServerRpc(0);
            uiCore.ExplicaStateText.text = explicaTexto;

        }


        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            MudarHabilitadoInteragivelBairros(false);
            stateMachineController.ResetMachineState();
            
        }

        public override void OnNetworkDespawn()
        {
            stateMachineController.Finish();
        }

        public void MudarHabilitadoInteragivelBairros(bool value) {
            foreach(Bairro bairro in remanejamentoData.ParBairroEleitor.Keys) {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }




    }
}
