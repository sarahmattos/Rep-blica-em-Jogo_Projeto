using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class ProcessaRemanejamentoState : State
    {
        private RemanejamentoState remanejamentoState;
        private RemanejamentoData remanejamentoData;

        private void Start()
        {
            remanejamentoState = GetComponentInParent<RemanejamentoState>();
            remanejamentoData = remanejamentoState.RemanejamentoData;

        }
        public override void EnterState()
        {
            Tools.Logger.Instance.LogPlayerAction("Moveu 1 eleitor para "+remanejamentoData.VizinhoEscolhido);

            if (!TurnManager.Instance.LocalIsCurrent) return;
            StartCoroutine(TaskProcessaRemanejamento());

        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            StopCoroutine(TaskProcessaRemanejamento());
        }

        private void RemoveEleitorBairroEscolhido()
        {
            remanejamentoData.BairroEscolhido.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(-1);
        }
        private void AdicionaEleitorVizinhoEscolhido()
        {

            remanejamentoData.VizinhoEscolhido.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(1);
        }

        private void DecrementaEleitorInBairroDictionary()
        {
            remanejamentoData.ParBairroEleitor[remanejamentoData.BairroEscolhido] += -1;
        }



        private IEnumerator TaskProcessaRemanejamento()
        {
            RemoveEleitorBairroEscolhido();
            AdicionaEleitorVizinhoEscolhido();
            DecrementaEleitorInBairroDictionary();
            if (remanejamentoData.GetEleitor(remanejamentoData.BairroEscolhido) < 1)
            {
                remanejamentoData.RemoveBairroParBairroEleitor(remanejamentoData.BairroEscolhido);
            }
            yield return new WaitForSeconds(0.2f);

            remanejamentoState.StateMachineController.NextStateServerRpc();
        }






    }
}
