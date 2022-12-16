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
    public NetworkVariable<FixedString4096Bytes > projetoNetworkTexto = new NetworkVariable<FixedString4096Bytes >("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public ProjetoObject projetoManager;
    [SerializeField] private TMP_Text text_projetoCarta;
    public string proposta;
    public int numRecompensa;
    public string recompensaText;
   
    public void sortearProjeto(){
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText += projetoManager.recompensaText;
        projetoNetworkTexto.Value=(proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString());
        Debug.Log("projeto: "+projetoNetworkTexto.Value);
    }
     private void OnEnable()
    {
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
        {
            text_projetoCarta.text =  newValue.ToString();
        };
    }
    private void onProjetoMuda(int previousValue, int newValue)
        {
           text_projetoCarta.text =  newValue.ToString();
        }
}
