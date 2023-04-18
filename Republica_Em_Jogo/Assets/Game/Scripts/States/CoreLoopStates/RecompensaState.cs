using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Territorio;
using UnityEngine;
using System.Linq;
using Game.UI;
using TMPro;

namespace Game
{
    public class RecompensaState : State
    {
        [SerializeField] private HudStatsJogador hudStatsJogador;
        [SerializeField] private FimDeJogoManager fimDeJogoManager;

        [SerializeField] private AvancoState avancoState;
        [SerializeField] private int qntdRecurso = 1;
        private AvancoData avancoData => avancoState.AvancoData;
        // private const int recompensaEleitores = 2;
        // private int qntdEleitorAplicado = 0;
        public bool TemRecompensa =>  (avancoData.BairrosAdquiridos) > 0 ? true : false;
        // public bool AplicouTodosEleitores => qntdEleitorAplicado == recompensaEleitores;
        public PlayerStats PlayerStatsAtual =>  PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual();
        public List<Bairro> bairrosDoPlayer => PlayerStatsAtual?.BairrosInControl;
        public string explicaTexto,explicaTextoCorpo;
        private UICoreLoop uiCore;
        [SerializeField] GameObject recompensaAviso;

        public override void EnterState()
        {
            hudStatsJogador.checaZonasInteiras();
            fimDeJogoManager.zonaObtidaEObjetivo();
            Tools.Logger.Instance.LogInfo("Enter State: RECOMPENSA");
            if(TurnManager.Instance.LocalIsCurrent){
                 recompensaAviso.SetActive(true);
                 GameObject go =recompensaAviso.transform.GetChild(0).gameObject;
                TMP_Text textoAviso = go.GetComponent<TMP_Text>();
                uiCore.MostrarAvisoEstado(explicaTexto,explicaTextoCorpo);

                if(!TemRecompensa) {
                // Tools.Logger.Instance.LogInfo("Como não avançou em nenhum bairro, não há recompensa nesta rodada.");
                textoAviso.text="Não recebe recompensa pois não influenciou um novo bairro!";
                return;
                 } 
            hudStatsJogador.updateRecursoCartaUI(qntdRecurso);
            textoAviso.text="Você ganhou uma carta de recurso por ter influenciado um novo bairro!";
            }
           
            
            
            // qntdEleitorAplicado = 0;
            // HabilitarBairros(true);
            // InscreverBairros();

        }
        private void Start()
        {
            uiCore = FindObjectOfType<UICoreLoop>();
        }
        public override void ExitState()
        {
            // HabilitarBairros(false);
            // DesincreverBairros();
        }

        private void HabilitarBairros(bool value) {
            foreach(Bairro bairro in bairrosDoPlayer) {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        // private void InscreverBairros() {
        //     foreach (Bairro bairro in bairrosDoPlayer)
        //     {
        //         bairro.Interagivel.click += OnClick;
        //     }
        // }

        // private void DesincreverBairros() {
        //     foreach (Bairro bairro in bairrosDoPlayer)
        //     {
        //         bairro.Interagivel.click -= OnClick;
        //     }
        // }

        // private void AplicarRecompensa(Bairro bairro) {
        //     bairro.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(1);
        //     qntdEleitorAplicado++;     

        // }

        // private void OnClick(Bairro bairro) {
        //     if(AplicouTodosEleitores) {
        //         Tools.Logger.Instance.LogInfo("Recompensas aplicadas.");
        //         return;
        //     }
        //     AplicarRecompensa(bairro);

            
        // }








    }

}
