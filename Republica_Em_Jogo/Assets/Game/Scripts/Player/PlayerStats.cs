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
        
        public override void OnNetworkSpawn()
        {
            GameStateHandler.Instance.gameplaySceneLoad += InitializeStats;

        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.gameplaySceneLoad -= InitializeStats;

        }


        public void InitializeStats()
        {
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            //bairrosControl.instance.jogadoresConectados= clientsConnected.Count;
            maxTerritorio = GameDataconfig.Instance.territoriosInScene;
            eleitores = maxTerritorio / clientsConnected.Count;
            nome = string.Concat("jogador ", playerID);
        }



    }
}

