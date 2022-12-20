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
        MENU_SCENE_LOAD,
        GAMEPLAY_SCENE_LOAD,
        //INITIALIZE_PLAYERS,
        INITIALIZE_TERRITORIO,
        DESENVOLVIMENTO,
        ELEICOES

    }

    public class GameStateHandler : NetworkSingleton<GameStateHandler>
    {
        private NetworkVariable<int> gameStateIndex = new NetworkVariable<int>();
        //private GameState currentState = GameState.MenuSceneLoad;

        public event Action menuSceneLoad; //stateIndex = 0;
        public event Action gameplaySceneLoad;//stateIndex = 1;
        public event Action initializeTerritorio;//stateIndex = 2;
        public event Action desenvolvimento; // stateIndex = 3;
        public event Action eleicoes; //stateIndex = 4;
        private void Awake()
        {
            gameStateIndex.OnValueChanged += onStateIndexChanged;

        }

        public override void OnDestroy()
        {
            gameStateIndex.OnValueChanged -= onStateIndexChanged;
        }


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateServerRpc(int state)
        {
            Logger.Instance.LogWarning(string.Concat("Stado atual de jogo: ", (GameState)state));
            gameStateIndex.Value = state;

        }

        //[ServerRpc(RequireOwnership = false)]
        //public void NextStateServerRPC()
        //{
        //    gameStateIndex.Value++;
        //    //gameStateIndex.Value = (GameState)((int)currentState + 1);
        //}

        public override void OnNetworkSpawn()
        {
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
                    initializeTerritorio?.Invoke();
                    break;
                case 3:
                    desenvolvimento?.Invoke();
                    break;
            }

        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (sceneName == GameDataconfig.Instance.MenuSceneName)
            {
                ChangeStateServerRpc((int)GameState.MENU_SCENE_LOAD);
            }

            if (sceneName == GameDataconfig.Instance.GameSceneName)
            {
                ChangeStateServerRpc((int)GameState.GAMEPLAY_SCENE_LOAD);
            }
        }

        private void OnGameplaySceneLoad()
        {
            if (!IsServer) return;
            StartCoroutine(Inicializacao_Territorio());
        }

        private IEnumerator Inicializacao_Territorio()
        {
            yield return new WaitForSeconds(1);
            ChangeStateServerRpc((int)GameState.INITIALIZE_TERRITORIO);

        }





    }



}

