

using System;
using System.Collections;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class MigraEleitorAvancoState : State
    {
        private const int minEleitores = 1;
        private const int maxEleitores = 3;
        private AvancoState avancoState;
        public event Action<int,Bairro> migraEleitores;
        public int MaxQntdEleitoresMigrar =>   
            Math.Clamp(
                (avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.contaEleitores-1), 
                minEleitores, 
                maxEleitores
            );

        public bool PodeMigrar => 
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.contaEleitores == 0;
        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
            migraEleitores += (eleitores, bairro) => Tools.Logger.Instance.LogWarning(eleitores+" eleitores para "+bairro.Nome);

        }




        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("pode migrar: "+PodeMigrar);

            if(!PodeMigrar) {
                
                avancoState.NextAvancoStateServerRpc();
                return;
            }
            Tools.Logger.Instance.LogInfo("Aperte 1 ou 2 ou 3 para escolher.");
            Debug.Log("max eleitores migrar: "+ MaxQntdEleitoresMigrar);
            Debug.Log("eleitores: "+avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.contaEleitores);
            StartCoroutine(InputReceiver());

        }

        public override void ExitState() 
        { 
            StopAllCoroutines();
        }


        //Por equanto, ainda é automatico. 
        //TODO: definir como sera a feito
        private void MigrarEleitores(int eleitores) {
            int eleitoresMigrar = MaxQntdEleitoresMigrar;
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(eleitores);
            avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(-eleitores);
        
        }

        private void MudaVizinhoControl() {
            int playerID = avancoState.AvancoData.BairroPlayer.PlayerIDNoControl.Value;
            avancoState.AvancoData.BairroVizinho.SetPlayerControlServerRpc(playerID);
        }

        public IEnumerator Migrar(int eleitores) 
        {
            MudaVizinhoControl();
            MigrarEleitores(eleitores);
            migraEleitores?.Invoke(eleitores, avancoState.AvancoData.BairroVizinho);
            yield return new WaitForSeconds(0.1f);
            avancoState.NextAvancoStateServerRpc();

        }


        //Provisório: só para testes
        private IEnumerator InputReceiver()
        {
            while(true) {
                if(MaxQntdEleitoresMigrar >= 1 && Input.GetKeyDown(KeyCode.Alpha1))  StartCoroutine(Migrar(1));
                if(MaxQntdEleitoresMigrar >= 2 && Input.GetKeyDown(KeyCode.Alpha2))  StartCoroutine(Migrar(2));
                if(MaxQntdEleitoresMigrar >= 3 && Input.GetKeyDown(KeyCode.Alpha3))  StartCoroutine(Migrar(3));
                yield return null;
            }

        }



    }
}
