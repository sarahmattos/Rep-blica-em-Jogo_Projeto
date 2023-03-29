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

        public override void EnterState()
        {
            Tools.Logger.Instance.LogPlayerAction("Escolhendo projeto.");
            if (TurnManager.Instance.LocalIsCurrent)
            {
                baralho.enabled = true;
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
