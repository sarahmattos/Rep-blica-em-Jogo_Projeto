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
    private NetworkVariable<FixedString4096Bytes > projetoNetworkTexto = new NetworkVariable<FixedString4096Bytes >("ola", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public ProjetoObject projetoManager;
    [SerializeField] private TMP_Text text_projetoCarta;
    public string proposta;
    public int numRecompensa;
    public string recompensaText;
   
    public void sortearProjeto(){
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
         Debug.Log("projeto: "+projetoNetworkTexto.Value);
        if (IsOwner) {
        projetoNetworkTexto.Value=(proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString());
        Debug.Log("projeto: "+projetoNetworkTexto.Value);
        }
    }
     private void OnEnable()
    {
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            Debug.Log("newValue.ToString()");
            text_projetoCarta.text =  newValue.ToString();
        };
    }
    private void onProjetoMuda(int previousValue, int newValue)
        {
           text_projetoCarta.text =  newValue.ToString();
        }
        private void Update()
    {
        
        //chatInput = GameObject.FindGameObjectsWithTag("inputField");
            if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
           proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
         Debug.Log("projeto: "+projetoNetworkTexto.Value);
        //if (IsOwner) {
        projetoNetworkTexto.Value=(proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString());
        Debug.Log("projeto: "+projetoNetworkTexto.Value);
        //}
        }
    }
}
