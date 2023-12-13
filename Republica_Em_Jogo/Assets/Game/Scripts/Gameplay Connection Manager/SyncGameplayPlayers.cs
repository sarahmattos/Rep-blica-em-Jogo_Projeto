using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class SyncGameplayPlayers : NetworkBehaviour
    {
        [SerializeField] private Canvas CanvasUiCarregamento;
        [SerializeField] private TMP_Text LoadingMessageText;
        // [SerializeField] private string disconnectionPlayerMessage;

        public State GameplaySceneLoad => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD);

        private void Start()
        {
            CanvasUiCarregamento.enabled = GameDataconfig.Instance.DevConfig.MostraUISyncCarregamentoPlayers;
            GameplaySceneLoad.Saida += OnGameplaySceneLoad;
        }

        public override void OnDestroy()
        {
            GameplaySceneLoad.Saida -= OnGameplaySceneLoad;

        }

        public override void OnNetworkSpawn()
        {
            if (GameDataconfig.Instance.DevConfig.MostraUISyncCarregamentoPlayers) return;

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            if (GameDataconfig.Instance.DevConfig.MostraUISyncCarregamentoPlayers) return;
            
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            base.OnNetworkDespawn();

        }


        private void OnClientConnect(ulong id)
        {

            if (GameDataconfig.Instance.MaxConnections == NetworkManager.Singleton.ConnectedClients.Count)
            {
                CanvasUiCarregamento.enabled = false;
            }
        }

        private void OnClientDisconnect(ulong id)
        {
            CanvasUiCarregamento.enabled = true;

        }




        private void OnGameplaySceneLoad()
        {
            CanvasUiCarregamento.enabled = false;
        }







    }
}
