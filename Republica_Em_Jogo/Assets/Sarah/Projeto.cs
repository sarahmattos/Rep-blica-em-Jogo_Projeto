using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class Projeto : NetworkBehaviour
{
    private NetworkVariable<FixedString4096Bytes> projetoNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
    private NetworkVariable<FixedString4096Bytes> zonaNetworkName = new NetworkVariable<FixedString4096Bytes>();
    private NetworkVariable<int> recompensaNetworkNum = new NetworkVariable<int>(-1);
    private NetworkVariable<int> idPlayer = new NetworkVariable<int>(-1);
    private NetworkVariable<int> votacaoResposta = new NetworkVariable<int>(-1);
    public ProjetoObject projetoManager;
    [SerializeField] private TMP_Text text_projetoCarta;
    [SerializeField] private TMP_Text text_avisoProjeto;
    [SerializeField] private TMP_Text text_avisoOutros;
    public GameObject projetoUI;
    public GameObject restoUI;
    public GameObject bntsUi, btns2, fecharBtn;
    public GameObject verProjetoBtn;
    public string proposta;
    public int numRecompensa;
    public string recompensaText, zonaNameLocal;
    public int clienteLocal= -1;
    public int sim , nao;
   
    //Client cashing
    private string clientDados;
     private string textoTotal="";


    [ServerRpc(RequireOwnership = false)]
    public void DefaultValuesServerRpc()
     {
        recompensaNetworkNum.Value=-1;
        votacaoResposta.Value=-1;
        projetoNetworkTexto.Value="";
        zonaNetworkName.Value="";
    }

   [ServerRpc(RequireOwnership = false)]
    public void UpdateVotacaoServerRpc(int valor)
     {
        votacaoResposta.Value =valor;
        
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateZonaServerRpc(string clientDados)
     {
        zonaNetworkName.Value =clientDados;
        Debug.Log(zonaNetworkName.Value);
        
    }
//
    [ServerRpc(RequireOwnership = false)]
    public void UpdateClientPositionServerRpc(string clientDados, int clientId, int num)
     {
     projetoNetworkTexto.Value = clientDados;
     recompensaNetworkNum.Value=num;
     idPlayer.Value= clientId;
    
    }
    

    public void sortearProjeto(){
        //textoprojeto
        //se for seu turno
        defaultValues();
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
        textoTotal= proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString();
        
        atualizarProjeto(textoTotal);
        
    }
    public void atualizarProjeto(string textoTotal2){
        if(NetworkManager.Singleton.IsClient){  
            int id =(int)NetworkManager.Singleton.LocalClientId;
            UpdateClientPositionServerRpc(textoTotal2, id, numRecompensa);
            Debug.Log("cliente");
        }
        if (NetworkManager.Singleton.IsServer){
            projetoNetworkTexto.Value = textoTotal2;
            idPlayer.Value= (int)NetworkManager.Singleton.LocalClientId;
            recompensaNetworkNum.Value = numRecompensa;
             Debug.Log("server");
            
        }
    }
     private void OnEnable()
    {

        idPlayer.OnValueChanged += (int  previousValue, int  newValue) =>
        {
            clienteLocal=newValue;
            projetoUI.SetActive(true);
            verProjetoBtn.SetActive(false);
            fecharBtn.SetActive(false);
            bntsUi.SetActive(true);
            text_avisoProjeto.text="Escolha uma zona:";
            if(newValue!=(int)NetworkManager.Singleton.LocalClientId){
                    bntsUi.SetActive(false);
                    text_avisoProjeto.text="Aguardando zona ser escolhida";
             }

            
            
        };
        recompensaNetworkNum.OnValueChanged += (int  previousValue, int  newValue) =>
        {
           numRecompensa= newValue;
        };
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            text_projetoCarta.text =  newValue.ToString();
        };
        zonaNetworkName.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            bntsUi.SetActive(false);
            if(clienteLocal==(int)NetworkManager.Singleton.LocalClientId){
            text_avisoProjeto.text = "\n"+"\n"+"\n"+"Zona escolhida: "+newValue.ToString()+"\n"+"Aguardando votação... ";
            }else{
                text_avisoProjeto.text = "\n"+"\n"+"Zona escolhida: "+newValue.ToString()+"\n"+"Vote: ";
                btns2.SetActive(true);
            }
            
        };
        votacaoResposta.OnValueChanged += (int  previousValue, int  newValue) =>
        {
            btns2.SetActive(false);
            Debug.Log(newValue);
            fecharBtn.SetActive(true);
            if(newValue==0){
                text_avisoProjeto.text="\n"+"\n"+"\n"+"PROJETO APROVADO"+"\n"+"Recompensa: "+numRecompensa+ " carta(s) e "+numRecompensa+" eleitor(es)";
            }
            if(newValue==1){
               text_avisoProjeto.text="\n"+"\n"+"\n"+"PROJETO NÃO APROVADO";
            }

            
        };
        
        

    }
    
    public void fechar(){
        fecharBtn.SetActive(false);
        projetoUI.SetActive(false);
        
        
    }
    public void defaultValues(){
        if (NetworkManager.Singleton.IsServer){
           recompensaNetworkNum.Value=-1;
           votacaoResposta.Value=-1;
           projetoNetworkTexto.Value="";
           zonaNetworkName.Value="";

        }
        if(NetworkManager.Singleton.IsClient){ 
            DefaultValuesServerRpc();
        }
        
    }

    public void escolherZona(string zonaName){
        if (NetworkManager.Singleton.IsServer){
           
            zonaNetworkName.Value =zonaName;
            Debug.Log(zonaNetworkName.Value);
        }
        if(NetworkManager.Singleton.IsClient){ 
            UpdateZonaServerRpc(zonaName);
        }
        
    }
    public void votacao(int resposta){

            Debug.Log("resposta"+resposta);
        
             if (NetworkManager.Singleton.IsServer){
                votacaoResposta.Value=resposta;
                 Debug.Log("votacao"+votacaoResposta.Value);
              }
              if(NetworkManager.Singleton.IsClient){
                UpdateVotacaoServerRpc(resposta);
                
              }
            
     
        
    }
   
}
