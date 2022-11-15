using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DadosCustomizacao : Singleton<DadosCustomizacao>
{
    [SerializeField] private List<ParNomeCor> parNomeCor;
    [SerializeField] private Dictionary<string, Color> dicionarioCores = new Dictionary<string, Color>();
    private void Awake()
    {
        GerarDicionarioCores();
    }

    private void GerarDicionarioCores()
    {
        foreach (ParNomeCor par in parNomeCor)
        {
            dicionarioCores.Add(par.Nome, par.Cor);
        }
    }

}

[Serializable]
public class ParNomeCor
{
    [SerializeField] private string nome;
    [SerializeField] private Color cor;

    public string Nome { get => nome; set => nome = value; }
    public Color Cor { get => cor; set => cor = value; }
}



