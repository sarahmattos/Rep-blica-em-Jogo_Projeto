using System;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;
using System.Collections;
using Game.Player;

namespace Game
{
    public class GameplayLoadState : State
    {
        private GameStateHandler stateHandler => GameStateHandler.Instance;
        public override void EnterState()
        {
            PlayerStatsManager.Instance.FindAllPlayerStats();
            if (!IsServer) return;
            // stateHandler.StateMachineController.NextStateServerRpc();
            StartCoroutine(SperaEVai(1f));
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