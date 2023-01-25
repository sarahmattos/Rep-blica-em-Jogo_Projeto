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
        void Start()
        {
            instance = this;
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
                    CoreLoopStateHandler.Instance.NextStateServerRpc();
                    distribuicaoProjeto=false;
                }
            };

        }
        void Update()
        {
        
        }
        public void passarState(){
            
            if(distribuicaoProjeto==true){
                 DiminuiValServerRpc();
            }else{
                CoreLoopStateHandler.Instance.NextStateServerRpc();
            }
           
         }
    }
}
