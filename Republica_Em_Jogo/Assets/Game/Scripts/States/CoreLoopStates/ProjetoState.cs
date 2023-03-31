using Game.Player;
using Game.Territorio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProjetoState : State
    {
        [SerializeField] private Baralho baralho;
        public string explicaTexto;
        private UICoreLoop uiCore;

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: PROJETO");
            if (TurnManager.Instance.LocalIsCurrent)
            {
                baralho.enabled = true;
                uiCore.ExplicaStateText.text = explicaTexto;
            }
            
            //EleicaoManager.Instance.CalculoEleicao();
        }

        public override void ExitState()
        {
            HabilitarBairrosPlayerLocal(false);
            baralho.enabled = false;

        }

        private void Start()
        {
            baralho.enabled = false;
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
