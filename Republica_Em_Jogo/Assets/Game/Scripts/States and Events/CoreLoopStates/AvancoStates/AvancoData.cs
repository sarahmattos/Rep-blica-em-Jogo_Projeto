using System;
using System.Collections.Generic;
using Game.Territorio;

namespace Game
{
    [Serializable]
    public class AvancoData
    {
        private Bairro bairroEscolhido;
        private Bairro vizinhoEscolhido;
        public Bairro BairroEscolhido
        {
            get => bairroEscolhido;
            set => bairroEscolhido = value;
        }
        public Bairro VizinhoEscolhido
        {
            get => vizinhoEscolhido;
            set => vizinhoEscolhido = value;
        }
        private int bairroEleitorDiscount;
        private int vizinhoEleitorDiscount;
        // dadosLancados?.Invoke();
        public event Action dadosLancados;

        private List<int> dadosPlayerAtual = new List<int>();
        private List<int> dadosVizinhos = new List<int>();        
        public List<int> DadosPlayerAtual => dadosPlayerAtual;
        public List<int> DadosVizinhos => dadosVizinhos;

        public void ClearData()
        {
            bairroEscolhido = null;
            vizinhoEscolhido = null;
            bairroEleitorDiscount = 0;
            vizinhoEleitorDiscount = 0;
            dadosPlayerAtual.Clear();
            dadosVizinhos.Clear();
        }

        public void BairroEleitorDiscount() {
            bairroEleitorDiscount--;
        }

        public void VizinhoEleitorDiscount() {
            vizinhoEleitorDiscount--;
        }

        public void SetDados(List<int> dadosPlayerAtual, List<int> dadosVizinhos) {
            this.dadosPlayerAtual = dadosPlayerAtual;
            this.dadosVizinhos = dadosVizinhos;
            dadosLancados?.Invoke();
        }

        public int QntdMenorDados()
        {
            return (DadosPlayerAtual.Count < DadosVizinhos.Count)
                ? DadosPlayerAtual.Count
                : DadosVizinhos.Count;
        }

    }
}
