using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Educaçao : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_Text text_edu;
    private RecursosCartaManager rc;
    int valor;
    private void Awake()
        {
            text_edu = GetComponentInChildren<TMP_Text>();
            rc = FindObjectOfType<RecursosCartaManager>();

        }

     private void OnMouseDown()
    {
        Debug.Log("clicado");
        if(rc.novosEdu>0){
            Debug.Log("clicado2");
            rc.novosEdu--;
            valor++;
            text_edu.SetText(valor.ToString());
        }
        
    }
}
