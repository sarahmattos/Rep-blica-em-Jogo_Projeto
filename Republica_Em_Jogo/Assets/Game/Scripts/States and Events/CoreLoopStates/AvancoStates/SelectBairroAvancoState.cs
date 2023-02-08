using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SelectBairroAvancoState : State
    {
        private AvancoState avancoState;

        private void Start()
        {
            avancoState = GetComponent<AvancoState>();
        }

        public override void EnterState() { }

        public override void ExitState() { }
    }
}
