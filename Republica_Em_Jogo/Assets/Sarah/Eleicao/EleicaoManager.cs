using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;
using Game.Territorio;
using Game.Tools;
using Unity.Collections;
using Game.UI;

namespace Game
{
    public class EleicaoManager : NetworkBehaviour
    {
        
        private NetworkVariable<FixedString4096Bytes> EleicaoText = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<int> conectados = new NetworkVariable<int>();
        public HudStatsJogador hs;
        public static EleicaoManager Instance;
        public int somaEleitores;
        public float[] cadeirasCamara;
        public int cadeirasTotais, minCadeirasVotacao;
        [SerializeField] private List<Bairro> todosBairros;
        private ZonaTerritorial[] zonasTerritoriais;
        public int numConectados;
        public int[] eleitoresPlayers;
        private SetUpZona setUpZona;
        private string cadeiras;
        void Start()
        {
           cadeirasTotais=12;
           zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
           setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            Instance = this;
            minCadeirasVotacao=7;
            
        }
        [ServerRpc(RequireOwnership = false)]
        public void ClientsConectServerRpc()
        {
            conectados.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }
        public void cadeirasInicial(){
            cadeirasCamara = new float[numConectados];
            for(int i=0;i<cadeirasCamara.Length;i++){
                    cadeirasCamara[i]=cadeirasTotais/numConectados;
                    if(i==(int)NetworkManager.Singleton.LocalClientId){
                        hs.cadeirasUi(cadeirasCamara[i]);
                    }
                }
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
        public void CalcularCadeiras()
        {
            cadeiras="";
            eleitoresPlayers = new int[numConectados];
            cadeirasCamara = new float[numConectados];
            setUpZona.SepararBairrosPorPlayer(eleitoresPlayers, numConectados);
            for (int i = 0; i < eleitoresPlayers.Length; i++)
            {
                float aux = ((float)eleitoresPlayers[i] * (float)cadeirasTotais) / (float)somaEleitores;
                cadeirasCamara[i] = Mathf.Round(aux);
                 if(i==(int)NetworkManager.Singleton.LocalClientId){
                        hs.cadeirasUi(cadeirasCamara[i]);
                    }
                      
                if(NetworkManager.Singleton.IsServer){
                    cadeiras += "Player "+i+" tem: "+cadeirasCamara[i].ToString() +" cadeiras"+ "\n";
                    EleicaoText.Value = cadeiras;
                }
            }
        }
        public void CalculoEleicao(){
                ContaTotalEleitores();
                CalcularCadeiras();
            
        }
        private void OnEnable()
    {
        //jogadores conectados
        EleicaoText.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
        {
            if (newValue != "")
            {
                UIeleicao.Instance.MostrarCadeiras(newValue.ToString());
                //Debug.Log(newValue.ToString());
            }
        };
        conectados.OnValueChanged += (int previousValue, int newValue) =>
        {
            if (newValue != 0)
            {
                numConectados=newValue;
                cadeirasInicial();
            }
        };
    }
    
    }
}