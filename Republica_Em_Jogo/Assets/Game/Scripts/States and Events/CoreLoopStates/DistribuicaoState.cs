using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Territorio;
using Game.Player;

namespace Game
{
    public class DistribuicaoState : State
    {
        [SerializeField]
        private RecursosCartaManager rc;

        [SerializeField]
        private HudStatsJogador hs;
        private List<Bairro> bairrosDoPlayerAtual;

        public void Start()
        {
            bairrosDoPlayerAtual = new List<Bairro>();
            TurnManager.Instance.vezDoPlayerLocal += quandoVezPlayerLocal;
        }

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("EnterState: DISTRIBUI��O");
            bairrosDoPlayerAtual.AddRange(
                PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl
            );
            foreach (Bairro bairro in bairrosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(true);
            }
        }

        public override void ExitState()
        {
            hs.distribuicaoInicial = false;
            foreach (Bairro bairro in bairrosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(false);
            }
            bairrosDoPlayerAtual.Clear();
        }

        public void OnDestroy()
        {
            TurnManager.Instance.vezDoPlayerLocal -= quandoVezPlayerLocal;
        }

        public void quandoVezPlayerLocal(bool value)
        {
            if (value)
            {
                hs.distribuicaoInicial = true;
                rc.conferirQuantidade();
            }
        }
    }
}
