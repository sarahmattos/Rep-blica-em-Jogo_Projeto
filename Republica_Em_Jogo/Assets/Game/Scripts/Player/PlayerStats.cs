using Unity.Netcode;
using UnityEngine;
using Game.managers;
using Logger = Game.Tools.Logger;
using System;
using System.Collections.Generic;

namespace Game.Player {

    public class PlayerStats : NetworkBehaviour
    {
        IReadOnlyDictionary<ulong, NetworkClient> clientsConnected => NetworkManager.Singleton.ConnectedClients;
        [SerializeField] private Color cor;
        [SerializeField] private int maxTerritorio;
        [SerializeField] private Objetivo objetivo;

        [SerializeField] private string nome;
        [SerializeField] private int eleitores;


        public int playerID => (int)OwnerClientId;
        //public int playerID => (int)NetworkManager.Singleton.LocalClientId;
        public Color Cor { get => cor; }
        public Objetivo Objetivo { get => objetivo;}
        public string Nome { get => nome;}
        public int Eleitores { get => eleitores; }

        public event Action<PlayerStats> initializeStats;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(TurnManager.Instance == null) return;

            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            NetworkManager.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;

        }
        private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
        {

            switch (sceneEvent.SceneEventType)
            {
                 
                case SceneEventType.Load:
                    {
                        if (sceneEvent.SceneName == GameDataconfig.Instance.GameSceneName)
                        {
                            InitializeStats();
                            initializeStats?.Invoke(this);
                            Logger.Instance.LogWarning("INVOKING INIT STATS AQUI");
                            
                        }
                        break;
                    }
            };
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.S)) 
                if (initializeStats == null) Logger.Instance.LogInfo("initStats vazio?");
            else Logger.Instance.LogInfo("TA CHEIO!");

        }

        public void InitializeStats()
        {
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            maxTerritorio = GameDataconfig.Instance.territoriosInScene;
            eleitores = maxTerritorio / clientsConnected.Count;
            nome = string.Concat("jogador ", playerID);
        }



    }
}

