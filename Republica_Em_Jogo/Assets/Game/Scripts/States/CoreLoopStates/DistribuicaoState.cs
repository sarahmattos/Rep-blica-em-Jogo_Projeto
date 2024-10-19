using Game.Player;
using Game.Territorio;
using Game.Tools;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DistribuicaoState : State
    {
        [SerializeField]
        private RecursosCartaManager rc;

        public string explicaTexto, explicaTextoCorpo;
        private UICoreLoop uiCore;

        [SerializeField]
        private HudStatsJogador hs;
        private List<Bairro> BairrosDoPlayerAtual => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;
        private RodadaController rodadaController;
        private SetUpZona setUpZona;

        public void Start()
        {
            TurnManager.Instance.vezDoPlayerLocal += quandoVezPlayerLocal;
            uiCore = FindObjectOfType<UICoreLoop>();
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
        }

        public override void EnterState()
        {
            if (TurnManager.Instance.LocalIsCurrent)
            {
                BairrosDoPlayerAtual.MudarHabilitado(true);
                rodadaController = FindObjectOfType<RodadaController>();
                uiCore.MostrarAvisoEstado(explicaTexto, explicaTextoCorpo);
                GameStateEmitter.SendMessage("Ditribua novos eleitores e troque cartas por recursos.");
            }
            else
            {
                GameStateEmitter.SendMessage("Aguarde ações do jogador.");
            }
        }




        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            hs.distribuicaoInicial = false;
            BairrosDoPlayerAtual.MudarHabilitado(false);
            setUpZona.resetaParticulaUI();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            TurnManager.Instance.vezDoPlayerLocal -= quandoVezPlayerLocal;
        }

        public void quandoVezPlayerLocal(bool value)
        {
            StartCoroutine(Espera3(1, value));
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
                if (EleicaoManager.Instance.inEleicao)
                {
                    StartCoroutine(EsperaEVai2(5));
                }
                else
                {
                    hs.distribuicaoInicial = true;
                    rc.conferirQuantidade();
                }

            }
        }
    }
}
