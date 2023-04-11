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
        
        public string explicaTexto;
        private UICoreLoop uiCore;

        [SerializeField]
        private HudStatsJogador hs;
        private List<Bairro> bairrosDoPlayerAtual;
        private RodadaController rodadaController;

        public void Start()
        {
            bairrosDoPlayerAtual = new List<Bairro>();
            TurnManager.Instance.vezDoPlayerLocal += quandoVezPlayerLocal;
            uiCore = FindObjectOfType<UICoreLoop>();
        }

        public override void EnterState()
        {
            // Tools.Logger.Instance.LogPlayerAction("Distribuindo eleitores");
            if(!TurnManager.Instance.LocalIsCurrent) return;
            bairrosDoPlayerAtual.AddRange(
                PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl
            );
            foreach (Bairro bairro in bairrosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(true);
            }
             rodadaController = FindObjectOfType<RodadaController>();
            
            
            uiCore.MostrarAvisoEstado(explicaTexto);
        }
        public override void ExitState()
        {
            if(!TurnManager.Instance.LocalIsCurrent) return;
            hs.distribuicaoInicial = false;
            foreach (Bairro bairro in bairrosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(false);
            }
            bairrosDoPlayerAtual.Clear();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
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
