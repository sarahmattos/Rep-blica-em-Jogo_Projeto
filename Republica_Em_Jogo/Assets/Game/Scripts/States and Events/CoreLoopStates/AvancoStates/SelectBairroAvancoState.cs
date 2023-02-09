using System.Collections.Generic;
using Game.Player;
using Game.Territorio;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private AvancoState avancoState;

        private List<Bairro> bairrosInteragiveis;
        private Bairro bairroEscolhido;

        public List<Bairro> BairrosInteragiveis => bairrosInteragiveis; 

        private void Start()
        {
            avancoState = GetComponent<AvancoState>();
        }

        public override void EnterState()
        {
            if(!TurnManager.Instance.LocalIsCurrent) return;
            BairrosInteragiveis?.Clear();

            //Se o player local é o player atual
            // PlayerAtualBairros = PlayerStats.Instantiate.meusBairros;
            HabilitarInteragivelBairros(true);
        }

        public override void ExitState()
        {
            HabilitarInteragivelBairros(false);
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

        // private Bairro[] GetBairrosPodemInteragir() {
        //     //pergar bairros playerStats do player loca
        //     //Verificar se o bairro pode ser interagivel (se eleitores count > 1).
        //     // return Bairro[]
        //     // BairrosInteragiveis.Add();
        // }



    }
}
