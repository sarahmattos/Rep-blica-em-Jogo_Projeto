using System;
using System.Collections.Generic;
using Game.Territorio;
using Unity.Netcode;

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
        
        private NetworkList<int> dadosPlayerAtual = new NetworkList<int>(new List<int>(),
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Server);
        private NetworkList<int> dadosVizinhos = new NetworkList<int>(new List<int>(),
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Server);        
        public NetworkList<int> DadosPlayerAtual => dadosPlayerAtual;
        public NetworkList<int> DadosVizinhos => dadosVizinhos;
        private int contagemRodada;
        public int ContagemRodada { get => contagemRodada; set => contagemRodada = value; }
        private int bairrosAdquiridos;
        public int BairrosAdquiridos { get => bairrosAdquiridos; set => bairrosAdquiridos = value; }

        public void ResetData() {
            ContagemRodada = 0;
            BairrosAdquiridos = 0;
            ClearRodadaDataServerRpc();
        }
        [ServerRpc(RequireOwnership=false)]
        public void ClearRodadaDataServerRpc()
        {
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

        [ServerRpc(RequireOwnership=false)]
        public void SetDadosServerRpc(List<int> dadosPlayerAtual, List<int> dadosVizinhos) {
            for(int i =0;i<dadosPlayerAtual.Count;i++){
                this.dadosPlayerAtual.Add(dadosPlayerAtual[i]);
            }
            for(int i =0;i<dadosVizinhos.Count;i++){
                this.dadosVizinhos.Add(dadosVizinhos[i]);
            }
            
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
