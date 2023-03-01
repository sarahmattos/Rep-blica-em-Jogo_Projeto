using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class LancamentoDadosAvancoState : State
    {
        private AvancoState avancoState;
        private Bairro bairroPlayerAtual => avancoState.AvancoData.BairroPlayer;
        private Bairro bairroVizinho => avancoState.AvancoData.BairroVizinho;

        private int randomDiceValue => UnityEngine.Random.Range(1, 7);

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("ENTER Process dados");

            LancarDados();
            ProcessarDescontagemEleitores();           
            avancoState.NextAvancoStateServerRpc();

        }

        public override void ExitState() 
        {
            Tools.Logger.Instance.LogInfo("EXIT Process dados");

        }

        private void LancarDados()
        {
            List<int> dadosPlayerAtual = new List<int>();
            List<int> dadosVizinhos = new List<int>();
            dadosPlayerAtual = GerarDados(bairroPlayerAtual);
            dadosVizinhos = GerarDados(bairroVizinho);
            dadosPlayerAtual.Sort();
            dadosPlayerAtual.Reverse();
            dadosVizinhos.Sort();
            dadosVizinhos.Reverse();
            avancoState.AvancoData.SetDadosServerRpc(dadosPlayerAtual, dadosVizinhos);
        }

        private void ProcessarDescontagemEleitores() {
            CalcularDiscountAvanco();
            AplicaDiscountPlayer();
            AplicaDiscountVizinho();
        }

        private List<int> GerarDados(Bairro bairro) {
            Eleitores eleitoresParaAvanco = bairro.SetUpBairro.Eleitores;
            List<int> dados = new List<int>();
            for (int i = 0; i < QntdDados(eleitoresParaAvanco); i++)
            {
                dados.Add(randomDiceValue);
            }
            return dados;
        }

        private void CalcularDiscountAvanco()
        {
            for (int i = 0; i < avancoState.AvancoData.QntdMenorDados(); i++)
            {
                SetDiscountEleitorValues(i);
            }
        }

        private void SetDiscountEleitorValues(int diceIndex)
        {
            if (ValorDadoPlayerMaior(diceIndex))
            {
                avancoState.AvancoData.VizinhoEleitorDiscount();
            }
            else
            {
                avancoState.AvancoData.PlayerEleitorDiscount();
            }
        }

        private bool ValorDadoPlayerMaior(int dado)
        {
            AvancoData avancoData = avancoState.AvancoData;
            if (avancoData.DadosPlayerAtual[dado] > avancoData.DadosVizinhos[dado])
                return true;
            else
                return false;
        }

        private int QntdDados(Eleitores eleitores)
        {
            return Mathf.Clamp(
                (eleitores.contaEleitores > 3) ? 3 : eleitores.contaEleitores - 1,
                1,
                512
            );
        }

        private void AplicaDiscountPlayer()
        {
            int eleitorDiscountPlayer = avancoState.AvancoData.EleitorDiscountPlayer;
            avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(
                eleitorDiscountPlayer
            );
        }

        private void AplicaDiscountVizinho() {
            int eleitorDiscountVizinho = avancoState.AvancoData.EleitorDiscountVizinho;
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(
                eleitorDiscountVizinho
            );
        }



    }
}
