using Game.Player;
using Game.Territorio;
using Game.UI;
using System.Collections.Generic;

namespace Game
{
    public class InicialDistribuicaoState : State
    {

        private List<Bairro> bairrosDoPlayerLocal => PlayerStatsManager.Instance.GetLocalPlayerStats().BairrosInControl;
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter: distribuicao inicial.");
            // TurnManager.Instance.UpdateTurn();

            if (!TurnManager.Instance.LocalIsCurrent) return;
            Tools.Logger.Instance.LogInfo("VOU JOGAR.");
            HudStatsJogador hudStatsJogador = FindObjectOfType<HudStatsJogador>();
            hudStatsJogador.distribuicaoInicial = true;
            hudStatsJogador.ChamatPlayerInicioRodada();
            MudarHabilitadoBairrosDoPlayer(true);


        }

        public override void ExitState()
        {
            Tools.Logger.Instance.LogInfo("EXIT: distribuicao inicial.");

            MudarHabilitadoBairrosDoPlayer(false);

            TurnManager.Instance.UpdateTurn();

        }


        private void MudarHabilitadoBairrosDoPlayer(bool value)
        {
            foreach (Bairro bairro in bairrosDoPlayerLocal)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }


    }
}
