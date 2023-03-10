using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;
using Game.Player;
using Game.Territorio;

namespace Game
{
    public class DadosUiGeral :  NetworkBehaviour
    {
        private NetworkVariable<FixedString4096Bytes> dadosPlayerServer = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<FixedString4096Bytes> dadosVizinhoServer = new NetworkVariable<FixedString4096Bytes>();
        public UIAvancoState uIAvancoState;
         int aux;
        // Start is called before the first frame update
        string player,vizinho;
        
        [ServerRpc(RequireOwnership = false)]
        public void atualizaUiDadosServerRpc(string _player, string _vizinho)
        {
            dadosPlayerServer.Value = _player;
            dadosVizinhoServer.Value=_vizinho;
        }
        [ServerRpc(RequireOwnership = false)]
        public void resetaUiDadosServerRpc()
        {
            dadosPlayerServer.Value = "Ataque: \n";;
            dadosVizinhoServer.Value= "Defesa: \n";
        }
         private void OnEnable()
            {
            //jogadores conectados
        
            dadosPlayerServer.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
               
                    aux++;
                    player=newValue.ToString();
                    Debug.Log("player: "+player);
                    atualizaUiDados();
                
                
            };
            dadosVizinhoServer.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
              
                    aux++;
                    vizinho=newValue.ToString();
                     Debug.Log("vizinho: "+vizinho);
                    atualizaUiDados();
                
            };
        
        }
        public void atualizaUiDados(){
            if(aux==2){
                uIAvancoState.UpdateTextDados2(player,vizinho);
                aux=0;
            }
            
        }
    }
}
