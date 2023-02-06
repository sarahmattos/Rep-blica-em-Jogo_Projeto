using Game.Territorio;
using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Logger = Game.Tools.Logger;
using Game.UI;


namespace Game
{

    public class InicializacaoState : State
    {
        //TODO: isolar DISTRIBUIÇÃO nos bairros em outra classe
        [SerializeField] private float intervaloTempo = 0.5f;
        [SerializeField] private HudStatsJogador hs;
        private ZonaTerritorial[] zonasTerritoriais;


        //OBS: embora não pareca ser necessario, sem SerializeField não funciona, por algum motivo.
        [SerializeField] private List<Bairro> todosBairros;
        private GameStateHandler stateHandler => GameStateHandler.Instance;

        public override void EnterState()
        {
            hs = FindObjectOfType<HudStatsJogador>();
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
            hs.AtualizarPlayerStatsBairro();

        }

        private List<Bairro> GetBairros()
        {
            List<Bairro> bairros = new List<Bairro>();
            for (int i = 0; i < zonasTerritoriais.Length; i++)
            {
                bairros.AddAll(zonasTerritoriais[i].Bairros);
            }

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
            //hs.AtualizarPlayerStatsBairro();
            stateHandler.NextStateServerRPC();



        }


    }

}