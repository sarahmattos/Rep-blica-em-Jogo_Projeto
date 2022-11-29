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

    //(1) somente um jogador deve fazer a distribuicao e (2) os dados devem ser acessíveis a todos.
    //(3) Este compartilhamento é feito nos próprios bairros e atualizados por cada jogador.
    public class DistribuicaoInicial : NetworkSingleton<DistribuicaoInicial>
    {
        [SerializeField] private ZonaTerritorial[] zonasTerritoriais;
        [SerializeField] private float intervaloTempo = 0.5f;
        private event Action distribuicaoStart;
        private event Action distribuicaoEnd;
        [SerializeField] private List<Bairro> todosBairros;
        private void Awake()
        {
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
        }

        private void Start()
        {
            for (int i = 0; i < zonasTerritoriais.Length; i++)
            {
                todosBairros.AddAll(zonasTerritoriais[i].Bairros);

            }

            todosBairros.Shuffle();
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



        private IEnumerator DefinePlayerNosBairros()
        {
            int clients = NetworkManager.Singleton.ConnectedClientsIds.Count;
            int aux = 0;

            foreach (Bairro bairro in todosBairros)
            {
                bairro.SetPlayerControl(aux);

                
                if (aux < clients - 1)
                {
                    aux++;
                }
                else
                {
                    aux = 0;
                }

                yield return new WaitForSeconds(intervaloTempo);
            }
        }



    }

}
