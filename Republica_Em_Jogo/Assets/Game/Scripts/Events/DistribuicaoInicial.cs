using Game;
using Game.Territorio;
using Game.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Territorio
{

    //(1) somente um jogador deve fazer a distribuicao e (2) os dados devem ser acess�veis a todos.
    //(3) Este compartilhamento � feito nos pr�prios bairros e atualizados por cada jogador.
    public class DistribuicaoInicial : NetworkSingleton<DistribuicaoInicial>
    {
        [SerializeField] private float intervaloTempo = 0.5f;
        private ZonaTerritorial[] zonasTerritoriais;
        private List<Bairro> todosBairros;


        private event Action distribuicaoStart;
        private event Action distribuicaoEnd;
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
                aux = (1 + aux) % (clients);

                yield return new WaitForSeconds(intervaloTempo);
            }
        }



    }

}
