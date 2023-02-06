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
        public float[] cadeirasCamara;
        public int cadeirasTotais;
        [SerializeField] private List<Bairro> todosBairros;
        private ZonaTerritorial[] zonasTerritoriais;
        public int numConectados;
        //public BairroArray[] bairrosPlayerSegmment;
        public int[] eleitoresPlayers;
        private SetUpZona setUpZona;
        void Start()
        {
           cadeirasTotais=12;
           zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
           setUpZona = GameObject.FindObjectOfType<SetUpZona>();
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
            numConectados = NetworkManager.Singleton.ConnectedClientsIds.Count;
            eleitoresPlayers = new int[numConectados];
            cadeirasCamara = new float[numConectados];
            setUpZona.SepararBairrosPorPlayer(eleitoresPlayers, numConectados);
            for (int i = 0; i < eleitoresPlayers.Length; i++)
            {
                float aux = ((float)eleitoresPlayers[i] * (float)cadeirasTotais) / (float)somaEleitores;
                Debug.Log(aux);
                cadeirasCamara[i] = Mathf.Round(aux);
                Debug.Log(cadeirasCamara[i]);
            }
            //bairrosPlayerSegmment = new BairroArray[numConectados];
            //setUpZona.SepararBairrosPorPlayer(bairrosPlayerSegmment, numConectados);
        }
        public void CalculoEleicao(){
            if(NetworkManager.Singleton.IsServer){
                ContaTotalEleitores();
                CalcularCadeiras();
            }
            
        }
    }
    /*[System.Serializable]
    public struct BairroArray
    {
        public List<Bairro> BairrosPorPlayer;
    }
    */
}
