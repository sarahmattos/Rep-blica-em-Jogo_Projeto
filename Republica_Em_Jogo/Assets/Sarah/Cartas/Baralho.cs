using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Territorio
{
    public class Baralho : MonoBehaviour
    {
        private Projeto projeto;
        private Corrupcao corrupcao;
        private MovimentosSociais movimentosSociais;

        void Start(){
            projeto = FindObjectOfType<Projeto>();
            corrupcao = FindObjectOfType<Corrupcao>();
            movimentosSociais = FindObjectOfType<MovimentosSociais>();
        }
        
        private void OnMouseDown()
        {
            if (!enabled) return;
            int rnd = Random.Range(0,100);
            if(rnd>=0 && rnd<25)corrupcao?.sortearCorrupcao();
            if(rnd>=25 && rnd<50)movimentosSociais?.sortearMS();
            if(rnd>=50)projeto?.sortearProjeto();
            this.enabled = false;
        }
        public void cartaProjetoTrue(){
        projeto.projetoUI.SetActive(true);
        projeto.verProjetoBtn.SetActive(false);
    }
        public void cartaProjetoFalse(){
        projeto.projetoUI.SetActive(false);
        projeto.verProjetoBtn.SetActive(true);
    }
        
    }
}

