using Game.Player;
using Game.Territorio;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tools;

namespace Game
{
    public class ProjetoState : State
    {
        [SerializeField] private Baralho baralho;
        public string explicaTexto, explicaTextoCorpo;
        private UICoreLoop uiCore;
        

        public override void EnterState()
        {
            if (TurnManager.Instance.LocalIsCurrent)
            {
                baralho.baralhoManager(true);
                uiCore.MostrarAvisoEstado(explicaTexto, explicaTextoCorpo);
            }
        }

        public override void ExitState()
        {
            PlayerStatsManager.Instance.GetLocalPlayerStats()?.BairrosInControl.MudarHabilitado(false);
            baralho.baralhoManager(false);
            SetUpZona.Instance.AllBairros.MudarInativity(false);
        }

        private void Start()
        {
            baralho = FindObjectOfType<Baralho>();
            uiCore = FindObjectOfType<UICoreLoop>();
        }


    }

}
