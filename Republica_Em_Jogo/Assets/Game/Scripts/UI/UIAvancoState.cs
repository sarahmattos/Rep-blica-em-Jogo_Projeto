using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class UIAvancoState : MonoBehaviour
    {
        [SerializeField] private TMP_Text text_dadosVizinhos;
        [SerializeField] private TMP_Text text_dadosPlayerAtual;
        [SerializeField] private AvancoState avancoState;
        public AvancoData AvancoData => avancoState.AvancoData;

        private void Start()
        {
            AvancoData.dadosLancados += UpdateTextDados;
        }
        private void OnDestroy()
        {
            AvancoData.dadosLancados -= UpdateTextDados;

        }

        private void UpdateTextDados() {
            string dadosPlayerAtual = "Seus: \n";
            string dadosVizinhos = "Vizinho: \n";
            foreach(int value in AvancoData.DadosPlayerAtual)
                dadosPlayerAtual = string.Concat(dadosPlayerAtual, value, "\n");
            foreach(int value in AvancoData.DadosVizinhos)
                dadosVizinhos = string.Concat(dadosVizinhos, value, "\n");
            
            text_dadosPlayerAtual.SetText(dadosPlayerAtual);
            text_dadosVizinhos.SetText(dadosVizinhos);   

        }

    }
}
