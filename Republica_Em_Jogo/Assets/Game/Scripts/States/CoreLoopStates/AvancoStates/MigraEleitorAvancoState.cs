

using System;
using System.Collections;
using System.Linq;
using Game.Territorio;
using Game.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MigraEleitorAvancoState : State
    {
        // [SerializeField] GameObject avisoPassaEleitor;
        private const int minEleitores = 1;
        private const int maxEleitores = 3;
        private AvancoState avancoState;
        private Bairro BairroPlayerAtual => avancoState.AvancoData.BairroPlayer;
        private Bairro BairroVizinho => avancoState.AvancoData.BairroVizinho;

        public int MaxQntdEleitoresMigrar =>
            Math.Clamp(
                (avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.contaEleitores - 1),
                minEleitores,
                maxEleitores
            );

        private bool RemoveuTodosEleitoresVizinho =>
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.contaEleitores == 0;

        public event Action MigraEleitores;
        public event Action<int, Bairro> MigrouEleitores;

        private void Start()
        {
            avancoState = GetComponentInParent<AvancoState>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void EnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            if (!RemoveuTodosEleitoresVizinho)
            {
                avancoState.StateMachineController.NextStateServerRpc();
                return;
            }
            GameStateEmitter.SendMessage("Bairro conquistado!");
            MigraEleitores?.Invoke();
            SetUpZona.Instance.AllBairros.MudarInativity(true);
            BairroPlayerAtual.Interagivel.MudarInativity(false);
            BairroVizinho.Interagivel.MudarInativity(false);

        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            SetUpZona.Instance.AllBairros.MudarInativity(false);
            StopAllCoroutines();
        }

        public void MigrarEleitores(int value)
        {
            StartCoroutine(ProcessarMigracao(value));
        }

        private IEnumerator ProcessarMigracao(int eleitores)
        {
            MudarVizinhoControl();
            MudarQntdEleitoresBairros(eleitores);
            // avisoPassaEleitor.SetActive(false);
            MigrouEleitores?.Invoke(eleitores, avancoState.AvancoData.BairroVizinho);
            yield return new WaitForSeconds(0.1f);
            avancoState.StateMachineController.NextStateServerRpc();

        }

        private void MudarVizinhoControl()
        {
            int playerID = avancoState.AvancoData.BairroPlayer.PlayerIDNoControl.Value;
            avancoState.AvancoData.BairroVizinho.SetPlayerControlServerRpc(playerID);
        }

        private void MudarQntdEleitoresBairros(int eleitores)
        {
            int eleitoresMigrar = MaxQntdEleitoresMigrar;
            avancoState.AvancoData.BairroVizinho.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(eleitores);
            avancoState.AvancoData.BairroPlayer.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(-eleitores);
            avancoState.AvancoData.BairrosAdquiridos++;

        }






        //Provisório: só para testes
        // private IEnumerator InputReceiver()
        // {
        //     while (true)
        //     {
        //         if (MaxQntdEleitoresMigrar >= 1 && Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(Migrar(1));
        //         if (MaxQntdEleitoresMigrar >= 2 && Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(Migrar(2));
        //         if (MaxQntdEleitoresMigrar >= 3 && Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(Migrar(3));
        //         yield return null;
        //     }

        // }




    }
}
