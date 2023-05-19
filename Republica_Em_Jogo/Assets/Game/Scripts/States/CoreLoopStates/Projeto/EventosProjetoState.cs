using System;
using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using Game.Tools;
using UnityEngine;
using System.Linq;
using Game.Player;

namespace Game
{
    public class EventosProjetoState : MonoBehaviour
    {
        [SerializeField] private Projeto projeto;



        private void Start()
        {
            projeto.ProjetoAprovado += OnProjetoAprovado;
        }



        private void OnDestroy()
        {
            projeto.ProjetoAprovado -= OnProjetoAprovado;

        }


        private void OnProjetoAprovado(string zonaName)
        {
            ZonaTerritorial zona = GetZona(zonaName);
            InativarOutrasZonas(zona);
            if (!PlayerLocalTemBairroNaZonaEscolhida(zona)) return;
            zona.Bairros.MudarHabilitado(true);



        }

        private ZonaTerritorial GetZona(string zonaName)
        {
            foreach (ZonaTerritorial zona in SetUpZona.Instance.Zonas)
            {
                if (zona.Nome == zonaName)
                {
                    return zona;
                }
            }
            throw new NullReferenceException("zona {zonaName} não existe.");
        }


        private bool PlayerLocalTemBairroNaZonaEscolhida(ZonaTerritorial zonaEscolhida)
        {
            foreach (Bairro bairro in zonaEscolhida.Bairros)
            {
                if (bairro.playerInControl == PlayerStatsManager.Instance.GetLocalPlayerStats()) return true;
            }
            Debug.Log("Não tem bairros na zona escolhida");
            return false;
        }

        private void InativarOutrasZonas(ZonaTerritorial zonaEscolhida)
        {
            List<ZonaTerritorial> outrasZonas = SetUpZona.Instance.Zonas.ToList(); //ainda com todas
            outrasZonas.Remove(zonaEscolhida); //agora sim, só as outras zonas

            foreach (ZonaTerritorial zona in outrasZonas)
            {
                zona.Bairros.MudarInativity(true);
                zona.Bairros.MudarInteragivel(false);

            }
        }








    }
}
