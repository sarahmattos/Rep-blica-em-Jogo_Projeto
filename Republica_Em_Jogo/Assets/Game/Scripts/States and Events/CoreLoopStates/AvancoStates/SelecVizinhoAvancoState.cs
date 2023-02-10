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
            Tools.Logger.Instance.LogInfo("Enter State: SELECT VIZINHO.");

        }

        public override void ExitState()
        {
            Tools.Logger.Instance.LogInfo("Exit State: SELECT VIZINHO.");

        }

    }

}
