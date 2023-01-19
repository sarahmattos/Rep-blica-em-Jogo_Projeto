using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class MovimentosSociais : NetworkBehaviour
{
    private NetworkVariable<FixedString4096Bytes> movimentoSocialNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
    public MovimentoSociaisObject MovimentoSociaisManager;
    private string movimento;
    private string recursoTipo;
    private int quantidadeRecurso;
    private int quantidadeEleitor;
    [SerializeField] private TMP_Text text_msCarta;
    [SerializeField] private GameObject cartaMV;

    [ServerRpc(RequireOwnership = false)]
        public void AtualizaTextoServerRpc(string textoTotal2)
        {
            movimentoSocialNetworkTexto.Value = textoTotal2;
        }

    public void sortearMS(){
            //defaultValues();
            int aux= Random.Range(0, MovimentoSociaisManager.movimento.Length);
            movimento = MovimentoSociaisManager.movimento[aux];
            recursoTipo = MovimentoSociaisManager.recursoTipo[aux];
            quantidadeRecurso = MovimentoSociaisManager.quantidadeRecurso;
            quantidadeEleitor = MovimentoSociaisManager.quantidadeEleitor;

            string textoTotal= "\n"+movimento +"\n"+"\n"+"Ganhe "+ quantidadeRecurso+" recurso de "+recursoTipo.ToString()+" e "+ quantidadeEleitor+" eleitores";
            AtualizaTextoServerRpc(textoTotal);

            
        }
        private void OnEnable()
        {
            movimentoSocialNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
                {
                    if(newValue!=""){
                        cartaMV.SetActive(true);
                        text_msCarta.text =  newValue.ToString();
                    }
                };
        }
        public void chamarRecompensas(){

        }

        public void panelFalse(GameObject panel){
             panel.SetActive(false);
         }
}
