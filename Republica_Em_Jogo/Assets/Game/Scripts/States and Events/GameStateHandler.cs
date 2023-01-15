using Game.Tools;
using System;
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
        INICIALIZACAO,
        DESENVOLVIMENTO,
        ELEICOES,
        //FIM_JOGO

    }


    [RequireComponent(typeof(MenuLoadState))]
    [RequireComponent(typeof(GameplayLoadState))]
    [RequireComponent(typeof(InicializaState))]
    [RequireComponent(typeof(DesenvState))]
    [RequireComponent(typeof(EleicaoState))]

    public class GameStateHandler : NetworkSingleton<GameStateHandler>
    {
        private NetworkVariable<int> gameStateIndex = new NetworkVariable<int>();
        private Dictionary<GameState, State> gameStatePairValue = new Dictionary<GameState, State>();
        private State currentGameState;
        public Action<GameState> estadoMuda;
        public Dictionary<GameState, State> GameStatePairValue => gameStatePairValue;

        private void Awake()
        {
            InitGameStateParValue();
            currentGameState = GameStatePairValue[GameState.MENU_SCENE_LOAD];

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            gameStateIndex.OnValueChanged += IndexEstadoJogoMuda;

        }
        public override void OnDestroy()
        {
            gameStateIndex.OnValueChanged -= IndexEstadoJogoMuda;
        }

        private void InitGameStateParValue()
        {
            GameStatePairValue.Add(GameState.MENU_SCENE_LOAD, GetComponent<MenuLoadState>());
            GameStatePairValue.Add(GameState.GAMEPLAY_SCENE_LOAD, GetComponent< GameplayLoadState>());
            GameStatePairValue.Add(GameState.INICIALIZACAO, GetComponent<InicializaState>());
            GameStatePairValue.Add(GameState.DESENVOLVIMENTO, GetComponent<DesenvState>());
            GameStatePairValue.Add(GameState.ELEICOES, GetComponent<EleicaoState>());
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeStateServerRpc(int indexState)
        {
            gameStateIndex.Value = indexState;

        }

        [ServerRpc(RequireOwnership = false)]
        public void NextStateServerRPC()
        {
            gameStateIndex.Value++;
            //TODO: restringir o acrescimo de estado?
        }



        private void IndexEstadoJogoMuda(int previous, int next)
        {

            currentGameState.InvokeSaida();
            currentGameState = GameStatePairValue[(GameState)next];
            estadoMuda?.Invoke((GameState)next);
            currentGameState.InvokeEntrada();
            Logger.Instance.LogWarning(string.Concat("Index de estado: ", next," ("+(GameState)next)+")");

        }


        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;

        }
        public override void OnNetworkDespawn()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;

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

    }



}

