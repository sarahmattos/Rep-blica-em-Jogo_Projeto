using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
 using Game.Territorio;

public class Saúde : NetworkBehaviour
{
    private NetworkVariable<int> quantidadeSaude = new NetworkVariable<int>(0);
    private TMP_Text text_saude;
    private RecursosCartaManager rc;
    public bool playerControlRecurso =false;
    private Bairro bairro;
    private Recursos recurso;
    public GameObject[] saudeIcone;

    [ServerRpc(RequireOwnership = false)]
        public void AtualizarValorUIServerRpc()
        {
            quantidadeSaude.Value++;
        }

    private void Awake()
        {
            text_saude = GetComponentInChildren<TMP_Text>();
            rc = FindObjectOfType<RecursosCartaManager>();
            bairro = GetComponentInParent<Bairro>();
            recurso = GetComponentInParent<Recursos>();
            //saudeIcone[] = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
            
            
            
            
        }
        private void Start()
        {
            saudeIcone = new GameObject[2];
            for (int i=0;i<2;i++){
                saudeIcone[i]=this.transform.GetChild(i).gameObject;
            }
             for (int i=0;i<saudeIcone.Length;i++){
               saudeIcone[i].SetActive(false);
            }
        }

     
     private void OnMouseDown()
    {
        //adicionarSaude();
    }
    public void adicionarSaude(){
         if(bairro.VerificaControl()){
            if(rc.novosSaude>0){
                rc.novosSaude--;
                AtualizarValorUIServerRpc();
            }
        }
    }
    private void OnEnable()
        {
            quantidadeSaude.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                recurso.saude =newValue;
                text_saude.SetText(newValue.ToString());
                if(newValue>0){
                    saudeIcone = new GameObject[2];
                    for (int i=0;i<2;i++){
                        saudeIcone[i]=this.transform.GetChild(i).gameObject;
                    }
                    for (int i=0;i<saudeIcone.Length;i++){
                     saudeIcone[i].SetActive(true);
                 }
                }
            };
        }
}