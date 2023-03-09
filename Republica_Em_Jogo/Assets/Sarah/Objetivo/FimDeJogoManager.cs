using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Territorio;
using Game.UI;
using Game.Player;

namespace Game
{
    public class FimDeJogoManager : MonoBehaviour
    {
        private SetUpZona setUpZona;
        private HudStatsJogador hs;
        public static FimDeJogoManager Instance;
        private PlayerStats ps;
        public bool vitoria;
        public void Start(){
            Instance = this;
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            hs = FindObjectOfType<HudStatsJogador>();
            
        }
        /*
        public void TenhoUmaZonaInteria(List<ZonaTerritorial> _zonaObtidaPorCompleto, List<ZonaTerritorial> tenhoBairro){
            for(int i=0;i<+_zonaObtidaPorCompleto.Count;i++){
                Debug.Log("Tenho a zona "+_zonaObtidaPorCompleto[i].Nome+" inteira!");
                tenhoBairro.Add(_zonaObtidaPorCompleto[i]);
            }
            
        }
        */
        public void zonaObtidaEObjetivo(){
            ps = hs.GetPlayerStats();
            if(setUpZona.tenhoZona.Count==0)Debug.Log("Não ganhou ainda!");
            for(int i=0;i<setUpZona.tenhoZona.Count;i++){
                if(setUpZona.tenhoZona[i].Nome==ps.Objetivo){
                    if(setUpZona.tenhoZona[i].ContaRecursosEducacao()>=2 && setUpZona.tenhoZona[i].ContaRecursosSaude()>=2){
                          vitoriaPlayer();
                    }else{
                        Debug.Log("Não tem recursos suficientes na zona de objetivo ainda!");
                    }
            
                }Debug.Log("Não tem zona do objetivo ainda!");
            }
            
        }
        public void vitoriaPlayer(){
            vitoria=true;
            Debug.Log("Você ganhou!");
        }
    }
    
}
