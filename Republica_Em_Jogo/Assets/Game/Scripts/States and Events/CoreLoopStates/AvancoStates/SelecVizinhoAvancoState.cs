using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Territorio;
using System.Linq;
using System;

namespace Game
{
    public class SelecVizinhoAvancoState : State
    {
        private AvancoState avancoState;
        private List<Bairro> BairrosVizinhos => avancoState.AvancoData.BairroEscolhido.Vizinhos.ToList();
        private List<Bairro> VizinhosInimigos;

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
            VizinhosInimigos = new List<Bairro>();
        }

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: SELECT VIZINHO.");
            if (!TurnManager.Instance.LocalIsCurrent)
                return;
            VizinhosInimigos = GetBairrosInimigos();
            MudaHabilitadoInteragivelBairros(VizinhosInimigos, true);
            InscreverClickInteragivelBairros(VizinhosInimigos);
        }

        public override void ExitState()
        {
            Tools.Logger.Instance.LogInfo("Exit State: SELECT VIZINHO.");
            if (!TurnManager.Instance.LocalIsCurrent)
                return;
            DesinscreverClickInteragivelBairros(VizinhosInimigos);
            MudaHabilitadoInteragivelBairros(VizinhosInimigos, false);
            VizinhosInimigos.Clear();
        }

        private void MudaHabilitadoInteragivelBairros(List<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        private void InscreverClickInteragivelBairros(List<Bairro> bairrosVizinhos)
        {
            foreach (Bairro bairro in bairrosVizinhos)
            {
                bairro.Interagivel.click += OnBairroClicado;
            }
        }

        private void DesinscreverClickInteragivelBairros(List<Bairro> bairrosVizinhos)
        {
            foreach (Bairro bairro in bairrosVizinhos)
            {
                bairro.Interagivel.click -= OnBairroClicado;
            }
        }

        private void OnBairroClicado(Bairro bairro)
        {
            avancoState.AvancoData.VizinhoEscolhido = bairro;
            avancoState.NextAvancoStateServerRpc();
        }

        private List<Bairro> GetBairrosInimigos()
        {
            return (
                from Bairro bairro in BairrosVizinhos
                where bairro.PlayerIDNoControl.Value != TurnManager.Instance.GetPlayerAtual
                select bairro
            ).ToList();
        }
    }
}
