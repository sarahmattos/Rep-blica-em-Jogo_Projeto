using System;
using System.Collections.Generic;
using Game.Territorio;

namespace Game
{
    [Serializable]
    public class AvancoData
    {
        private Bairro bairroPlayer;
        private Bairro bairroVizinho;
        public Bairro BairroPlayer
        {
            get => bairroPlayer;
            set => bairroPlayer = value;
        }
        public Bairro BairroVizinho
        {
            get => bairroVizinho;
            set => bairroVizinho = value;
        }
        private int eleitorDiscountPlayer;
        public int EleitorDiscountPlayer => eleitorDiscountPlayer;
        private int eleitorDiscountVizinho;
        public int EleitorDiscountVizinho => eleitorDiscountVizinho;
        public event Action dadosLancados;

        private List<int> dadosPlayerAtual = new List<int>();
        private List<int> dadosVizinhos = new List<int>();        
        public List<int> DadosPlayerAtual => dadosPlayerAtual;
        public List<int> DadosVizinhos => dadosVizinhos;
        private int contagemRodada;
        public int ContagemRodada { get => contagemRodada; set => contagemRodada = value; }
        private int bairrosAdquiridos;
        public int BairrosAdquiridos { get => bairrosAdquiridos; set => bairrosAdquiridos = value; }

        public void ResetData() {
            ContagemRodada = 0;
            BairrosAdquiridos = 0;
            ClearRodadaData();
        }

        public void ClearRodadaData()
        {
            
            BairroPlayer?.Interagivel.ChangeSelectedBairro(false);
            BairroVizinho?.Interagivel.ChangeSelectedBairro(false);
            bairroPlayer = null;
            bairroVizinho = null;
            eleitorDiscountPlayer = 0;
            eleitorDiscountVizinho = 0;
            dadosPlayerAtual.Clear();
            dadosVizinhos.Clear();
        }

        public void PlayerEleitorDiscount() {
            eleitorDiscountPlayer--;
        }

        public void VizinhoEleitorDiscount() {
            eleitorDiscountVizinho--;
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
