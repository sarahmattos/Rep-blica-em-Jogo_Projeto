using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace Game
{
    public class ControlePassarState : NetworkBehaviour
    {
        public static ControlePassarState instance;
        public NetworkVariable<int> QuantidadePlayerRecompensa = new NetworkVariable<int>(0);
        public bool distribuicaoProjeto=false;
        private RecursosCartaManager rc;
        void Start()
        {
            instance = this;
            rc = FindObjectOfType<RecursosCartaManager>();
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
                        CoreLoopStateHandler.Instance.NextStateServerRpc();
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
                    CoreLoopStateHandler.Instance.NextStateServerRpc();
                    }
                
                }
           
         }
    }
}
