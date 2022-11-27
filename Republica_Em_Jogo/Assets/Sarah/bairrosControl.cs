using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bairrosControl : Singleton<bairrosControl>
{
    //public static bairrosControl instance;
    private GameObject[] gameObjects;
    public Color bairroColor;
    //public GameDataconfig gameDataconfig;
    public int jogadoresConectados;
    private int bairroId;
    public int numeroSorteado;
    public List<int> numerosJaSorteados = new List<int>();
    List<int> numeros = new List<int>();
    private int distribuicao = -1;
    private bool sair=false;

    void Start()
    {
        //instance = this;
        gameObjects = GameObject.FindGameObjectsWithTag("Bairro");
        //gameDataconfig = GameObject.FindObjectOfType<GameDataconfig>();
        Debug.Log(gameObjects.Length);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            numeros.Add(i);
           
        }
    }
            
    
    public void GerarNumerosAleatorios()
    {
        int indice = Random.Range(0, numeros.Count);
        numeroSorteado = numeros[indice];
        numerosJaSorteados.Add(numeroSorteado);
        numeros.Remove(numeros[indice]);
    }
    public void updateColorBairro()
    {
        for (int i = 0; i < numerosJaSorteados.Count; i++)
        {
            distribuicao++;
            var bairroRenderer = gameObjects[numerosJaSorteados[i]].GetComponent<Renderer>();
            bairroColor = GameDataconfig.Instance.PlayerColorOrder[distribuicao];
            bairroRenderer.material.SetColor("_Color", bairroColor);
            if (distribuicao == 2)
            {
                distribuicao = -1;
            }

            if(i== numerosJaSorteados.Count)
            {
                break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        while (numeros.Count != 0)
        {
            GerarNumerosAleatorios();
        }
        
        if (numeros.Count == 0 && sair==false)
        {
            updateColorBairro();
            sair = true;
            
        }
    }

}




