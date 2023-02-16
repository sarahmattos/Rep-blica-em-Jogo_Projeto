

using System;
using System.Collections;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class MigraEleitorAvancoState : State
    {
        private const int minEleitoresMigrar = 1;
        private const int maxEleitoresMigrar = 3;
        private AvancoState avancoState;
        public event Action<int,Bairro> migraEleitores;
        public int MaxQntdEleitoresMigrar =>   
            Math.Clamp(
                (avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.contaEleitores-1), 
                minEleitoresMigrar, 
                maxEleitoresMigrar
            );

        public bool PodeMigrar => 
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.contaEleitores == 0;
        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
            migraEleitores += (eleitores, bairro) => Tools.Logger.Instance.LogWarning("Até "+eleitores+" eleitores para "+bairro.Nome);

        }




        public override void EnterState()
        {
            if(PodeMigrar) {
                StartCoroutine(Migrar());
                return;
            }
            avancoState.NextAvancoStateServerRpc();

            
        }

        public override void ExitState() 
        { 
            StopCoroutine(Migrar());

        }


        //Por equanto, ainda é automatico. 
        //TODO: definir como sera a feito
        private void MigrarEleitores() {
            int eleitoresMigrar = MaxQntdEleitoresMigrar;
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.MudaValorEleitores(eleitoresMigrar);
            avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.MudaValorEleitores(-eleitoresMigrar);
        
        }

        private void MudaVizinhoControl() {
            int playerID = avancoState.AvancoData.BairroPlayer.PlayerIDNoControl.Value;
            avancoState.AvancoData.BairroVizinho.SetPlayerControl(playerID);
        }

        public IEnumerator Migrar() 
        {
            MudaVizinhoControl();
            MigrarEleitores();
            migraEleitores?.Invoke(MaxQntdEleitoresMigrar, avancoState.AvancoData.BairroVizinho);
            yield return new WaitForSeconds(1);
            avancoState.NextAvancoStateServerRpc();

        }



    }
}
