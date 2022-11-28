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
        [SerializeField] private int intervaloTempo;
        private event Action distribuicaoStart;
        private event Action distribuicaoEnd;
        [SerializeField] private List<Bairro> todosBairros;
        private void Awake()
        {
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
            zonasTerritoriais.Shuffle();
            
            for (int i = 0; i < zonasTerritoriais.Length; i++)
            {
                todosBairros.AddAll(zonasTerritoriais[i].Bairros);

            }
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
            Logger.Instance.LogWarning(string.Concat("somos o serve: ", IsServer));
            if (!IsServer) return;

            distribuicaoStart?.Invoke();

            int clients = NetworkManager.Singleton.ConnectedClientsIds.Count;
            int aux = 0;
            Logger.Instance.LogInfo("Estamos distribuindo os territórios.");
            Logger.Instance.LogInfo(string.Concat("...", clients, " conectados."));
            foreach (ZonaTerritorial zona in zonasTerritoriais)
            {
                foreach (Bairro bairro in zona.Bairros)
                {
                    bairro.SetPlayerControl(aux);
                    Logger.Instance.LogInfo(string.Concat("aux atual: ", aux));
                    if (aux < clients-1)
                    {
                        aux++;
                    }
                    else
                    {
                        aux = 0;
                    }
                    //aux.NextValue(clients);
                }
            }
            distribuicaoEnd?.Invoke();
        }



        //private IEnumerator DefinePlayerNosBairros()
        //{
        //    yield return new WaitForSeconds(intervaloTempo);
        //}



    }

}
