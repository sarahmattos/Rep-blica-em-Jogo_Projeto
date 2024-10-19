using Game.Player;
using Game.Territorio;
using Game.Tools;
using Game.UI;
using UnityEngine;

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
                GameStateEmitter.SendMessage("Escolha uma ação.");

                baralho.baralhoManager(true);
                uiCore.MostrarAvisoEstado(explicaTexto, explicaTextoCorpo);
            }

        }

        public override void ExitState()
        {
            GameStateEmitter.SendMessage("");
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
