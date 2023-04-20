using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;
using Game.UI;
using UnityEngine;
using Game.Tools;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private const int eleitoresBairroMinParaAvancar = 2;
        private AvancoState avancoState;
        private List<Bairro> BairrosInControl => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;


        public List<Bairro> BairrosInteragiveis
        {
            get
            {
                return (
                    from Bairro bairro in BairrosInControl
                    where TemEleitoresParaAvancar(bairro) && TemVizinhoInimigo(bairro)
                    select bairro
                ).ToList();
            }
        }

        private List<Bairro> bairroNaoPodemInteragir
        {
            get
            {
                List<Bairro> bairros = SetUpZona.Instance.AllBairros;
                return bairros.Except(BairrosInteragiveis).ToList();
            }
        }

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            if (BairrosInteragiveis.Count == 0)
            {
                //TODO: retornar para o jogador alguma interface indicando que nao ha bairros para avancar
                return;
            }
            BairrosInteragiveis.MudarHabilitado(true);
            bairroNaoPodemInteragir.MudarInativity(true);
            InscreverClickInteragivelBairros(BairrosInteragiveis);

        }

        public override void ExitState()
        {
            if (BairrosInteragiveis.Count == 0) return;

            BairrosInteragiveis.MudarHabilitado(false);
            SetUpZona.Instance.AllBairros.MudarInativity(false);
            DesinscreverClickInteragivelBairros(BairrosInteragiveis);


        }

        private void InscreverClickInteragivelBairros(List<Bairro> bairros)
        {
            foreach (Bairro bairro in BairrosInteragiveis)
            {
                bairro.Interagivel.Click += OnBairroClicado;
            }
        }

        private void DesinscreverClickInteragivelBairros(List<Bairro> bairros)
        {
            foreach (Bairro bairro in BairrosInteragiveis)
            {
                bairro.Interagivel.Click -= OnBairroClicado;
            }
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
            avancoState.StateMachineController.NextStateServerRpc();
        }


    }
}
