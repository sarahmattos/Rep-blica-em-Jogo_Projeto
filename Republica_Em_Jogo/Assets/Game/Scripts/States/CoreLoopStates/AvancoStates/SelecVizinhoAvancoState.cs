using System.Collections.Generic;
using Game.Territorio;
using System.Linq;
using Game.Player;
using Game.UI;
using Game.Tools;

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
            bairro.Interagivel.ChangeSelectedBairro(true);
            avancoState.AvancoData.BairroVizinho = bairro;
            avancoState.StateMachineController.NextStateServerRpc();
        }


    }
}
