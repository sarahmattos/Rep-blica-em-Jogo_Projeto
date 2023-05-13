using UnityEngine;
namespace Game.UI
{
    public class Baralho : MonoBehaviour
    {
        private Projeto projeto;
        private Corrupcao corrupcao;
        private MovimentosSociais movimentosSociais;

        RodadaController rodadaController;
        private Animator animator;
        [SerializeField] GameObject baralho2D;
        public int porcentagemCop, porcentagemMs;
            
        public State ProjetoState =>
            CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.PROJETO];
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

        }
        public void PuxarCartaAnimation()
        {
            animator.Play("PuxarCarta");
            
        }


        void Start()
        {
            projeto = FindObjectOfType<Projeto>();
            corrupcao = FindObjectOfType<Corrupcao>();
            movimentosSociais = FindObjectOfType<MovimentosSociais>();
            baralhoManager(false);

            ProjetoState.Entrada += () => { animator.Play("MostrarBaralho"); };
            porcentagemCop=1;
            porcentagemMs=2;
        }

        private void OnDestroy()
        {
            ProjetoState.Entrada -= () => { animator.Play("MostrarBaralho"); };
        }

        public void sortearAcao()
        {
            rodadaController = FindObjectOfType<RodadaController>();
            int rodada = rodadaController.Rodada;
            if(rodada>1 && rodada<=7){
                porcentagemCop +=4;
                porcentagemMs +=8;
            }
            int rnd = Random.Range(0, 100);
            if (rnd >= 0 && rnd < porcentagemCop) corrupcao?.sortearCorrupcao();
            if (rnd >= porcentagemCop && rnd < porcentagemMs) movimentosSociais?.sortearMS();
            if (rnd >= porcentagemMs) projeto?.sortearProjeto();
        }
        public void baralhoManager(bool valor)
        {
            baralho2D.SetActive(valor);
        }


    }
}

