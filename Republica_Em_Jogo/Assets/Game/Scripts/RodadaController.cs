using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class RodadaController : MonoBehaviour
    {
        private int rodada = 0;
        public int maxRodada => GameDataConfig.Instance.MaxRodadasParaEleicoes;
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
            if (rodada % maxRodada == 0)
            {
                rodadaMaxAlcancada?.Invoke();
            }
        }



    }
}
