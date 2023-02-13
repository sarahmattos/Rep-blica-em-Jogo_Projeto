using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProcessaAvancoState : State
    {
        private AvancoState avancoState;

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            //Provisório: só pra visualizar.
            avancoState.AvancoData.BairroEscolhido.Interagivel.MudarHabilitado(true);
            avancoState.AvancoData.VizinhoEscolhido.Interagivel.MudarHabilitado(true);
        }

        public override void ExitState()
        {
        }
    }
}
