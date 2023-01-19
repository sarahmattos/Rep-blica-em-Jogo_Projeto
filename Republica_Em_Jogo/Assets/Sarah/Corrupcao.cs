using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class Corrupcao : NetworkBehaviour
{
    private NetworkVariable<FixedString4096Bytes> corrupcaoNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
    public CorrupcaoObject CorrupcaoManager;
    private string corrupcao;
    private int penalidade;
    private string complementText;
    [SerializeField] private TMP_Text text_CorrupcaoCarta;
    [SerializeField] private GameObject cartaCorrupcao;

    [ServerRpc(RequireOwnership = false)]
        public void AtualizaTextoServerRpc(string textoTotal2)
        {
            corrupcaoNetworkTexto.Value = textoTotal2;
        }

    public void sortearCorrupcao(){
            //defaultValues();
            corrupcao = CorrupcaoManager.corrupcao[Random.Range(0, CorrupcaoManager.corrupcao.Length)];
            complementText= CorrupcaoManager.complementText;
            penalidade =CorrupcaoManager.penalidade;
            string textoTotal= "\n"+corrupcao +"\n"+"\n"+ complementText;
            AtualizaTextoServerRpc(textoTotal);

            
        }
        private void OnEnable()
        {
            corrupcaoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
                {
                    if(newValue!=""){
                        cartaCorrupcao.SetActive(true);
                        text_CorrupcaoCarta.text =  newValue.ToString();
                    }
                };
        }
        public void chamarPenalidade(){

        }

        public void panelFalse(GameObject panel){
             panel.SetActive(false);
         }
}
