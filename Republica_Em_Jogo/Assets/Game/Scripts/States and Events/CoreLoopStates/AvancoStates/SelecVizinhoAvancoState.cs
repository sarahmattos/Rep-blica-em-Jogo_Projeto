using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SelecVizinhoAvancoState : State
    {
        private State nextState;

        private void Start()
        {
            nextState = GetComponent<ProcessaAvancoState>();
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {

        }

    }

}
