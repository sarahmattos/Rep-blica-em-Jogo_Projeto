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
    private NetworkVariable<int> idPlayer = new NetworkVariable<int>(-1);
    private NetworkVariable<bool> boolNetwork = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> boolNetwork2 = new NetworkVariable<bool>(false);
    public ProjetoObject projetoManager;
    [SerializeField] private TMP_Text text_projetoCarta;
    [SerializeField] private TMP_Text text_avisoProjeto;
    [SerializeField] private TMP_Text text_avisoOutros;
    public GameObject projetoUI;
    public GameObject restoUI;
    public GameObject bntsUi;
    public GameObject verProjetoBtn;
    public string proposta;
    public int numRecompensa;
    public string recompensaText, zonaNameLocal;
    public bool thisClient=false;
    public int clienteLocal= -1;
   
    //Client cashing
    private string clientDados;
     private string textoTotal="";


   
    [ServerRpc(RequireOwnership = false)]
    public void UpdateZonaServerRpc(string clientDados)
     {
        zonaNetworkName.Value =clientDados;
        Debug.Log(zonaNetworkName.Value);
        
    }
//
    [ServerRpc(RequireOwnership = false)]
    public void UpdateClientPositionServerRpc(string clientDados, int clientId)
     {
     projetoNetworkTexto.Value = clientDados;
     idPlayer.Value= clientId;
    
    }
    [ServerRpc(RequireOwnership = false)]
    public void true2UIServerRpc()
     {
        boolNetwork2.Value= true;
    }
    [ServerRpc(RequireOwnership = false)]
    public void false2UIServerRpc()
     {
        boolNetwork2.Value= false;
    }

    public void sortearProjeto(){
        //textoprojeto
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
        textoTotal= proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString();
        
        atualizarProjeto(textoTotal);
        
    }
    public void atualizarProjeto(string textoTotal2){
        if(NetworkManager.Singleton.IsClient){  
            int id =(int)NetworkManager.Singleton.LocalClientId;
            UpdateClientPositionServerRpc(textoTotal2, id);
            Debug.Log("cliente");
        }
        if (NetworkManager.Singleton.IsServer){
            projetoNetworkTexto.Value = textoTotal2;
            idPlayer.Value= (int)NetworkManager.Singleton.LocalClientId;
             Debug.Log("server");
            
        }
    }
     private void OnEnable()
    {

        idPlayer.OnValueChanged += (int  previousValue, int  newValue) =>
        {
            projetoUI.SetActive(true);
            verProjetoBtn.SetActive(false);
            if(newValue!=(int)NetworkManager.Singleton.LocalClientId){
                    bntsUi.SetActive(false);
                    text_avisoProjeto.text="Aguardando zona ser escolhida";
             }

            
            
        };
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            text_projetoCarta.text =  newValue.ToString();
        };
        zonaNetworkName.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            //text_avisoProjeto.transform.position = new Vector3(3.5f, -90f, transform.position.z);
            text_avisoProjeto.text = "\n"+"\n"+"\n"+"Zona escolhida: "+newValue.ToString()+"\n"+"Aguardando votação... ";
        };
        
        boolNetwork2.OnValueChanged += (bool  previousValue, bool  newValue) =>
        {
             if(newValue==true){
                bntsUi.SetActive(false);
             }
            
        };
        

        
        

    }
    private void Update(){
        /*if(NetworkManager.Singleton.IsClient){
        //Debug.Log(projetoNetworkTexto.Value);
        //Debug.Log(boolNetwork.Value);
        }
        if(boolNetwork.Value==true){

            projetoUI.SetActive(true);
            Debug.Log("ativou");
            verProjetoBtn.SetActive(false);

             if (NetworkManager.Singleton.IsServer){
                
                Debug.Log("server");
                boolNetwork.Value=false;
             }
              if(NetworkManager.Singleton.IsClient){ 
                Debug.Log("boolNetwork.Value");
                falseUIServerRpc();
              }
        }
   
        if(boolNetwork2.Value==true){

            bntsUi.SetActive(false);
            

             if (NetworkManager.Singleton.IsServer){
                boolNetwork2.Value=false;
             }
              if(NetworkManager.Singleton.IsClient){ 
                false2UIServerRpc();
              }
        }
         */

    }
    public void escolherZona(string zonaName){
        if (NetworkManager.Singleton.IsServer){
           
            zonaNetworkName.Value =zonaName;
            boolNetwork2.Value=true;
            Debug.Log(zonaNetworkName.Value);
        }
        if(NetworkManager.Singleton.IsClient){ 
            true2UIServerRpc();
            UpdateZonaServerRpc(zonaName);
        }
        
    }
   
}
