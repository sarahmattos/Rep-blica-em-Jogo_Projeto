using System;
using Game.Player;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class UIEleitorCurrency : MonoBehaviour
    {
        [SerializeField] private TMP_Text textEleitoresNovos;
        private Animator animator;
        private State InicializacaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO);
        private PlayerStats PlayerStatsLocal => PlayerStatsManager.Instance.GetLocalPlayerStats();

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        private void Start()
        {
            PlayExitAnim();
            InicializacaoState.Saida += SubscribeOnGameplaySceneLoad;
        }

        private void OnDestroy()
        {
            //desinscrevendo o metodo de subscrição
            InicializacaoState.Saida -= SubscribeOnGameplaySceneLoad;

            //desinscrevendo os metodos subscritos dentro do metodo de subscrição. hahahah
            PlayerStatsLocal.DefiniuEleitoresNovos -= ConfigureCurrency;
            PlayerStatsLocal.DistribuiuEleitor -= SetPositiveText;
            PlayerStatsLocal.FimDistricaoEleitores -= PlayExitAnim;

        }

        private void SubscribeOnGameplaySceneLoad()
        {
            Tools.Logger.Instance.LogInfo("Player stats local: " + PlayerStatsLocal.Nome);
            PlayerStatsLocal.DefiniuEleitoresNovos += ConfigureCurrency;
            PlayerStatsLocal.DistribuiuEleitor += SetPositiveText;
            PlayerStatsLocal.FimDistricaoEleitores += PlayExitAnim;
        }

        public void PlayEnterAnim()
        {
            animator.Play("CurrencyEnter");
        }

        public void PlayExitAnim()
        {
            animator.Play("CurrencyExit");
        }

        public void ConfigureCurrency(int eleitores)
        {
            PlayEnterAnim();
            SetCurrentText(string.Concat("+", eleitores));
        }


        public void SetPositiveText(int value)
        {
            SetCurrentText(string.Concat("+", value));
        }

        public void SetNegativeText(int value)
        {
            SetCurrentText(string.Concat("-", value));
        }

        private void SetCurrentText(string value)
        {
            textEleitoresNovos.SetText(value);
        }

    }
}
