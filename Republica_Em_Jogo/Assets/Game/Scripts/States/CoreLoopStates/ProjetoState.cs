using Game.Player;
using Game.Territorio;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProjetoState : State
    {
        [SerializeField] private Baralho baralho;
        public string explicaTexto,explicaTextoCorpo;
        private UICoreLoop uiCore;

        public override void EnterState()
        {
            // Tools.Logger.Instance.LogPlayerAction("Escolhendo projeto.");
            if (TurnManager.Instance.LocalIsCurrent)
            {
                //baralho.enabled = true;
                baralho.baralhoManager(true);
                uiCore.MostrarAvisoEstado(explicaTexto,explicaTextoCorpo);
            }
            
            //EleicaoManager.Instance.CalculoEleicao();
        }

        public override void ExitState()
        {
            HabilitarBairrosPlayerLocal(false);
            baralho.baralhoManager(false);
        }

        private void Start()
        {   
            baralho = FindObjectOfType<Baralho>();
            uiCore = FindObjectOfType<UICoreLoop>();
        }

        public void OnDestroy()
        {

        }


        private void HabilitarBairrosPlayerLocal(bool value)
        {
            List<Bairro> bairros = PlayerStatsManager.Instance.GetLocalPlayerStats().BairrosInControl;
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

    }

}
