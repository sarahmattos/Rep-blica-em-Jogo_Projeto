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
     Debug.Log("Cliente idPlayer "+idPlayer.Value);
     Debug.Log("Cliente NetworkManager.Singleton.LocalClientId "+NetworkManager.Singleton.LocalClientId);
    }
        
    [ServerRpc(RequireOwnership = false)]
    public void falseUIServerRpc()
     {
        boolNetwork.Value= false;
    }
    [ServerRpc(RequireOwnership = false)]
    public void trueUIServerRpc()
     {
        boolNetwork.Value= true;
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
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
        textoTotal= proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString();
        atualizarProjeto(textoTotal);
        
    }
    public void atualizarProjeto(string textoTotal2){
        if(NetworkManager.Singleton.IsClient){  
            Debug.Log(NetworkManager.Singleton.LocalClientId);
            int id =(int)NetworkManager.Singleton.LocalClientId;
            UpdateClientPositionServerRpc(textoTotal2, id);
            trueUIServerRpc();
            Debug.Log("cliente");
        }
        if (NetworkManager.Singleton.IsServer){
            projetoNetworkTexto.Value = textoTotal2;
            boolNetwork.Value= true;
            idPlayer.Value= (int)NetworkManager.Singleton.LocalClientId;
            Debug.Log("server");
            
        }
    }
     private void OnEnable()
    {
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            text_projetoCarta.text =  newValue.ToString();
        };
        zonaNetworkName.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            //text_avisoProjeto.transform.position = new Vector3(3.5f, -90f, transform.position.z);
            text_avisoProjeto.text = "\n"+"\n"+"\n"+"Zona escolhida: "+newValue.ToString()+"\n"+"Aguardando votação... ";
        };
        boolNetwork.OnValueChanged += (bool  previousValue, bool  newValue) =>
        {
             if(newValue==true){
                Debug.Log("mudou o bool");
                projetoUI.SetActive(true);
                verProjetoBtn.SetActive(false);
                if(thisClient!=true){
                    bntsUi.SetActive(false);
                    text_avisoProjeto.text="Aguardando zona ser escolhida";
                }
                //-90.23
             }
            
        };
        boolNetwork2.OnValueChanged += (bool  previousValue, bool  newValue) =>
        {
             if(newValue==true){
                bntsUi.SetActive(false);
             }
            
        };
        idPlayer.OnValueChanged += (int  previousValue, int  newValue) =>
        {
           /* Debug.Log("newValue "+newValue);
            Debug.Log("NetworkManager.Singleton.LocalClientId "+NetworkManager.Singleton.LocalClientId);
             if(newValue==(int)NetworkManager.Singleton.LocalClientId){
                thisClient=true;
                Debug.Log("mudou o this cliente "+thisClient);
             }
            */
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
