using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class ProcessaAvancoState : State
    {
        private AvancoState avancoState;
        private Bairro bairroEscolhido => avancoState.AvancoData.BairroEscolhido;
        private Bairro vizinhoEscolhido => avancoState.AvancoData.BairroEscolhido;

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void EnterState()
        {
            //Provisório: só pra visualizar.
            avancoState.AvancoData.BairroEscolhido.Interagivel.MudarHabilitado(true);
            avancoState.AvancoData.VizinhoEscolhido.Interagivel.MudarHabilitado(true);
            
            
            LancarDados();
        }

        public override void ExitState() { }

        private void LancarDados()
        {
            Eleitores eleitoresParaAvanco = bairroEscolhido.SetUpBairro.Eleitores;
            Eleitores eleitoresVizinho = vizinhoEscolhido.SetUpBairro.Eleitores;
            Tools.Logger.Instance.LogInfo("Meu bairro: "+QntdDados(eleitoresParaAvanco));
            Tools.Logger.Instance.LogInfo("vizinho: "+QntdDados(eleitoresVizinho));

        }

        private int QntdDados(Eleitores eleitores)
        {
            return (eleitores.contaEleitores > 3) ? 3 : eleitores.contaEleitores - 1;
        }
    }
}
