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
     private string textoTotal="";


[ServerRpc(RequireOwnership = false)]
    public void UpdateClientPositionServerRpc(string clientDados)
     {
     projetoNetworkTexto.Value = clientDados;
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
            UpdateClientPositionServerRpc(textoTotal2);
        }
        if (NetworkManager.Singleton.IsServer){
            projetoNetworkTexto.Value = textoTotal2;
        }
    }
     private void OnEnable()
    {
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            text_projetoCarta.text =  newValue.ToString();
        };
    }
   
}
