using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;

namespace Game
{
    public class ControlePassarState : NetworkBehaviour
    {
        public static ControlePassarState instance;
        public NetworkVariable<int> QuantidadePlayerRecompensa = new NetworkVariable<int>(0);
        public bool distribuicaoProjeto=false;
        private RecursosCartaManager rc;
        private HudStatsJogador hs;
        void Start()
        {
            instance = this;
            rc = FindObjectOfType<RecursosCartaManager>();
             hs = FindObjectOfType<HudStatsJogador>();
        }

        
        [ServerRpc(RequireOwnership = false)]
        public void AumentaValServerRpc()
        {
            QuantidadePlayerRecompensa.Value++;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DiminuiValServerRpc()
        {
            QuantidadePlayerRecompensa.Value--;
        }

        private void OnEnable()
        {
            //jogadores conectados
            QuantidadePlayerRecompensa.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                if(newValue==0){
                    if(distribuicaoProjeto==true){
                         if (NetworkManager.Singleton.IsServer){
                            CoreLoopStateHandler.Instance.NextStateServerRpc();
                         }
                            distribuicaoProjeto=false;
                        }
                    }
                    
            };

        }
       
        public void passarState(){
            
            if(distribuicaoProjeto==true){
                 DiminuiValServerRpc();
            }else{
                if(rc.chamarDistribuicao==false){
                        if (rc.comTrocaTrue==false)
                        {
                            CoreLoopStateHandler.Instance.NextStateServerRpc();
                        }
                    }
                    
                    
                
                }
           
         }
         public void MudacomTroca(){
            rc.comTrocaTrue=false;
         }
    }
}
