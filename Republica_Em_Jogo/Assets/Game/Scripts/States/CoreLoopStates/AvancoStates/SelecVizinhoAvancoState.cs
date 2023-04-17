using System.Collections.Generic;
using Game.Territorio;
using System.Linq;
using Game.Player;
using Game.UI;

namespace Game
{
    public class SelecVizinhoAvancoState : State
    {
        private AvancoState avancoState;
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
        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
            // VizinhosInimigos = new List<Bairro>();
        }

        public override void EnterState()
        {
            // UICoreLoop.Instance.UpdateTitleTextWithPlayerTag(", agora escolha um vizinho.");

            MudaHabilitadoInteragivelBairros(VizinhosInimigos, true);
            InscreverClickInteragivelBairros(VizinhosInimigos);
            SetBairrosInativity(GetVizinhosInimigosNaoPodemInteragir(), true);
        }

        public override void ExitState()
        {
            DesinscreverClickInteragivelBairros(VizinhosInimigos);
            MudaHabilitadoInteragivelBairros(VizinhosInimigos, false);
            SetBairrosInativity(SetUpZona.Instance.AllBairros, false);

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
            bairro.Interagivel.ChangeSelectedBairro(true);
            avancoState.AvancoData.BairroVizinho = bairro;
            avancoState.NextAvancoStateServerRpc();
        }

        private List<Bairro> GetVizinhosInimigosNaoPodemInteragir()
        {
            List<Bairro> bairros = SetUpZona.Instance.AllBairros;
            Tools.Logger.Instance.LogInfo("Bairros totais: " + bairros.Count);
            // int playerIdVizinho = BairrosVizinhos[0].PlayerIDNoControl.Value;
            // List<Bairro> bairrosPlayerVizinho = PlayerStatsManager.Instance.GetPlayerStats(playerIdVizinho).BairrosInControl;
            return bairros.Except(VizinhosInimigos).ToList();
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
