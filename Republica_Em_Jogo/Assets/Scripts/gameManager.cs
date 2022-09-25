using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public Jogador[] jogadores;
    //public Territorio[] cartaTerritorios;
    public List<Territorio> cartaTerritorio = new List<Territorio>();
    int rand, aux, max, rand4;

    void Start()
    {
        aux = 0;
        max = 4;
       // divisãoInicial();
    }

    void Update()
    {
        
    }
    public void divisãoInicial()
    {

        while (cartaTerritorio.Count>0)
        {
            aux++;
            rand = Random.Range(0, cartaTerritorio.Count);
            Debug.Log("rand: " +  cartaTerritorio[rand].nome);
            cartaTerritorio[rand].eleitor.id = aux;
            Debug.Log("id: " + cartaTerritorio[rand].eleitor.id);


            //Debug.Log("cor: " + cartaTerritorio[rand].eleitor.cor);
            //cartaTerritorios[rand].eleitor.mudarCor(aux-1);
            
            //Debug.Log("aux: " + aux);
            Debug.Log(jogadores[aux - 1].cor);
            cartaTerritorio[rand].eleitor.cor = jogadores[aux - 1].cor;
            
            Debug.Log(cartaTerritorio[rand].eleitor.cor);
            if (aux == max)
            {
                aux = 0;
            }
            cartaTerritorio.RemoveAt(rand);
            //cartaTerritorio[rand].eleitor.cores = jogadores[cartaTerritorio[rand].eleitor.id].cor;


        }
        

    }
}
