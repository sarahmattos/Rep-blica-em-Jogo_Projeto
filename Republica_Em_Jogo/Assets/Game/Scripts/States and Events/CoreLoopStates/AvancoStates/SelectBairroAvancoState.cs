using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private const int eleitoresBairoMinParaAvancar = 2;
        private AvancoState avancoState;
        private List<Bairro> bairrosInteragiveis;
        private Bairro bairroEscolhido;
        public List<Bairro> BairrosInteragiveis => bairrosInteragiveis;

        private void Start()
        {
            bairrosInteragiveis = new List<Bairro>();
            avancoState = GetComponent<AvancoState>();
        }

        public override void EnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent)
                return;
            bairrosInteragiveis.AddRange(GetBairrosPodemInteragir());
            HabilitarInteragivelBairros(BairrosInteragiveis, true);
            foreach (Bairro bairro in bairrosInteragiveis)
            {
                bairro.Interagivel.click += OnBairroClicado;
            }
        }

        public override void ExitState()
        {
            foreach (Bairro bairro in bairrosInteragiveis)
            {
                bairro.Interagivel.click -= OnBairroClicado;
            }
            DesabilitarInteragivelDosBairrosNaoEscolhidos();
            BairrosInteragiveis?.Clear();
        }

        private void HabilitarInteragivelBairros(List<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        private List<Bairro> GetBairrosPodemInteragir()
        {
            List<Bairro> bairrosInControl = PlayerStatsManager.Instance
                .GetPlayerStatsDoPlayerAtual()
                .BairrosInControl;
            List<Bairro> bairrosPodemInteragir = bairrosInControl
                .Where(n => n.SetUpBairro.Eleitores.contaEleitores >= eleitoresBairoMinParaAvancar)
                .Select(n => n)
                .ToList();
            return bairrosPodemInteragir;
        }

        private void OnBairroClicado(Bairro bairro)
        {
            bairroEscolhido = bairro;
            bairrosInteragiveis.Remove(bairroEscolhido);

            avancoState.NextAvancoStateServerRpc();
        }

        private void DesabilitarInteragivelDosBairrosNaoEscolhidos()
        {
            HabilitarInteragivelBairros(bairrosInteragiveis, false);
        }


        
    }
}
