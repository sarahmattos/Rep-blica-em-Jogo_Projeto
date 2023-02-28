using Game.Territorio;
using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Logger = Game.Tools.Logger;
using Game.UI;
using Game.Player;


namespace Game
{

    public class InicializacaoState : State
    {
        //TODO: isolar DISTRIBUI��O nos bairros em outra classe
        [SerializeField] private float intervaloTempo = 0.5f;
        [SerializeField] private HudStatsJogador hs;
        private ZonaTerritorial[] zonasTerritoriais;


        //OBS: embora n�o pareca ser necessario, sem SerializeField n�o funciona, por algum motivo.
        [SerializeField] private List<Bairro> todosBairros;
        private GameStateHandler stateHandler => GameStateHandler.Instance;

        public override void EnterState()
        {
            hs = FindObjectOfType<HudStatsJogador>();
            Logger.Instance.LogInfo("Enter state: Inicializa");
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
            todosBairros = GetBairros();
            todosBairros.Shuffle();            
            if (!IsServer) return;
            StartCoroutine(DistribuirBairros());
        }

        public override void ExitState()
        {
            StopAllCoroutines();
            Logger.Instance.LogInfo("Exit state: Inicializa");
            hs.AtualizarPlayerStatsBairro();
            EleicaoManager.Instance.ClientsConectServerRpc();
        }

        private List<Bairro> GetBairros()
        {
            List<Bairro> bairros = new List<Bairro>();
            foreach (ZonaTerritorial zonas  in zonasTerritoriais)
            {
                bairros.AddAll(zonas.Bairros);
            }
            return bairros;

        }

        private IEnumerator DistribuirBairros()
        {
            if (!IsServer) yield return null;

            int clients = NetworkManager.Singleton.ConnectedClientsIds.Count;
            int aux = 0;

            foreach (Bairro bairro in todosBairros)
            {
                bairro.SetPlayerControlServerRpc(aux);
                bairro.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(1);
                aux = (1 + aux) % (clients);

                yield return new WaitForSeconds(intervaloTempo);
            }
            stateHandler.ChangeStateServerRpc((int)GameState.DESENVOLVIMENTO);



        }


    }

}