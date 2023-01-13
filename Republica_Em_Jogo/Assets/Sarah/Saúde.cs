using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Saúde : MonoBehaviour
{
     private TMP_Text text_saude;
    private RecursosCartaManager rc;
    int valor;
    private void Awake()
        {
            text_saude = GetComponentInChildren<TMP_Text>();
            rc = FindObjectOfType<RecursosCartaManager>();

        }

     private void OnMouseDown()
    {
        Debug.Log("clicado");
        if(rc.novosSaude>0){
            Debug.Log("clicado2");
            rc.novosSaude--;
            valor++;
            text_saude.SetText(valor.ToString());
        }
        
    }
}