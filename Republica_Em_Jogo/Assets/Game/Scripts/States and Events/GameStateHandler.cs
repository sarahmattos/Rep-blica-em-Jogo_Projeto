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
        FIM_JOGO
    }

    [RequireComponent(typeof(MenuLoadState))]
    [RequireComponent(typeof(GameplayLoadState))]
    [RequireComponent(typeof(InicializacaoState))]
    [RequireComponent(typeof(DesenvolvimentoState))]
    [RequireComponent(typeof(EleicaoState))]
    [RequireComponent(typeof(FimJogoState))]    
    public class GameStateHandler : NetworkSingleton<GameStateHandler>
    {
        private NetworkVariable<int> gameStateIndex = new NetworkVariable<int>();
        private Dictionary<GameState, State> statePairValue = new Dictionary<GameState, State>();
        private State currentGameState;
        public Action<GameState> estadoMuda;
        //para acessar os gerais do jogo, basta acessar StateParValue[GameState.ESTADO];
        public Dictionary<GameState, State> StatePairValue => statePairValue;

        private void Awake()
        {
            SetGameStatePairValues();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            //currentGameState = StatePairValue[GameState.MENU_SCENE_LOAD];
            gameStateIndex.OnValueChanged += IndexEstadoJogoMuda;

            NetworkManager.Singleton.OnServerStarted += () =>
            {
                ChangeStateServerRpc((int)GameState.MENU_SCENE_LOAD);
            };


        }

        public override void OnDestroy()
        {
            gameStateIndex.OnValueChanged -= IndexEstadoJogoMuda;
            statePairValue.Clear();
            if(!IsHost) return;
            gameStateIndex.Dispose();

        }

        private void SetGameStatePairValues()
        {
            StatePairValue.Add(GameState.MENU_SCENE_LOAD, GetComponent<MenuLoadState>());
            StatePairValue.Add(GameState.GAMEPLAY_SCENE_LOAD, GetComponent< GameplayLoadState>());
            StatePairValue.Add(GameState.INICIALIZACAO, GetComponent<InicializacaoState>());
            StatePairValue.Add(GameState.DESENVOLVIMENTO, GetComponent<DesenvolvimentoState>());
            StatePairValue.Add(GameState.ELEICOES, GetComponent<EleicaoState>());
            statePairValue.Add(GameState.FIM_JOGO, GetComponent<FimJogoState>());
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
            //TODO: restrigir o acrescimo no index em rela��o ao n�mero de estados que existem
        }



        private void IndexEstadoJogoMuda(int previous, int next)
        {
            currentGameState?.InvokeSaida();
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
            if (sceneName == GameDataConfig.Instance.MenuSceneName)
            {
                ChangeStateServerRpc((int)GameState.MENU_SCENE_LOAD);
            }

            if (sceneName == GameDataConfig.Instance.GameplaySceneName)
            {
                ChangeStateServerRpc((int)GameState.GAMEPLAY_SCENE_LOAD);
            }
        }

    }



}

