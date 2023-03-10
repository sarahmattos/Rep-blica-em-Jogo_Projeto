using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;
using Game.Player;
using Game.Territorio;

namespace Game
{
    public class FimDeJogoManager : NetworkBehaviour
    {
        private NetworkVariable<FixedString4096Bytes> VitoriaTextServer = new NetworkVariable<FixedString4096Bytes>();
        [SerializeField] private TMP_Text text_vitoria, text_vitoria2;
        [SerializeField] private GameObject vitoriaUi;
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
        [ServerRpc(RequireOwnership = false)]
        public void TesteServerRpc(string textoTotal2)
        {
            VitoriaTextServer.Value = textoTotal2;
        }
        
         private void OnEnable()
    {
        //jogadores conectados
      
        VitoriaTextServer.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
        {
             if (newValue != "")
            {
                vitoriaUi.SetActive(true);
                if(vitoria){
                    text_vitoria.text="Vitória";
                    text_vitoria2.text="Você venceu";
                }else{
                    text_vitoria.text="Derrota";
                    text_vitoria2.text=newValue.ToString();
                }
                
            }
        };
        
    }

        public void zonaObtidaEObjetivo(){
            ps = hs.GetPlayerStats();
            if(setUpZona.tenhoZona.Count==0)Debug.Log("Não ganhou ainda!");
            for(int i=0;i<setUpZona.tenhoZona.Count;i++){
                if(setUpZona.tenhoZona[i].Nome==ps.Objetivo){
                    if(setUpZona.tenhoZona[i].ContaRecursosEducacao()>=2 && setUpZona.tenhoZona[i].ContaRecursosSaude()>=2){
                          vitoriaPlayer(setUpZona.tenhoZona[i].Nome);
                    }else{
                        Debug.Log("Não tem recursos suficientes na zona de objetivo ainda!");
                    }
            
                }Debug.Log("Não tem zona do objetivo ainda!");
            }
            
        }
        public void vitoriaPlayer(string _zonaObjectivo){
            vitoria=true;
            Debug.Log("Você ganhou!");
            string textoTotal = "Jogador "+NetworkManager.Singleton.LocalClientId.ToString()+" venceu conquistando a zona "+ _zonaObjectivo+" com recursos suficientes!";
            TesteServerRpc(textoTotal);
        }
        public void VoltarMenu(){
             //Application.LoadLevel("MainMenuScene");
             Application.Quit();
        }
    }
    
}
