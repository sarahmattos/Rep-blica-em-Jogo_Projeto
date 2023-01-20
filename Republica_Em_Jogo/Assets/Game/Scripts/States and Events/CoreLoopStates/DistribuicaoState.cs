using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DistribuicaoState : State
    {
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("EnterState: DISTRIBUI��O");
        }

        public override void ExitState()
        {
        }
    }

}
