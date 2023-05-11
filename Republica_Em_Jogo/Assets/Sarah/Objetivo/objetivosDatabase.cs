using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Game.Tools;
using Unity.Networking.Transport;

public class objetivosDatabase : Singleton<objetivosDatabase>
{
    public string objetivoComplemento;
    public string objetivoZona;
    public string[] zonasNome;
    int indice;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
    
    public void gerarObjetivo(string zonaComplemento)
    {
        objetivoZona=zonaComplemento;
        objetivoComplemento = 
            "Conquistar a zona:\n"+ 
            objetivoZona + 
            "\n\nAdicionando: \n2 recursos de educação. \n2 recursos de saúde.";
    }
    
}
