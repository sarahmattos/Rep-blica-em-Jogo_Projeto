using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DistribuicaoState : State
    {
        [SerializeField] private RecursosCartaManager rc;
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("EnterState: DISTRIBUI��O");
            rc.conferirQuantidade();
        }

        public override void ExitState()
        {
        }
    }

}
