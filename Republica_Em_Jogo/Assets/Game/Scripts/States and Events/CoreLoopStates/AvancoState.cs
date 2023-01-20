using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AvancoState : State
    {
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: AVAN�O");

        }

        public override void ExitState()
        {
        }
    }
}

