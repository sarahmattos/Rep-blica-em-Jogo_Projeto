using System;
using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;
using System.Linq;
using Game.Tools;

namespace Game
{
    public class SelectVizinhoRemanejamentoState : State
    {
        [SerializeField] private InteragivelBackground interagivelBackground;
        private RemanejamentoState remanejamentoState;
        private RemanejamentoData remanejamentoData;
        private List<Bairro> VizinhosDoPlayerAtual
        {
            get
            {
                {
                    return (from Bairro bairro in remanejamentoData.BairroEscolhido.Vizinhos
                            where bairro.PlayerIDNoControl.Value == TurnManager.Instance.PlayerAtual
                            select bairro).ToList();
                }
            }
        }
        private void Start()
        {
            remanejamentoState = GetComponentInParent<RemanejamentoState>();
            remanejamentoData = remanejamentoState.RemanejamentoData;

        }

        public override void EnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            InscreverClickInteragivelBairros();
            VizinhosDoPlayerAtual.MudarInteragivel(true);
            VizinhosDoPlayerAtual.MudarInativity(false);
            SetUpZona.Instance.AllBairros.Except(VizinhosDoPlayerAtual).MudarInativity(true);
            interagivelBackground.MudaHabilitado(true);
            interagivelBackground.Click += CancelRemanejamento;
        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            DesinscreverClickInteragivelBairros();
            VizinhosDoPlayerAtual.MudarInteragivel(false);
            interagivelBackground.MudaHabilitado(false);
            interagivelBackground.Click -= CancelRemanejamento;

        }


        private void InscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in VizinhosDoPlayerAtual)
            {
                bairro.Interagivel.Click += OnBairroClicado;
            }
        }

        private void DesinscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in VizinhosDoPlayerAtual)
            {
                bairro.Interagivel.Click -= OnBairroClicado;
            }
        }

        private void OnBairroClicado(Bairro bairro)
        {
            bairro.Interagivel.ChangeSelectedBairro(true);
            remanejamentoData.VizinhoEscolhido = bairro;
            remanejamentoState.StateMachineController.NextStateServerRpc();
        }

        private void CancelRemanejamento()
        {
            remanejamentoState.StateMachineController.ChangeStateServerRpc(0);

        }

    }
}
