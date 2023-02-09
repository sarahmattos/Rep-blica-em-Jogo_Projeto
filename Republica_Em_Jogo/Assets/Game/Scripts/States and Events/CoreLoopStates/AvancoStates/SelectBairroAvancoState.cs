using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private  const int eleitoresBairoMinParaAvancar = 2;
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
            if(!TurnManager.Instance.LocalIsCurrent) return;
            bairrosInteragiveis.AddRange(GetBairrosPodemInteragir());
            HabilitarInteragivelBairros(true);
            // ReceberBairroEscolhido Bairro Escolhido no click do Interagível
            //mover o escolhido para bairroEscolhido;
            
        }

        public override void ExitState()
        {
            BairrosInteragiveis?.Clear();
            HabilitarInteragivelBairros(false);
            // ReceberBairroEscolhido Bairro Escolhido no click do Interagível

        }

        private void HabilitarInteragivelBairros(bool value)
        {
            foreach (Bairro bairro in BairrosInteragiveis)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }

        }

        private void ReceberBairroEscolhido(Bairro bairro)
        {
            bairroEscolhido = bairro;
        }

        private List<Bairro> GetBairrosPodemInteragir() {
            List<Bairro> bairrosInControl = PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;    
            List<Bairro> bairrosPodemInteragir = bairrosInControl.Where(n => n.SetUpBairro.Eleitores.contaEleitores >= eleitoresBairoMinParaAvancar).Select(n => n).ToList();
            return bairrosPodemInteragir;
        }



    }
}
