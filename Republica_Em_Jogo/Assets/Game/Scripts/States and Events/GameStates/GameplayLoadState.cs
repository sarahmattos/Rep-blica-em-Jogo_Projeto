using System;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Game
{
    public class GameplayLoadState : State
    {
        private GameStateHandler stateHandler => GameStateHandler.Instance;

        public override void EnterState()
        {
            if (!IsServer) return;
            stateHandler.NextStateServerRPC();
        }

        public override void ExitState()
        {
        }



    }
}