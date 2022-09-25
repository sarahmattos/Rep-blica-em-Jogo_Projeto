using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public int id;
    public string nome;
    private string cor;
    public bool turno;
    public enum acao
    {
        projeto,
        atacar,
        carta
    }
    public acao acaoEscolhida;
    void Start()
    {
      
    }

    void Update()
    {
        if (turno)
        {
            switch (acaoEscolhida)
            {
                case acao.projeto:
                    apresentarProjeto();
                    break;
                case acao.atacar:
                    atacarTerritorio();
                    break;
                case acao.carta:
                    puxarCarta();
                    break;
            }
        }
    }
    public void apresentarProjeto()
    {
        //+1 jogador
        //aprovação/reprovacao
    }
    public void atacarTerritorio()
    {
        //territorio origem
        //territorio origem
        //quantidade eleitores

    }
    public void puxarCarta()
    {
        //baralho
        //quantidade
    }
}
