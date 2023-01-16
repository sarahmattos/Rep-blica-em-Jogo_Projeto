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
        private Dictionary<GameState, State> statePairValue = new Dictionary<GameState, State>();
        private State currentGameState;
        public Action<GameState> estadoMuda;
        //para acessar os gerais do jogo, basta acessar StateParValue[GameState.ESTADO];
        public Dictionary<GameState, State> StatePairValue => statePairValue;


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SetGameStatePairValues();
            currentGameState = StatePairValue[GameState.MENU_SCENE_LOAD];

            gameStateIndex.OnValueChanged += IndexEstadoJogoMuda;

        }
        public override void OnDestroy()
        {
            gameStateIndex.OnValueChanged -= IndexEstadoJogoMuda;
        }

        private void SetGameStatePairValues()
        {
            StatePairValue.Add(GameState.MENU_SCENE_LOAD, GetComponent<MenuLoadState>());
            StatePairValue.Add(GameState.GAMEPLAY_SCENE_LOAD, GetComponent< GameplayLoadState>());
            StatePairValue.Add(GameState.INICIALIZACAO, GetComponent<InicializaState>());
            StatePairValue.Add(GameState.DESENVOLVIMENTO, GetComponent<DesenvState>());
            StatePairValue.Add(GameState.ELEICOES, GetComponent<EleicaoState>());
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
            //TODO: restrigir o acrescimo no index em relação ao número de estados que existem
        }



        private void IndexEstadoJogoMuda(int previous, int next)
        {

            currentGameState.InvokeSaida();
            currentGameState = StatePairValue[(GameState)next];
            estadoMuda?.Invoke((GameState)next);
            currentGameState.InvokeEntrada();
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

