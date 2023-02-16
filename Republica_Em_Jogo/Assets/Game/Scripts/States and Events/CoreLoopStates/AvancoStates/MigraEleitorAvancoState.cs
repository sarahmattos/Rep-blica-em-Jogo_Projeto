

using System;
using Game.Territorio;

namespace Game
{
    public class MigraEleitorAvancoState : State
    {
        private AvancoState avancoState;
        public event Action<int,Bairro> migraEleitores;
        public int MaxQntdEleitoresMigrar =>   
             Math.Clamp(avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.contaEleitores, 
             1, 3);

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
            migraEleitores += (eleitores, bairro) => Tools.Logger.Instance.LogWarning("Até "+eleitores+" eleitores para "+bairro.Nome);

        }




        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: processa dados");
            AplicaDiscountPlayer();
            AplicaDiscountVizinho();

            if(avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.contaEleitores == 0) {
                migraEleitores?.Invoke(MaxQntdEleitoresMigrar, avancoState.AvancoData.BairroVizinho);
                // return;
            }

            
            avancoState.NextAvancoStateServerRpc();
        }

        public override void ExitState() 
        { 

        }


        //TODO: passar para LancamentoDadosAvancoState ?
        private void AplicaDiscountPlayer()
        {
            int eleitorDiscountPlayer = avancoState.AvancoData.EleitorDiscountPlayer;
            avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.MudaValorEleitores(
                eleitorDiscountPlayer
            );
        }

        //TODO: passar para LancamentoDadosAvancoState ?
        private void AplicaDiscountVizinho() {
            int eleitorDiscountVizinho = avancoState.AvancoData.EleitorDiscountVizinho;
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.MudaValorEleitores(
                eleitorDiscountVizinho
            );
        }



    }
}
