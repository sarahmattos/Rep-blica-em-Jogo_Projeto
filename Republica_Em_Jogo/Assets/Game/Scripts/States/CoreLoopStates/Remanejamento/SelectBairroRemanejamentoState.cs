using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class SelectBairroRemanejamentoState : State
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
            if (!TurnManager.Instance.LocalIsCurrent) return;

            remanejamentoData.ResetSelectedBairros();
            MudarHabilitadoInteragivelBairros(true);
            InscreverClickInteragivelBairros();


        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;

            MudarHabilitadoInteragivelBairros(false);
            DesinscreverClickInteragivelBairros();
        }

        private void InscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in remanejamentoData.ParBairroEleitor.Keys)
            {
                bairro.Interagivel.Click += OnBairroClicado;
            }
        }


        private void DesinscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in remanejamentoData.ParBairroEleitor.Keys)
            {
                bairro.Interagivel.Click -= OnBairroClicado;
            }
        }

        private void OnBairroClicado(Bairro bairro)
        {
            remanejamentoData.BairroEscolhido = bairro;
            remanejamentoState.StateMachineController.NextStateServerRpc();

        }

        private void MudarHabilitadoInteragivelBairros(bool value)
        {
            foreach (Bairro bairro in remanejamentoData.ParBairroEleitor.Keys)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

    }
}
