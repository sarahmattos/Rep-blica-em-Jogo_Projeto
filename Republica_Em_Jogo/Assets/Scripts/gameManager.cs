using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Territorio;

public class gameManager : MonoBehaviour
{
    //public Jogador[] jogadores;
    ////public List<Bairro> cartaTerritorio = new List<Bairro>();
    ////int rand, aux;
    ////public int numJogadores;

    //void Start()
    //{
    //    //aux = 0;
    //}

    //void Update()
    //{
        
    //}
    ////public void divisãoInicial()
    //{

    //    while (cartaTerritorio.Count>0)
    //    {
    //        //aux serve para limitar a quantidade de carta por pessoa em jogo
    //        aux++;
    //        //rand vai randomizar os territorios da nossa lista, removendo eles quando foram distribuidos
    //        rand = Random.Range(0, cartaTerritorio.Count);
    //        //nome do territorio sorteado
    //        Debug.Log("rand: " +  cartaTerritorio[rand].nome);
    //        //o id do eleitor recebe o auxiliar que representa o jogador
    //        cartaTerritorio[rand].eleitor.id = aux;
    //        Debug.Log("id: " + cartaTerritorio[rand].eleitor.id);
    //        //a cor do eleitor fica igual a cor do jogador respectivamente
    //        cartaTerritorio[rand].eleitor.cor = jogadores[aux - 1].cor;
    //        cartaTerritorio[rand].eleitor.mudarCor();
    //        //quando o numero chegar na quantidade de jogadores volta a distribuir para o primeiro e assim por diante
    //        if (aux == numJogadores)
    //        {
    //            aux = 0;
    //        }
    //        //remove o território já sorteado da lista de territorios
    //        cartaTerritorio.RemoveAt(rand);

    //    }
    //}
}
