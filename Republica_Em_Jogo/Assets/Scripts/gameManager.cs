using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    private Jogador jogador1, jogador2;
    public Territorio[] cartaTerritorios;
    int rand, rand2, rand3, rand4;
    void Start()
    {
        //jogador1.cor = "amarelo";
        //jogador2.cor = "rosa";
        divisãoInicial();
    }

    void Update()
    {
        
    }
    public void divisãoInicial()
    {
         rand = Random.Range(0, 4);
        cartaTerritorios[rand].eleitor.mudarCor(0);
        Debug.Log("rand: " + rand);
         rand2 = Random.Range(0, 4);
        while (rand2 == rand)
        {
             rand2 = Random.Range(0, 4);
        }
        cartaTerritorios[rand2].eleitor.mudarCor(2);
        Debug.Log("rand2: " + rand2);
         rand3 = Random.Range(0, 4);
        while (rand3 == rand|| rand3== rand2)
        {
             rand3 = Random.Range(0, 4);
        }
        cartaTerritorios[rand3].eleitor.mudarCor(0);
        Debug.Log("rand3: " + rand3);
         rand4 = Random.Range(0, 4);
        while (rand4 == rand || rand4 == rand2 || rand4==rand3)
        {
             rand4 = Random.Range(0, 4);
        }
        cartaTerritorios[rand4].eleitor.mudarCor(2);
        Debug.Log("rand4: " + rand4);

        //jogador1.

    }
}
