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
        DadosUiGeral dadosUiGeral;
        public AvancoData AvancoData => avancoState.AvancoData;

        private void Start()
        {
            AvancoData.dadosLancados += UpdateTextDados;
            dadosUiGeral=FindObjectOfType<DadosUiGeral>();
        }
        private void OnDestroy()
        {
            AvancoData.dadosLancados -= UpdateTextDados;

        }

        private void UpdateTextDados() {
            string dadosPlayerAtual = "Ataque: \n";
            string dadosVizinhos = "Defesa: \n";
            foreach(int value in AvancoData.DadosPlayerAtual)
                dadosPlayerAtual = string.Concat(dadosPlayerAtual, value, "\n");
            foreach(int value in AvancoData.DadosVizinhos)
                dadosVizinhos = string.Concat(dadosVizinhos, value, "\n");
            
            //text_dadosPlayerAtual.SetText(dadosPlayerAtual);
            //text_dadosVizinhos.SetText(dadosVizinhos);  
            dadosUiGeral.reseta2UiDadosServerRpc();
            dadosUiGeral.atualizaUiDadosServerRpc(dadosPlayerAtual,dadosVizinhos);

        }
        public void UpdateTextDados2(string _p,string _v){
            text_dadosPlayerAtual.SetText(_p);
            text_dadosVizinhos.SetText(_v);   
            
        }

    }
}
