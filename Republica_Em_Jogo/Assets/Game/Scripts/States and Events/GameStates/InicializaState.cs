using Game.Territorio;
using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Logger = Game.Tools.Logger;


namespace Game
{

    public class InicializaState : State
    {
        [SerializeField] private float intervaloTempo = 0.5f;
        private ZonaTerritorial[] zonasTerritoriais;

        //OBS: embora não pareca ser necessario, sem SerializeField não funciona, por algum motivo.
        [SerializeField] private List<Bairro> todosBairros;

        public override void EnterState()
        {
            Logger.Instance.LogInfo("Enter state: Inicializa");
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
            todosBairros = GetBairros();
            if (!IsServer) return;
            StartCoroutine(DistribuirBairros());
        }

        public override void ExitState()
        {
            StopAllCoroutines();
            Logger.Instance.LogInfo("Exit state: Inicializa");

        }

        private List<Bairro> GetBairros()
        {
            List<Bairro> bairros = new List<Bairro>();
            for (int i = 0; i < zonasTerritoriais.Length; i++)
            {
                bairros.AddAll(zonasTerritoriais[i].Bairros);
            }

            bairros.Shuffle();
            return bairros;

        }

        private IEnumerator DistribuirBairros()
        {
            if (!IsServer) yield return null;

            int clients = NetworkManager.Singleton.ConnectedClientsIds.Count;
            int aux = 0;


            Logger.Instance.LogInfo("distribuição COMECOU.");
            foreach (Bairro bairro in todosBairros)
            {
                bairro.SetPlayerControl(aux);
                aux = (1 + aux) % (clients);

                yield return new WaitForSeconds(intervaloTempo);
            }

            Logger.Instance.LogInfo("distribuição TERMINOU.");
            stateHandler.NextStateServerRPC();



        }


    }

}