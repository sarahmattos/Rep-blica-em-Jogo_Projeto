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
        private GameState currentState = GameState.MenuSceneLoad;

        public event Action menuSceneLoad;
        public event Action gameplaySceneLoad;
        public event Action initializePlayers;
        public event Action initialDistribution;
           
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateServerRPC(int index)
        {
            gameStateIndex.Value = index;
            currentState = (GameState)index;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextStateServerRPC()
        {
            gameStateIndex.Value++;
            currentState = (GameState)((int)currentState + 1);
        }

        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            gameStateIndex.OnValueChanged += onStateIndexChanged;
            gameplaySceneLoad += OnGameplaySceneLoad;
            initializePlayers += OnInitializePlayers;
            initialDistribution += OnInitialDistribution;
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
        

        public override void OnNetworkDespawn()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            gameplaySceneLoad -= OnGameplaySceneLoad;
            initializePlayers -= OnInitializePlayers;
            initialDistribution -= OnInitialDistribution;

        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (sceneName == GameDataconfig.Instance.MenuSceneName)
            {
                ChangeStateServerRPC(0);
            }

            if (sceneName == GameDataconfig.Instance.GameSceneName)
            {
                ChangeStateServerRPC(1);
                gameplaySceneLoad?.Invoke();
            }
        }

        private void OnGameplaySceneLoad()
        {
            initializePlayers?.Invoke();
            Logger.Instance.LogInfo("TESTE1: Cena carregada");
        }

        private void OnInitializePlayers()
        {
            initialDistribution?.Invoke();
            Logger.Instance.LogInfo("TESTE2: Players Initialized");
        }


        private void OnInitialDistribution()
        {
            Logger.Instance.LogInfo("TESTE3: Initial Distribution");

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                NextStateServerRPC();
            }
        }
    }

}

