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
    public ProjetoObject projetoManager;
    [SerializeField] private TMP_Text text_projetoCarta;
    public string proposta;
    public int numRecompensa;
    public string recompensaText;
   
    //Client cashing
    private string clientDados;
     private string textoTotal="ola";

    private void UpdateServer() {
        projetoNetworkTexto.Value = textoTotal;
    
    }

    private void UpdateClient() {
    if (clientDados != textoTotal )
         {
        clientDados = textoTotal;
        UpdateClientPositionServerRpc(clientDados); 
        } 
        
    }

[ServerRpc(RequireOwnership = false)]
    public void UpdateClientPositionServerRpc(string clientDados)
     {
    textoTotal = clientDados; 
    }




void Update()
    {
        if (NetworkManager.Singleton.IsServer) {
        UpdateServer(); }
        if (NetworkManager.Singleton.IsClient) {
        UpdateClient(); } 
        
    }

    public void sortearProjeto(){
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
        textoTotal= proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString();
        
    }
     private void OnEnable()
    {
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            Debug.Log("newValue.ToString()");
            text_projetoCarta.text =  newValue.ToString();
        };
    }
   
}
