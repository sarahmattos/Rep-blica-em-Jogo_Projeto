using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProjetoState : State
    {
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: PROJETO");

        }

        public override void ExitState()
        {
        }
    }

}
