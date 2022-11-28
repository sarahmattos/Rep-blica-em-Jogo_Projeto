using Game.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Game.Tools.Logger;

namespace Game {

    public enum GameState
    {
        MenuSceneLoad,
        gameplaySceneLoad,
        initializePlayers,
        initialDistribuition,
        //loopGame,
        //eleicoes,
        //endGame
    }

    public class GameStateHandler : NetworkSingleton<GameStateHandler>
    {
        private NetworkVariable<int> gameStateIndex = new NetworkVariable<int>();
        //private GameState currentState = GameState.MenuSceneLoad;

        public event Action menuSceneLoad; //stateIndex = 0;
        public event Action gameplaySceneLoad;//stateIndex = 1;
        public event Action initializePlayers;//stateIndex = 2;
        public event Action initialDistribution;//stateIndex = 3;

        private void Awake()
        {
            gameStateIndex.OnValueChanged += teste;
            gameStateIndex.OnValueChanged += onStateIndexChanged;
        }

        public override void OnDestroy()
        {
            gameStateIndex.OnValueChanged -= teste;
            gameStateIndex.OnValueChanged -= onStateIndexChanged;
        }

        private void teste(int previousValue, int newValue)
        {
            Logger.Instance.LogWarning("state index atual.: " + newValue);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        [ClientRpc]
        public void ChangeStateClientRpc(int state)
        {
            Logger.Instance.LogWarning(string.Concat("Stado atual de jogo: ", (GameState)state));
            gameStateIndex.Value = state;
            //currentState = (GameState)state;
        }

        //[ServerRpc(RequireOwnership = false)]
        //public void NextStateServerRPC()
        //{
        //    gameStateIndex.Value++;
        //    currentState = (GameState)((int)currentState + 1);
        //}

        public override void OnNetworkSpawn()
        {
            Logger.Instance.LogInfo("Chamei no OnSpawn no Game State.");
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            gameplaySceneLoad += OnGameplaySceneLoad;
        }
        public override void OnNetworkDespawn()
        {
            gameStateIndex.OnValueChanged -= onStateIndexChanged;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            
            gameplaySceneLoad -= OnGameplaySceneLoad;
        }

        private void onStateIndexChanged(int previous, int next)
        {
            switch(next)
            {
                case 0:
                    menuSceneLoad?.Invoke();
                    break;
                case 1:
                    gameplaySceneLoad?.Invoke();
                    break;
                case 2:
                    initializePlayers?.Invoke();
                    break;
                case 3:
                    initialDistribution?.Invoke();
                    break;
            }

        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (sceneName == GameDataconfig.Instance.MenuSceneName)
            {
                ChangeStateClientRpc((int)GameState.MenuSceneLoad);
            }

            if (sceneName == GameDataconfig.Instance.GameSceneName)
            {
                ChangeStateClientRpc((int)GameState.gameplaySceneLoad);
            }
        }

        private void OnGameplaySceneLoad()
        {

            if (!IsServer) return;
            ChangeStateClientRpc((int)GameState.initializePlayers);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                //if (!IsServer) return;
                ChangeStateClientRpc((int)GameState.initialDistribuition);
            }
        }
    }

}

