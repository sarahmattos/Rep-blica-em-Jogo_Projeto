using System;
using System.Collections.Generic;
using Game.Player;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    public class CalculoCadeirasEleicao
    {
        private int cadeirasTotais = 12;
        private int TotalEleitoresInTerritorio
        {
            get
            {
                int eleitoresInTerritorio = 0;
                foreach (Bairro bairro in SetUpZona.Instance.AllBairros)
                {
                    eleitoresInTerritorio += bairro.SetUpBairro.Eleitores.contaEleitores;
                }
                return eleitoresInTerritorio;
            }
        }

        public int Calcular(PlayerStats playerStats)
        {
            return RecontagemPorArredondamento(playerStats);
            //ou escolhendo o método abaixo:
            // return RecontagemOficial(playerStats);
        }

        private int RecontagemPorArredondamento(PlayerStats playerStats)
        {
            float cadeirasDoPlayer = (float)cadeirasTotais * playerStats.GetEleitoresTotais() / TotalEleitoresInTerritorio;
            int cadeirasArredondado = (int)Mathf.Round(cadeirasDoPlayer);
            return cadeirasArredondado;
        }



        #region RecontagemOficial
        //REFERENCIAS
        //Regras aqui: https://prmdcp2.wixsite.com/mppeb/blank-ghuf9
        //https://www.cnnbrasil.com.br/politica/entenda-como-e-feita-a-conta-que-define-o-numero-de-deputados-do-meu-estado/
        private int RecontagemOficial(PlayerStats playerStats)
        {
            List<PlayerStats> players = playerStatsInOrdem();
            float qe = TotalEleitoresInTerritorio / cadeirasTotais;
            List<float> allQp = quocientesPartidarios(players, qe);
            List<int> allQpCeiling = AllQpCeiling(allQp);
            float CeilingDiff = this.CeilingDiff(allQp);
            List<float> maiorMedia = MaiorMedia(allQpCeiling, players);

            //falta so as cadeiras extras

            return 0;
        }

        private List<PlayerStats> playerStatsInOrdem()
        {
            List<PlayerStats> playerStats = new List<PlayerStats>();
            foreach (int id in TurnManager.Instance.ordemPlayersID)
            {
                PlayerStats ps = PlayerStatsManager.Instance.GetPlayerStats(id);
                playerStats.Add(ps);
            }

            return playerStats;
        }

        private List<float> quocientesPartidarios(List<PlayerStats> playerStats, float qe)
        {
            List<float> allQp = new List<float>();
            foreach (PlayerStats ps in playerStats)
            {
                float qp = ps.GetEleitoresTotais() / qe;
                allQp.Add(qp);
            }
            return allQp;
        }

        private List<int> AllQpCeiling(List<float> allQp)
        {
            List<int> allQpCeiling = new List<int>();
            foreach (float value in allQp)
            {
                int qpCeiling = (int)Mathf.Ceil(value);
                allQpCeiling.Add(qpCeiling);
            }
            return allQpCeiling;
        }

        private float CeilingDiff(List<float> allQp)
        {
            float diff = 0;
            foreach (float value in allQp)
            {
                int qpCeiling = (int)Mathf.Ceil(value);
                diff = (value - qpCeiling);
            }
            return diff;
        }

        private List<float> MaiorMedia(List<int> allQpCeiling, List<PlayerStats> players)
        {
            List<float> mmTotal = new List<float>();
            for (int i = 0; i < players.Count; i++)
            {
                float mmPlayer = (allQpCeiling[i] / (players[i].GetEleitoresTotais() + 1));
                mmTotal.Add(mmPlayer);
            }
            return mmTotal;
        }

        #endregion


    }

}
