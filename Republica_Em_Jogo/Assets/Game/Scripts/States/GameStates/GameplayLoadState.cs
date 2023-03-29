using System;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class GameplayLoadState : State
    {
        private GameStateHandler stateHandler => GameStateHandler.Instance;
        public override void EnterState()
        {
            if (!IsServer) return;
            //stateHandler.NextStateServerRPC();
            StartCoroutine(SperaEVai(1));
        }

        public override void ExitState()
        {
        }


        private IEnumerator SperaEVai(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            stateHandler.StateMachineController.NextStateServerRpc();

        }

    }
}