using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProcessaAvancoState : State
    {
        private State nextState;

        private void Start()
        {
            nextState = GetComponent<SelectBairroAvancoState>();
        }


        public override void EnterState()
        {
        }

        public override void ExitState()
        {

            //desabilhitar interagivel do bairroEscolhido e bairroAlvo;
        }
    }
}
