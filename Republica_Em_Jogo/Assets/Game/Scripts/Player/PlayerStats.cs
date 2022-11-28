using Unity.Netcode;
using UnityEngine;
using Game.managers;
using Logger = Game.Tools.Logger;
using System;
using System.Collections.Generic;

namespace Game.Player {

    public class PlayerStats : NetworkBehaviour
    {
        //a linha abaixo só pode ser acessado pelo server (e host)
        //IReadOnlyDictionary<ulong, NetworkClient> clientsConnected => NetworkManager.Singleton.ConnectedClients;
        [SerializeField] private Color cor;
        [SerializeField] private int maxTerritorio;
        [SerializeField] private Objetivo objetivo;

        [SerializeField] private string nome;
        [SerializeField] private int eleitoresTotais;


        private void Awake()
        {
            //para os clients inscreverem métodos no initializePlayers
            GameStateHandler.Instance.initializePlayers += InitializeStats;

        }

        public override void OnDestroy()
        {
            //para os clients desinscrever métodos no initializePlayers
            GameStateHandler.Instance.initializePlayers += InitializeStats;
        }

        public int playerID => (int)OwnerClientId;
        
        public Color Cor { get => cor; }
        public Objetivo Objetivo { get => objetivo;}
        public string Nome { get => nome;}
        public int EleitoresTotais { get => eleitoresTotais; }
        
        public override void OnNetworkSpawn()
        {
            //host inscreve
            //host inscreve
            GameStateHandler.Instance.initializePlayers += InitializeStats;
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.initializePlayers -= InitializeStats;

        }

        public void InitializeStats()
        {
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            //bairrosControl.instance.jogadoresConectados= clientsConnected.Count;
            maxTerritorio = GameDataconfig.Instance.territoriosInScene;
            eleitoresTotais = maxTerritorio / /*clientsConnected.Count;*/  2;
            nome = string.Concat("jogador ", playerID);




            //TODO: isso, aqui, não ta legal. só provisório.
            //Logger.Instance.LogError(IsHost.ToString());  
            //if (IsServer)
            //{
            //    GameStateHandler.Instance.ChangeStateClientRpc((int)GameState.initialDistribuition);

            //}


        }



    }
}

