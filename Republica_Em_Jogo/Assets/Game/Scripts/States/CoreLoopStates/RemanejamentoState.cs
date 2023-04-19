using System.Collections.Generic;
using Game.Player;
using Game.Territorio;
using Game.Tools;
using Game.UI;
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
        public string explicaTexto, explicaTextoCorpo;
        private UICoreLoop uiCore;


        private void Start()
        {
            stateMachineController = GetComponent<StateMachineController>();
            stateMachineController.Initialize(subStates);
            stateMachineController.ResetMachineState();
            uiCore = FindObjectOfType<UICoreLoop>();
        }

        public override void EnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            remanejamentoData.ClearData();
            remanejamentoData.ArmazenarBairrosRemanejaveis();
            stateMachineController.ChangeStateServerRpc(0);
            uiCore.MostrarAvisoEstado(explicaTexto, explicaTextoCorpo);

        }


        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            remanejamentoData.ParBairroEleitorigualUm.Keys.MudarHabilitado(false);
            remanejamentoData.BairrosPlayerAtual.MudarInativity(false);
            stateMachineController.ResetMachineState();

        }

        public override void OnNetworkDespawn()
        {
            stateMachineController.Finish();
        }


    }
}
