using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objetivosDatabase : MonoBehaviour
{
    public string objetivoComplemento;
    public string[] zonasNome;
    int indice;
    void Start()
    {
        zonasNome = new string[7];


        zonasNome[0] = "Sul";
        zonasNome[1] = "Norte1";
        zonasNome[2] = "Norte2";
        zonasNome[3] = "Oeste1";
        zonasNome[4] = "Oeste2";
        zonasNome[5] = "Oeste3";
        zonasNome[6] = "Centro";





        indice = Random.Range(0, zonasNome.Length);
        gerarObjetivo(zonasNome[indice]);

    }
    
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Instantiate!"))
        {
            indice = Random.Range(0, zonasNome.Length);
            gerarObjetivo(zonasNome[indice]);
        }
    }
    
    public void gerarObjetivo(string zonaComplemento)
    {
        objetivoComplemento = "Seu objetivo é conquistar a zona "+ zonaComplemento + " acrescentando nela dois recursos de educação e de saúde";
        Debug.Log(objetivoComplemento);
    }
    
}
