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
            Tools.Logger.Instance.LogInfo("Enter State Lancam. Dados");
            StartCoroutine(LancarDados());

            //Provisório: só pra visualizar.
            avancoState.AvancoData.BairroPlayer.Interagivel.MudarHabilitado(true);
            avancoState.AvancoData.BairroVizinho.Interagivel.MudarHabilitado(true);
        }

        public override void ExitState() 
        {
            //Provisório: só pra visualizar.
            avancoState.AvancoData.BairroPlayer.Interagivel.MudarHabilitado(false);
            avancoState.AvancoData.BairroVizinho.Interagivel.MudarHabilitado(false);
         }

        private IEnumerator LancarDados()
        {
            List<int> dadosPlayerAtual = new List<int>();
            List<int> dadosVizinhos = new List<int>();
            dadosPlayerAtual = GerarDados(bairroPlayerAtual);
            dadosVizinhos = GerarDados(bairroVizinho);
            dadosPlayerAtual.Sort();
            dadosPlayerAtual.Reverse();
            dadosVizinhos.Sort();
            dadosVizinhos.Reverse();
            avancoState.AvancoData.SetDados(dadosPlayerAtual, dadosVizinhos);
            CalcularDiscountAvanco();
            yield return new WaitForSeconds(1);
             
            avancoState.NextAvancoStateServerRpc();
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


    }
}
