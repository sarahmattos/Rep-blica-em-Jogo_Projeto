using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.UI;
namespace Game.Territorio

{
public class SetUpZona : MonoBehaviour
{
    //public static SetUpZona instance;
    private ZonaTerritorial[] zonas;
    public ZonaTerritorial[] ZonaTerritorial => zonas;
    private HudStatsJogador hs;
        void Start()
    {
            //instance= this;
            hs = FindObjectOfType<HudStatsJogador>();
        }

    private void Awake()
        {
            zonas = GetComponentsInChildren<ZonaTerritorial>();
            
        }
        public void eleitoresZona(int valor, string nome){
            foreach(ZonaTerritorial zona in zonas)
            {
                if(zona.Nome==nome){
                    zona.adicionarEleitoresZona(valor);
                }
            }

        }
        public void playerZona(ulong valor, string nome){
            foreach(ZonaTerritorial zona in zonas)
            {
                if(zona.Nome==nome){
                    //Debug.Log("recursoNameZona: "+nome);
                    zona.verificarPlayerNasZonas(valor);
                }
            }
        }
            
        public void chamar(){
            eleitoresZona(2,"Oeste1");
        }
        public void ChamarReseteBairroNaZona(){
            foreach(ZonaTerritorial zona in zonas)
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
            ps.bairrosInControl = new Bairro[listaAux.Count];
            for (int i = 0; i < ps.bairrosInControl.Length; i++)
            {
                ps.bairrosInControl[i] = listaAux[i];
            }

        }

        public void SepararBairrosPorPlayer(int[] _eleitoresPlayers, int numPlayer)
        {
            foreach (ZonaTerritorial zona in zonas)
            {
                zona.ContarBairroInControlTodosPlayers(_eleitoresPlayers, numPlayer);
            }

        }

    }
}
