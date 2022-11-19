using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Territorio
{
    [Serializable]
    public class DadoBairro
    {
        [SerializeField] private string nome;
        [SerializeField] private DadoBairro[] bairrosVizinhos;
        //[SerializeField] private Eleitores[] eleitores;
        //[SerializeField] private int recursos;

        public string Nome { get => nome; }
        public DadoBairro[] BairrosVizinhos { get => bairrosVizinhos; }
        //public Eleitores[] Eleitores { get => eleitores; }
    }

}
