using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;
//using Game.UI;

namespace Game
{
    public class EleicaoManager : NetworkBehaviour
    {
        public static EleicaoManager Instance;
        public int somaEleitoresLocal;
        public float cadeirasCamara;
        private int cadeirasTotais;
        private int eleitoresLocal;
        private int id;
        private int aux;
        public int jogadoreOn;

        public NetworkVariable<int> somaEleitores = new NetworkVariable<int>(0);
        public NetworkVariable<int> count = new NetworkVariable<int>(0);
        //private HudStatsJogador hs;
        void Start()
        {
           // hs = FindObjectOfType<HudStatsJogador>();
           cadeirasTotais=12;
           Instance = this;
           //CalculoEleicao();
        //server vai passar todos playerstats e primeiro somar todos os valores e dpsfazer o calculo
        }

        [ServerRpc(RequireOwnership = false)]
        private void SomaEleitoresPlayersServerRpc(int valor)
        {   
            Debug.Log("entrou no server deveria ser duas vezes");
            count.Value =NetworkManager.Singleton.ConnectedClientsIds.Count;
            somaEleitores.Value += valor;
            
        }
         private void OnEnable()
            {
                somaEleitores.OnValueChanged += (int previousValue, int newValue) =>
                {
                    aux++;
                    Debug.Log(aux);
                    Debug.Log(jogadoreOn);
                    if(aux==jogadoreOn){
                        Debug.Log("entrou");
                        somaEleitoresLocal = newValue;
                        CalculaNumeroEleicao();
                        aux=0;
                    }
                    
                };
                count.OnValueChanged += (int previousValue, int newValue) =>
                {
                    jogadoreOn = newValue;
                };
            }
        private void CalculaNumeroEleicao(){
           
                cadeirasCamara= Mathf.Round((eleitoresLocal*cadeirasTotais)/somaEleitoresLocal);
                Debug.Log("player "+id+" tem "+cadeirasCamara+" cadeiras.");
            
        }
        // Update is called once per frame
        void Update()
        {
        
        }

        public void CalculoEleicao(){
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                if (stats.IsLocalPlayer)
                {
                    id=stats.playerID;
                    eleitoresLocal=stats.EleitoresTotais;
                   SomaEleitoresPlayersServerRpc(eleitoresLocal);
                }
            }
            
            
        }
    }
}
