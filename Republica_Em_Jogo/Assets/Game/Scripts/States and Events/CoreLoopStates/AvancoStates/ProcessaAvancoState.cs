using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;
using System.Linq;
using System;

namespace Game
{
    public class ProcessaAvancoState : State
    {
        private AvancoState avancoState;
        private Bairro bairroPlayerAtual => avancoState.AvancoData.BairroEscolhido;
        private Bairro bairroVizinho => avancoState.AvancoData.VizinhoEscolhido;

        private int randomDiceValue => UnityEngine.Random.Range(1, 7);

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            StartCoroutine(LancarDados());

            //Provisório: só pra visualizar.
            avancoState.AvancoData.BairroEscolhido.Interagivel.MudarHabilitado(true);
            avancoState.AvancoData.VizinhoEscolhido.Interagivel.MudarHabilitado(true);
        }

        public override void ExitState() { }

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
            CalculaAvanco();
            yield return null;
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

        private void CalculaAvanco()
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
                avancoState.AvancoData.BairroEleitorDiscount();
            }
            else
            {
                avancoState.AvancoData.VizinhoEleitorDiscount();
            }
        }

        private bool ValorDadoPlayerMaior(int dado)
        {
            AvancoData avancoData = avancoState.AvancoData;
            if (avancoData.DadosPlayerAtual[dado] < avancoData.DadosVizinhos[dado])
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
