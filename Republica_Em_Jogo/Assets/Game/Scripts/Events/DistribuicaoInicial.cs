using Game;
using Game.Territorio;
using Game.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Game.Tools.Logger;

namespace Territorio
{

    //(1) somente um jogador deve fazer a distribuicao e (2) os dados devem ser acess�veis a todos.
    //(3) Este compartilhamento � feito nos pr�prios bairros e atualizados por cada jogador.
    public class DistribuicaoInicial : NetworkSingleton<DistribuicaoInicial>
    {
        [SerializeField] private float intervaloTempo = 0.5f;
        private ZonaTerritorial[] zonasTerritoriais;
        
        //OBS: embora não pareca ser necessario, sem SerializeField não funciona, por algum motivo.
        [SerializeField] private List<Bairro> todosBairros;


        private event Action distribuicaoStart;
        private event Action distribuicaoEnd;
        private void Awake()
        {
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
        }

        private void Start()
        {
           todosBairros = GetBairros();
        }

        public override void OnNetworkSpawn()
        {
            GameStateHandler.Instance.initialDistribution += DistribuiBairros;

        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.initialDistribution -= DistribuiBairros;
        }

        private void DistribuiBairros()
        {
            if (!IsServer) return;

            distribuicaoStart?.Invoke();
            StartCoroutine(DefinePlayerNosBairros());
            distribuicaoEnd?.Invoke();
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


        private IEnumerator DefinePlayerNosBairros()
        {
            int clients = NetworkManager.Singleton.ConnectedClientsIds.Count;
            int aux = 0;

            foreach (Bairro bairro in todosBairros)
            {
                Logger.Instance.LogInfo(aux.ToString());
                bairro.SetPlayerControl(aux);                
                aux = (1 + aux) % (clients);

                yield return new WaitForSeconds(intervaloTempo);
            }
        }



    }

}
