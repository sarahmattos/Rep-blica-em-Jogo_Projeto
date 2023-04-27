using System.Collections.Generic;
using Game.Territorio;
using System.Linq;
using Game.Player;
using Game.UI;
using Game.Tools;
using UnityEngine;

namespace Game
{
    public class SelecVizinhoAvancoState : State
    {
        private AvancoState avancoState;
        private DadosUiGeral dadosUiGeral;
        private List<Bairro> BairrosVizinhos => avancoState.AvancoData.BairroPlayer.Vizinhos.ToList();
        public List<Bairro> VizinhosInimigos
        {
            get
            {
                return (from Bairro bairro in BairrosVizinhos
                        where bairro.PlayerIDNoControl.Value != TurnManager.Instance.PlayerAtual
                        select bairro
                ).ToList();
            }
        }

        private List<Bairro> VizinhosInimigosNaoPodemInteragir
        {
            get
            {
                return SetUpZona.Instance.AllBairros.Except(VizinhosInimigos).ToList();
            }
        }

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
             dadosUiGeral=FindObjectOfType<DadosUiGeral>();
        }

        public override void EnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            VizinhosInimigos.MudarHabilitado(true);
            InscreverClickInteragivelBairros(VizinhosInimigos);
            VizinhosInimigosNaoPodemInteragir.MudarInativity(true);
        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            DesinscreverClickInteragivelBairros(VizinhosInimigos);
            VizinhosInimigos.MudarHabilitado(false);
            SetUpZona.Instance.AllBairros.MudarInativity(false);

        }

        private void InscreverClickInteragivelBairros(List<Bairro> bairrosVizinhos)
        {
            foreach (Bairro bairro in bairrosVizinhos)
            {
                bairro.Interagivel.Click += OnBairroClicado;
            }
        }

        private void DesinscreverClickInteragivelBairros(List<Bairro> bairrosVizinhos)
        {
            foreach (Bairro bairro in bairrosVizinhos)
            {
                bairro.Interagivel.Click -= OnBairroClicado;
            }
        }

        private void OnBairroClicado(Bairro bairro)
        {
            dadosUiGeral.atualizaCorVizinhoDadoServerRpc(Color.white);
            bairro.Interagivel.ChangeSelectedBairro(true);
            avancoState.AvancoData.BairroVizinho = bairro;
            foreach (PlayerStats playerStats in PlayerStatsManager.Instance.AllPlayerStats)
                {
                    if(bairro.PlayerIDNoControl.Value == playerStats.playerID ){
                         dadosUiGeral.atualizaCorVizinhoDadoServerRpc(playerStats.Cor);
                    }
                }
            avancoState.StateMachineController.NextStateServerRpc();
            
        }


    }
}
