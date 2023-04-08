using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private const int eleitoresBairroMinParaAvancar = 2;
        private AvancoState avancoState;
        private List<Bairro> BairrosInControl =>
            PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;
        private List<Bairro> bairrosInteragiveis;
        public List<Bairro> BairrosInteragiveis => bairrosInteragiveis;

        private void Start()
        {
            bairrosInteragiveis = new List<Bairro>();
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            bairrosInteragiveis = GetBairrosPodemInteragir();
            if (bairrosInteragiveis.Count == 0)
            {
                // CoreLoopStateHandler.Instance.NextStateServerRpc();
                return;
            }
            InscreverClickInteragivelBairros(bairrosInteragiveis);
            MudarHabilitadoInteragivelBairros(BairrosInteragiveis, true);
        }

        public override void ExitState()
        {
            DesinscreverClickInteragivelBairros(bairrosInteragiveis);
            // DesabilitarInteragivelDosBairrosNaoEscolhidos();
            MudarHabilitadoInteragivelBairros(bairrosInteragiveis, false);
            BairrosInteragiveis.Clear();
        }

        private void InscreverClickInteragivelBairros(List<Bairro> bairros)
        {
            foreach (Bairro bairro in bairrosInteragiveis)
            {
                bairro.Interagivel.Click += OnBairroClicado;
            }
        }

        private void DesinscreverClickInteragivelBairros(List<Bairro> bairros)
        {
            foreach (Bairro bairro in bairrosInteragiveis)
            {
                bairro.Interagivel.Click -= OnBairroClicado;
            }
        }

        private void MudarHabilitadoInteragivelBairros(List<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        private List<Bairro> GetBairrosPodemInteragir()
        {
            return (
                from Bairro bairro in BairrosInControl
                where TemEleitoresParaAvancar(bairro) && TemVizinhoInimigo(bairro)
                select bairro
            ).ToList();
        }

        private bool TemEleitoresParaAvancar(Bairro bairro)
        {
            if (bairro.SetUpBairro.Eleitores.contaEleitores >= eleitoresBairroMinParaAvancar)
                return true;
            else
                return false;
        }

        private bool TemVizinhoInimigo(Bairro bairro)
        {
            foreach (Bairro vizinho in bairro.Vizinhos)
            {
                if (vizinho.PlayerIDNoControl.Value != bairro.PlayerIDNoControl.Value)
                    return true;
            }

            return false;
        }

        private void OnBairroClicado(Bairro bairro)
        {
            avancoState.AvancoData.BairroPlayer = bairro;
            avancoState.NextAvancoStateServerRpc();
        }
    }
}
