using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Button))]
    public class NextStateButtonHandler : MonoBehaviour
    {
        [SerializeField] private SelectBairroAvancoState selectBairroAvancoState;
        [SerializeField] private SelectBairroRemanejamentoState selectBairroRemanejamentoState;
        
        public State DistribuicaoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.DISTRIBUICAO];
        public State ProjetoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.PROJETO];
        public State EleicaoState => GameStateHandler.Instance.StateMachineController.GetState((int)CoreLoopState.PROJETO);
        private Button button;
        private void Awake()
        {
            button = GetComponent<Button>();
        }
        void Start()
        {
            DistribuicaoState.Entrada += DeactiveButton;
            DistribuicaoState.Saida += ActiveButton;
            selectBairroAvancoState.Entrada += ActiveButton;
            selectBairroAvancoState.Saida += DeactiveButton;            
            selectBairroRemanejamentoState.Entrada += ActiveButton;
            selectBairroRemanejamentoState.Saida += DeactiveButton;
            ProjetoState.Entrada += DeactiveButton;
        }



        private void OnDestroy()
        {
            DistribuicaoState.Entrada -= DeactiveButton;
            DistribuicaoState.Saida -= ActiveButton;
            selectBairroAvancoState.Entrada -= ActiveButton;
            selectBairroAvancoState.Saida -= DeactiveButton;            
            selectBairroRemanejamentoState.Entrada -= ActiveButton;
            selectBairroRemanejamentoState.Saida -= DeactiveButton;
            ProjetoState.Entrada -= DeactiveButton;
        }

        private void OnEnable()
        {
            DeactiveButton();
        }

        private void DeactiveButton()
        {
            button.interactable = false;
        }

        private void ActiveButton()
        {
            button.interactable = true;

        }


    }
}
