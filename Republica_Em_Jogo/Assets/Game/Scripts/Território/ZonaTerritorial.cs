
using Game.Tools;
using UnityEngine;
 using Game.UI;
using Game.Player;
using System.Collections.Generic;
namespace Game.Territorio
{
    public class ZonaTerritorial : MonoBehaviour
    {
        [SerializeField] private string nome;
        private Bairro[] bairros;
        public Bairro[] Bairros => bairros;
        public string Nome { get => nome; }
        private Projeto projeto;  
        private HudStatsJogador hs;
        private ControlePassarState cp;
        private int soma, soma2, soma3;
        public List<Outline> outlines;
        public int eleitoresAdicionais;

        private void Awake()
        {
            bairros = GetComponentsInChildren<Bairro>();
             projeto = FindObjectOfType<Projeto>();
             hs = FindObjectOfType<HudStatsJogador>();
             cp = FindObjectOfType<ControlePassarState>();
        }

        public void verificarPlayerNasZonas(ulong client)
        {
            foreach(Bairro bairro in bairros)
            {
                if(bairro.PlayerIDNoControl.Value == (int)client)
                {
                    projeto.playerInZona=true;
                }
            }
        }
       private void Start()
       {    
           foreach(Bairro bairro in bairros){
               Outline outline = bairro.GetComponentInChildren<Outline>();
                outlines.Add(outline);
           }
       }
        public void adicionarEleitoresZona(int valor){
            foreach(Bairro bairro in bairros)
            {
                bairro.bairroNaZonaEscolhida=true;
                if(bairro.VerificaControl()){
                hs.ValorEleitoresNovos(valor);
                }
            }

        }
        
        public void ResetarBairroNaZona(){
            foreach(Bairro bairro in bairros){
                bairro.bairroNaZonaEscolhida=false;
                }
            }
        public void ContarBairroInControl(PlayerStats _ps, List<Bairro> _listAux) {
            
            foreach (Bairro bairro in bairros)
            {
                if (bairro.PlayerIDNoControl.Value == _ps.playerID)
                {
                    _listAux.Add(bairro);
                }
            }
           
        }
        public void ContarBairroInControlTodosPlayers(int[] _eleitoresPlayers, int numPlayer)
        {

            foreach (Bairro bairro in bairros)
            {
                for (int i = 0; i < numPlayer; i++)
                {
                    if (bairro.PlayerIDNoControl.Value == i)
                    {
                        _eleitoresPlayers[i]+= bairro.SetUpBairro.Eleitores.contaEleitores;
                    }
                }
            }
        }

        public ZonaTerritorial checaSePlayerTemTodosBairrosDeUmaZona(int client)
        {
            soma=0;
            foreach(Bairro bairro in bairros)
            {
                if(bairro.PlayerIDNoControl.Value ==client)
                {
                    soma++;
                }
            }
            if(soma==bairros.Length){
                return this;
            }else{
                return null;
            }
        }

        public int ContaRecursosEducacao(){
            soma2=0;
            foreach(Bairro bairro in bairros){
                   soma2 += bairro.checaNumerodeEducacao();
                }
                return soma2;
            }
            public int ContaRecursosSaude(){
            soma3=0;
            foreach(Bairro bairro in bairros){
                   soma3 += bairro.checaNumerodeSaude();
                }
                return soma3;
            }
    }
}



