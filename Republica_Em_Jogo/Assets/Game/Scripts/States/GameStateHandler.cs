using Game.Tools;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Game.Tools.Logger;

namespace Game
{

    public enum GameState
    {
        MENU_SCENE_LOAD,
        GAMEPLAY_SCENE_LOAD,
        INICIALIZACAO,
        DESENVOLVIMENTO,
        ELEICOES,
        FIM_JOGO
    }

    public class GameStateHandler : NetworkSingleton<GameStateHandler>
    {
        // private NetworkVariable<int> gameStateIndex = new NetworkVariable<int>();
        [SerializeField] private List<State> states;
        // private State currentGameState;
        // public Action<GameState> estadoMuda;
        //para acessar os gerais do jogo, basta acessar StateParValue[GameState.ESTADO];
        // public List<State> States => states;
        private StateMachineController stateMachineController;
        public StateMachineController StateMachineController => stateMachineController;

        private void Awake()
        {
            stateMachineController = GetComponent<StateMachineController>();
            StateMachineController.Initialize(states);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //currentGameState = StatePairValue[GameState.MENU_SCENE_LOAD];
            NetworkManager.Singleton.OnServerStarted += () =>
            {
                StateMachineController.ChangeStateServerRpc((int)GameState.MENU_SCENE_LOAD);
            };


        }

        public override void OnDestroy()
        {
        }

        public override void OnNetworkSpawn()
        {

            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;

        }
        public override void OnNetworkDespawn()
        {
            stateMachineController.Finish();
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
        }


        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (sceneName == GameDataconfig.Instance.MenuSceneName)
            {
                stateMachineController.ChangeStateServerRpc((int)GameState.MENU_SCENE_LOAD);
            }

            if (sceneName == GameDataconfig.Instance.GameplaySceneName)
            {
                stateMachineController.ChangeStateServerRpc((int)GameState.GAMEPLAY_SCENE_LOAD);
            }
        }

    }



}

