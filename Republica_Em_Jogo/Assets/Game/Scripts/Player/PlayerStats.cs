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
            nome = string.Concat("jogador ", playerID);
        }



    }
}

