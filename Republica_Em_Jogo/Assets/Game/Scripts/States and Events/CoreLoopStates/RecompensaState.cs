using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RecompensaState : State
    {
        public override void EnterState()
        {
        }

        public override void ExitState()
        {
            Game.Tools.Logger.Instance.LogError("Exit Recompensa State");
        }
    }

}
