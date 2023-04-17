using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;
using Game.UI;
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
            // UICoreLoop.Instance.UpdateTitleTextWithPlayerTag(" escolha um bairro para avançar.");
            bairrosInteragiveis = GetBairrosPodemInteragir();
            if (bairrosInteragiveis.Count == 0)
            {
                // CoreLoopStateHandler.Instance.NextStateServerRpc();
                return;
            }
            InscreverClickInteragivelBairros(bairrosInteragiveis);
            MudarHabilitadoInteragivelBairros(BairrosInteragiveis, true);
            SetBairrosInativity(GetBairroNaoPodemInteragir(), true);

        }

        public override void ExitState()
        {
            DesinscreverClickInteragivelBairros(bairrosInteragiveis);
            // DesabilitarInteragivelDosBairrosNaoEscolhidos();
            MudarHabilitadoInteragivelBairros(bairrosInteragiveis, false);
            BairrosInteragiveis.Clear();
            SetBairrosInativity(SetUpZona.Instance.AllBairros, false);

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

        private List<Bairro> GetBairroNaoPodemInteragir()
        {
            List<Bairro> bairros = SetUpZona.Instance.AllBairros;
            Debug.Log("conta de bairros: "+bairros);
            Debug.Log("diferença: "+ bairros.Except(GetBairrosPodemInteragir()).ToList().Count);
            return bairros.Except(GetBairrosPodemInteragir()).ToList();

            //             return (
            //     from Bairro bairro in BairrosInControl
            //     where !TemEleitoresParaAvancar(bairro)
            //     select bairro
            // ).ToList();
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
            bairro.Interagivel.ChangeSelectedBairro(true);
            avancoState.AvancoData.BairroPlayer = bairro;
            avancoState.NextAvancoStateServerRpc();
        }

        private void SetBairrosInativity(List<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarInativity(value);
            }

        }
    }
}
