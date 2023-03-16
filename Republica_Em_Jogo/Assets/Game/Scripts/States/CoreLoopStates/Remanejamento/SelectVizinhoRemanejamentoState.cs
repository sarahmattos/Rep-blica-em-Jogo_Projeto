using System;
using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;
using System.Linq;

namespace Game
{
    public class SelectVizinhoRemanejamentoState : State
    {

        private RemanejamentoState remanejamentoState;
        private RemanejamentoData remanejamentoData;
        private List<Bairro> vizinhosDoPlayerAtual;

        private void Start()
        {
            remanejamentoState = GetComponentInParent<RemanejamentoState>();
            remanejamentoData = remanejamentoState.RemanejamentoData;

        }

        public override void EnterState()
        {

            Tools.Logger.Instance.LogInfo("Escolha um bairro vizinho para receber eleitores.");
            if (!TurnManager.Instance.LocalIsCurrent) return;
            vizinhosDoPlayerAtual = GetVizinhosDoPlayerAtual();
            InscreverClickInteragivelBairros();
            MudaHabilitadoInteragivelBairros(true);
        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            DesinscreverClickInteragivelBairros();
            MudaHabilitadoInteragivelBairros(false);
        }


        public List<Bairro> GetVizinhosDoPlayerAtual()
        {
            return (from Bairro bairro in remanejamentoData.BairroEscolhido.Vizinhos
                    where bairro.PlayerIDNoControl.Value == TurnManager.Instance.PlayerAtual
                    select bairro).ToList();
        }


        private void MudaHabilitadoInteragivelBairros(bool value)
        {
            foreach (Bairro bairro in vizinhosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        private void InscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in vizinhosDoPlayerAtual)
            {
                bairro.Interagivel.click += OnBairroClicado;
            }
        }

        private void DesinscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in vizinhosDoPlayerAtual)
            {
                bairro.Interagivel.click -= OnBairroClicado;
            }
        }

        private void OnBairroClicado(Bairro bairro)
        {
            remanejamentoData.VizinhoEscolhido = bairro;
            remanejamentoState.StateMachineController.NextStateServerRpc();
        }


    }
}