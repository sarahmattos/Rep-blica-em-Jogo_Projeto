using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private const int eleitoresBairroMinParaAvancar = 2;
        private AvancoState avancoState;
        private List<Bairro> bairrosInteragiveis;
        public List<Bairro> BairrosInteragiveis => bairrosInteragiveis;

        private void Start()
        {
            bairrosInteragiveis = new List<Bairro>();
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: SELECT BAIRRO.");
            bairrosInteragiveis = GetBairrosPodemInteragir();
            InscreverClickInteragivelBairros(bairrosInteragiveis);
            MudarHabilitadoInteragivelBairros(BairrosInteragiveis, true);
            

        }

        public override void ExitState()
        {
            Tools.Logger.Instance.LogInfo("Exit State: SELECT BAIRRO.");
            DesinscreverClickInteragivelBairros(bairrosInteragiveis);
            // DesabilitarInteragivelDosBairrosNaoEscolhidos();
            MudarHabilitadoInteragivelBairros(bairrosInteragiveis, false);
            BairrosInteragiveis?.Clear();
        }

        private void InscreverClickInteragivelBairros(List<Bairro> bairros) {
            foreach (Bairro bairro in bairrosInteragiveis)
            {
                bairro.Interagivel.click += OnBairroClicado;
            }
        }
        private void DesinscreverClickInteragivelBairros(List<Bairro> bairros) {
            
            foreach (Bairro bairro in bairrosInteragiveis)
            {
                bairro.Interagivel.click -= OnBairroClicado;
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
            // List<Bairro> bairrosInControl => PlayerStatsManager.Instance
            //     .GetPlayerStatsDoPlayerAtual()
            //     .BairrosInControl;
            
            // List<Bairro> bairrosPodemInteragir = bairrosInControl
            //     .Where(n => n.SetUpBairro.Eleitores.contaEleitores >= eleitoresBairroMinParaAvancar)
            //     .Select(n => n)
            //     .ToList();
            // return bairrosPodemInteragir;

             return (
                from Bairro bairro in PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl
                where bairro.SetUpBairro.Eleitores.contaEleitores >= eleitoresBairroMinParaAvancar 
                select bairro
            ).ToList();
        }

        private void OnBairroClicado(Bairro bairro)
        {
            avancoState.AvancoData.BairroEscolhido = bairro;
            avancoState.NextAvancoStateServerRpc();
        }



        
    }
}
