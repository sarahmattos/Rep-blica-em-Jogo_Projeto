using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class FimJogoState : State
    {
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Fim de jogo");
        }

        public override void ExitState()
        {
        }
    }
}

