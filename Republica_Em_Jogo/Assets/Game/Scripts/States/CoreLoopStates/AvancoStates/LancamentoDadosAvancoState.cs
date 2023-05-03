using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using Game.UI;
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
            // UICoreLoop.Instance.UpdateTitleText("Rolando Dados...");
            if (!TurnManager.Instance.LocalIsCurrent) return;
            LancarDados();
            ProcessarDescontagemEleitores();
            avancoState.StateMachineController.NextStateServerRpc();

        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
        }

        private void LancarDados()
        {
            List<int> dadosPlayerAtual = new List<int>();
            List<int> dadosVizinhos = new List<int>();
            dadosPlayerAtual = GerarDados(bairroPlayerAtual, 1);
            dadosVizinhos = GerarDados(bairroVizinho, 0);
            dadosPlayerAtual.Sort();
            dadosPlayerAtual.Reverse();
            dadosVizinhos.Sort();
            dadosVizinhos.Reverse();
            avancoState.AvancoData.SetDados(dadosPlayerAtual, dadosVizinhos);
        }

        private void ProcessarDescontagemEleitores()
        {
            CalcularDiscountAvanco();
            AplicaDiscountPlayer();
            AplicaDiscountVizinho();
        }

        private List<int> GerarDados(Bairro bairro, int diminuiEleitor)
        {
            Eleitores eleitoresParaAvanco = bairro.SetUpBairro.Eleitores;
            List<int> dados = new List<int>();
            for (int i = 0; i < QntdDados(eleitoresParaAvanco, diminuiEleitor); i++)
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

        private int QntdDados(Eleitores eleitores, int subtrair)
        {
            return Mathf.Clamp(
                (eleitores.contaEleitores > 3) ? 3 : eleitores.contaEleitores - subtrair,
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

        private void AplicaDiscountVizinho()
        {
            int eleitorDiscountVizinho = avancoState.AvancoData.EleitorDiscountVizinho;
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(
                eleitorDiscountVizinho
            );
        }



    }
}
