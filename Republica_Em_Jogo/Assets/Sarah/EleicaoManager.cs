using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;

namespace Game
{
    public class EleicaoManager : NetworkManager
    {
        private int somaEleitores;
        private float cadeirasCamara;
        private int cadeirasTotais;
        void Start()
        {
           cadeirasTotais=12;
        //server vai passar todos playerstats e primeiro somar todos os valores e dpsfazer o calculo
        }
        private void SomaEleitoresPlayers()
        {
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                Debug.Log("player "+stats.playerID+": "+stats.EleitoresTotais);
                somaEleitores =+ stats.EleitoresTotais;
            }
        }
        private void CalculaNumeroEleicao(){
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                cadeirasCamara= Mathf.Round((stats.EleitoresTotais*cadeirasTotais)/somaEleitores);
                Debug.Log("player "+stats.playerID+" tem "+cadeirasCamara+" cadeiras.");
            }
        }
        // Update is called once per frame
        void Update()
        {
        
        }

        public void CalculoEleicao(){
            if(NetworkManager.Singleton.IsServer){
                SomaEleitoresPlayers();
                CalculaNumeroEleicao();
            }
            
        }
    }
}
