using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RodadaController : MonoBehaviour
    {
        private int rodada = 1;
        public int Rodada => rodada;
        public int maxRodada => GameDataconfig.Instance.MaxRodadasParaEleicoes;
        public event Action<int> rodadaMuda;
        public Action rodadaMaxAlcancada;

        public void InscreverEvents()
        {
            TurnManager.Instance.FirstPlayerTurn += AcrescentarRodada;
            rodadaMuda += OnRodadaMuda;
        }

        public void DesinscreverEvents()
        {
            TurnManager.Instance.FirstPlayerTurn -= AcrescentarRodada;
            rodadaMuda -= OnRodadaMuda;
        }


        private void AcrescentarRodada()
        {
            rodada++;
            rodadaMuda?.Invoke(rodada);
        }

        private void OnRodadaMuda(int rodada)
        {
            if (rodada % maxRodada == 1)
            {
                rodadaMaxAlcancada?.Invoke();
            }
        }



    }
}
