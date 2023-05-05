using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;

namespace Game.Tools
{
    public static class BairroExtensions
    {
        public static bool TemVizinhoPlayerIDIgual(this Bairro bairro)
        {
            foreach (Bairro vizinho in bairro.Vizinhos)
            {
                if (vizinho.PlayerIDNoControl.Value == bairro.PlayerIDNoControl.Value)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EleitoresMenorQue1(this Bairro bairro)
        {
            if (bairro.SetUpBairro.Eleitores.contaEleitores > 1) return true;
            return false;
        }

    }
}
