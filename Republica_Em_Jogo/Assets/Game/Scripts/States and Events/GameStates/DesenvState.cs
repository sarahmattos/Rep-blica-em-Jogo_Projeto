using System.Collections;
using System.Collections.Generic;
using Game.Tools;


namespace Game
{
    public class DesenvState : State
    {
        public override void EnterState()
        {
            Logger.Instance.LogInfo("Enter state: DesenvState");

        }

        public override void ExitState()
        {
            Logger.Instance.LogInfo("Exit state: DesenvState");

        }
    }

}