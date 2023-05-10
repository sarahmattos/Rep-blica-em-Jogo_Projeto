using System;
using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using Game.Tools;
using UnityEngine;
using System.Linq;

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


        private void OnProjetoAprovado(string zonaEscolhida)
        {
            Tools.Logger.Instance.LogInfo(string.Concat("projeto aprovado na zona: ", zonaEscolhida));

            foreach (ZonaTerritorial zona in SetUpZona.Instance.Zonas)
            {
                if (zona.name == zonaEscolhida)
                {
                    MudaAtivityOutrasZonas(zona);
                    break;
                }
            }

        }

        private void MudaAtivityOutrasZonas(ZonaTerritorial zonaEscolhida)
        {
            IEnumerable<ZonaTerritorial> outrasZonas = SetUpZona.Instance.Zonas.Except((IEnumerable<ZonaTerritorial>)zonaEscolhida);
            foreach(ZonaTerritorial zona in outrasZonas) {
                zona.Bairros.MudarInativity(true);
                zona.Bairros.MudarInteragivel(false);

            }
        }



    }
}
