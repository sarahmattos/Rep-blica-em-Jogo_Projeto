using UnityEngine;
namespace Game.UI
{
    public class Baralho : MonoBehaviour
    {
        private Projeto projeto;
        private Corrupcao corrupcao;
        private MovimentosSociais movimentosSociais;

        private Animator animator;
        [SerializeField] GameObject baralho2D;

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
        }

        private void OnDestroy()
        {
            ProjetoState.Entrada -= () => { animator.Play("MostrarBaralho"); };
        }

        public void sortearAcao()
        {
            int rnd = Random.Range(0, 100);
            if (rnd >= 0 && rnd < 25) corrupcao?.sortearCorrupcao();
            if (rnd >= 25 && rnd < 50) movimentosSociais?.sortearMS();
            if (rnd >= 50) projeto?.sortearProjeto();
        }
        public void baralhoManager(bool valor)
        {
            baralho2D.SetActive(valor);
        }

        // public void cartaProjetoTrue()
        // {
        //     projeto.verProjetoBtn.SetActive(false);
        // }
        // public void cartaProjetoFalse()
        // {
        //     //projeto.RestoUI.SetActive(false);
        //     projeto.verProjetoBtn.SetActive(true);
        // }



    }
}

