using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;
using Unity.Collections;
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
        private int aux, aux2;
        public int jogadoreOn;
        public string valoresServer;

        public NetworkVariable<int> somaEleitores = new NetworkVariable<int>(0);
        public NetworkVariable<int> count = new NetworkVariable<int>(0);
        public NetworkVariable<FixedString4096Bytes> cadeirasNetwork = new NetworkVariable<FixedString4096Bytes>("");
        void Start()
        {
           cadeirasTotais=12;
           Instance = this;
            aux2 = 0;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SomaEleitoresPlayersServerRpc(int valor)
        {   
            Debug.Log("entrou no server");
            count.Value =NetworkManager.Singleton.ConnectedClientsIds.Count;
            somaEleitores.Value += valor;
            
        }
        [ServerRpc(RequireOwnership = false)]
        private void MostrarUiServerRpc(string valor)
        {
            Debug.Log("server recebe: "+valor);
            cadeirasNetwork.Value += valor;
            Debug.Log(cadeirasNetwork.Value);
        }
        [ServerRpc(RequireOwnership = false)]
        public void DefaultServerRpc()
        {
            cadeirasNetwork.Value = "";
            somaEleitores.Value = 0;
        }
        private void OnEnable()
            {
                somaEleitores.OnValueChanged += (int previousValue, int newValue) =>
                {
                    aux++;
                    Debug.Log(aux);
                    Debug.Log(jogadoreOn);
                    if(aux==jogadoreOn){
                        aux = 0;
                        Debug.Log("entrou1");
                        somaEleitoresLocal = newValue;
                        Debug.Log("valor soma " + newValue);
                        CalculaNumeroEleicao();
                        
                    }
                    
                };
                count.OnValueChanged += (int previousValue, int newValue) =>
                {
                    jogadoreOn = newValue;
                };
            cadeirasNetwork.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                aux2++;
                Debug.Log(aux2);
                Debug.Log(jogadoreOn);
                if (aux2 == jogadoreOn)
                {
                    aux2 = 0;
                    Debug.Log("entrou2");
                    Debug.Log("valor final indo praui " + newValue.ToString());
                    UIeleicao.Instance.MostrarCadeiras(newValue.ToString());
                    
                    
                }
            };
        }
        private void CalculaNumeroEleicao(){
            Debug.Log("calculo");
            float soma = (eleitoresLocal * cadeirasTotais) / somaEleitoresLocal;
            cadeirasCamara = Mathf.Round(soma);
            string frase = "player " + id + " tem " + cadeirasCamara + " cadeiras." + "\n";
            Debug.Log(frase);
            MostrarUiServerRpc(frase);
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
        public void resetauxs()
        {
            aux = 0;
            aux2 = 0;
        }
    }
}
