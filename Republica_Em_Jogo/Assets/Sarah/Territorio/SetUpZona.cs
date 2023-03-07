using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.UI;
using Game.Tools;

namespace Game.Territorio
{
    public class SetUpZona : Singleton<SetUpZona>
    {
        private ZonaTerritorial[] zonas;
        public ZonaTerritorial[] Zonas => zonas;
        private HudStatsJogador hs;
        public List<ZonaTerritorial> tenhoZona;

        void Start()
        {
            hs = FindObjectOfType<HudStatsJogador>();
        }

        private void Awake()
        {
            zonas = GetComponentsInChildren<ZonaTerritorial>();
        }

        public void eleitoresZona(int valor, string nome)
        {
            foreach (ZonaTerritorial zona in zonas)
            {
                if (zona.Nome == nome)
                {
                    zona.adicionarEleitoresZona(valor);
                }
            }
        }

        public void playerZona(ulong valor, string nome)
        {
            foreach (ZonaTerritorial zona in zonas)
            {
                if(zona.Nome==nome){
                    //Debug.Log("recursoNameZona: "+nome);
                    zona.verificarPlayerNasZonas(valor);
                }
            }
        }

        public void chamar()
        {
            eleitoresZona(2, "Oeste1");
        }

        public void ChamarReseteBairroNaZona()
        {
            foreach (ZonaTerritorial zona in zonas)
            {
                zona.ResetarBairroNaZona();
            }
        }


        public void ProcurarBairrosInZona()
        {
            PlayerStats ps = hs.GetPlayerStats();
            List<Bairro> listaAux = new List<Bairro>();
            foreach (ZonaTerritorial zona in zonas)
            {
                zona.ContarBairroInControl(ps, listaAux);
            }
            ps.BairrosInControl = new List<Bairro>();
            for (int i = 0; i < ps.BairrosInControl.Count; i++)
            {
                ps.BairrosInControl[i] = listaAux[i];
            }

        }
        
        public void SepararBairrosPorPlayer(int[] _eleitoresPlayers, int numPlayer)
        {
            foreach (ZonaTerritorial zona in zonas)
            {
                zona.ContarBairroInControlTodosPlayers(_eleitoresPlayers, numPlayer);
            }
        }
        public void PlayerTemZonaInteira(int client){
            tenhoZona = new List<ZonaTerritorial>();
            foreach (ZonaTerritorial zona in zonas)
            {
               if(zona.checaSePlayerTemTodosBairrosDeUmaZona(client)!=null){
                tenhoZona.Add(zona.checaSePlayerTemTodosBairrosDeUmaZona(client));
               } 
            }
        }

        
    }
}
