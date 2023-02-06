using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;
using Game.Territorio;
using Game.Tools;

namespace Game
{
    public class EleicaoManager : NetworkBehaviour
    {
        public int somaEleitores;
        private float cadeirasCamara;
        private int cadeirasTotais;
        [SerializeField] private List<Bairro> todosBairros;
        private ZonaTerritorial[] zonasTerritoriais;
        void Start()
        {
           cadeirasTotais=12;
           zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
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
        public void ContaTotalEleitores()
        {
            todosBairros = GetBairros();
            somaEleitores = 0;
            foreach (Bairro bairro in todosBairros)
            {
                somaEleitores += bairro.SetUpBairro.Eleitores.contaEleitores;
            }

        }
        private List<Bairro> GetBairros()
        {
            List<Bairro> bairros = new List<Bairro>();
            for (int i = 0; i < zonasTerritoriais.Length; i++)
            {
                bairros.AddAll(zonasTerritoriais[i].Bairros);
            }

            return bairros;

        }
        public void CalculoEleicao(){
            if(NetworkManager.Singleton.IsServer){
                //SomaEleitoresPlayers();
                //CalculaNumeroEleicao();
                ContaTotalEleitores();
            }
            
        }
    }
}
