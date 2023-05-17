using Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UICoreLoop : Singleton<UICoreLoop>
    {
        [SerializeField] private Button nextStateButton;
        [SerializeField] private TMP_Text playerAtualText;
        [SerializeField] private TMP_Text logStateText;
        [SerializeField] public TMP_Text ExplicaStateTextTitulo;
        [SerializeField] public TMP_Text ExplicaStateTextCorpo;
        [SerializeField] public GameObject ExplicaStateUi;
        private RodadaController rodadaController;
        private Animator animator;
        public Button NextStateButton => nextStateButton;
        private State DesenvolvimentoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DESENVOLVIMENTO);
        private string TagPlayerAtualStilizado
        {
            get
            {
                return string.Concat(GameDataconfig.Instance.TagParticipante, " ", TurnManager.Instance.PlayerAtual);

            }
        }



        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {

            nextStateButton.onClick.AddListener(OnNextStateButtonClick);

            DesenvolvimentoState.Entrada += OnDesenvolvimentoStateEnter;
            TurnManager.Instance.vezDoPlayerLocal += OnPlayerTurnUpdate;
            CoreLoopStateHandler.Instance.estadoMuda += UpdateTitleText;

            nextStateButton.gameObject.SetActive(false);

        }

        private void OnDestroy()
        {
            DesenvolvimentoState.Saida -= OnDesenvolvimentoStateEnter;

            TurnManager.Instance.vezDoPlayerLocal -= OnPlayerTurnUpdate;
            CoreLoopStateHandler.Instance.estadoMuda -= UpdateTitleText;


        }

        private void OnDesenvolvimentoStateEnter()
        {
            rodadaController = FindObjectOfType<RodadaController>();
            if (rodadaController.Rodada == 1) PlayEnterAnim();

        }
        // private void OnRodadaMuda(int rodada)
        // {
        //     Debug.Log("Rodada atualiza");
        //     if (rodada == 1) PlayEnterAnim();

        // }

        private void PlayEnterAnim()
        {
            animator.Play("EnterUiCoreLoop");
        }

        public void OnNextStateButtonClick()
        {
            CoreLoopStateHandler.Instance.NextStateServerRpc();

        }


        private void OnPlayerTurnUpdate(bool value)
        {
            nextStateButton.gameObject.SetActive(value);

            // UpdateTitleText(Tools.CollectionExtensions.KeyByValue(CoreLoopStateHandler.Instance.StatePairValues, CoreLoopStateHandler.Instance.CurrentState));

        }

        // private void UpdateTextDesenv(CoreLoopState state)
        // {
        //     UpdateTitleText(state);
        // }

        public void MostrarAvisoEstado(string avisoTitulo, string avisoCorpo)
        {
            rodadaController = FindObjectOfType<RodadaController>();
            int rodada = rodadaController.Rodada;
            if (rodada <= 1)
            {
                ExplicaStateTextTitulo.text = avisoTitulo;
                ExplicaStateTextCorpo.text = avisoCorpo;
                if (TurnManager.Instance.LocalIsCurrent) ExplicaStateUi.SetActive(true);
            }
        }

        public void UpdateTitleText(CoreLoopState state)
        {
            // string titleText = string.Concat( state.ToString());
            logStateText.SetText(state.ToString());
            playerAtualText.SetText(GameDataconfig.Instance.TagPlayerAtualColorizada());

        }


    }

}

