using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Projeto : MonoBehaviour
{
    public ProjetoObject projetoManager;
    [SerializeField] private TMP_Text text_projetoCarta;
    public string proposta;
    public int numRecompensa;
    public string recompensaText;
    void Start()
    {
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText += projetoManager.recompensaText;
        text_projetoCarta.text = (proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
