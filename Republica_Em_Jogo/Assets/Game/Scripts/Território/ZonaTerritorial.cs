
using Game.Tools;
using UnityEngine;
 using Game.UI;

namespace Game.Territorio
{
    public class ZonaTerritorial : MonoBehaviour
    {
        [SerializeField] private string nome;
        private Bairro[] bairros;
        int x;

        public Bairro[] Bairros => bairros;
        public string Nome { get => nome; }
        private Projeto projeto;  
        private HudStatsJogador hs;
        private ControlePassarState cp;
         
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
                if(bairro.playerIDNoControl.Value == (int)client)
                {
                    Debug.Log("bairroNome: "+bairro.Nome);
                    projeto.playerInZona=true;
                }
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

        }


    }



