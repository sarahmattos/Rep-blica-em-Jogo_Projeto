using Unity.Netcode;
using UnityEngine;
using Game.managers;
using Logger = Game.Tools.Logger;
using System;

namespace Game.Player {

    public class PlayerStats : NetworkBehaviour
    {
        [SerializeField] private Color cor;
        [SerializeField] private Objetivo objetivo;

        [SerializeField] private string nome;
        [SerializeField] private int eleitores;

        public int playerID => (int)OwnerClientId;
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

            //InitializeStats();
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            NetworkManager.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;


        }
        private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
        {
            Logger.Instance.LogInfo("olha aqui: "+sceneEvent.SceneEventType.ToString());


            switch (sceneEvent.SceneEventType)
            {
                case SceneEventType.LoadComplete:
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


        public void InitializeStats()
        {
            Debug.Log("Initializando");
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            nome = string.Concat("jogador ", playerID);
        }



    }
}

