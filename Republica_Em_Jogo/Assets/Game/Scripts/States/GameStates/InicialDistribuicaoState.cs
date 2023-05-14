using Game.Player;
using Game.Territorio;
using Game.UI;
using System.Collections.Generic;

namespace Game
{
    public class InicialDistribuicaoState : State
    {

        private List<Bairro> bairrosDoPlayerLocal => PlayerStatsManager.Instance.GetLocalPlayerStats().BairrosInControl;
        private HudStatsJogador HudStatsJogador => HudStatsJogador.Instance;
        public override void EnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            HudStatsJogador.eleitoresNovosDeProjeto += () => MudarHabilitadoBairrosDoPlayer(true);
            // MudarHabilitadoBairrosDoPlayer(true);

        }

        public override void ExitState()
        {
            HudStatsJogador.eleitoresNovosDeProjeto -= () => MudarHabilitadoBairrosDoPlayer(true);
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
