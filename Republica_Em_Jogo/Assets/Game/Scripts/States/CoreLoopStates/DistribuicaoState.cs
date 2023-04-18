using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Territorio;
using Game.Player;

namespace Game
{
    public class DistribuicaoState : State
    {
        [SerializeField]
        private RecursosCartaManager rc;
        
        public string explicaTexto,explicaTextoCorpo;
        private UICoreLoop uiCore;

        [SerializeField]
        private HudStatsJogador hs;
        private List<Bairro> bairrosDoPlayerAtual;
        private RodadaController rodadaController;

        public void Start()
        {
            bairrosDoPlayerAtual = new List<Bairro>();
            TurnManager.Instance.vezDoPlayerLocal += quandoVezPlayerLocal;
            uiCore = FindObjectOfType<UICoreLoop>();
        }

        public override void EnterState()
        {
            // Tools.Logger.Instance.LogPlayerAction("Distribuindo eleitores");
            if(!TurnManager.Instance.LocalIsCurrent) return;
            bairrosDoPlayerAtual.AddRange(
                PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl
            );
            foreach (Bairro bairro in bairrosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(true);
            }
             rodadaController = FindObjectOfType<RodadaController>();
            
            
            uiCore.MostrarAvisoEstado(explicaTexto,explicaTextoCorpo);
        }
       
        public override void ExitState()
        {
            if(!TurnManager.Instance.LocalIsCurrent) return;
            hs.distribuicaoInicial = false;
            foreach (Bairro bairro in bairrosDoPlayerAtual)
            {
                bairro.Interagivel.MudarHabilitado(false);
            }
            bairrosDoPlayerAtual.Clear();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            TurnManager.Instance.vezDoPlayerLocal -= quandoVezPlayerLocal;
        }

        public void quandoVezPlayerLocal(bool value)
        {
             StartCoroutine(Espera3(1,value));
        }
         private IEnumerator EsperaEVai2(int s)
        {
            yield return new WaitForSeconds(s);
             hs.distribuicaoInicial = true;
            rc.conferirQuantidade();
        }
        private IEnumerator Espera3(int s, bool value)
        {
            yield return new WaitForSeconds(s);
            if (value)
            {
                if(EleicaoManager.Instance.inEleicao){
                    StartCoroutine(EsperaEVai2(5));
                }else{
                    hs.distribuicaoInicial = true;
                    rc.conferirQuantidade();
                }
                
            }
        }
    }
}
