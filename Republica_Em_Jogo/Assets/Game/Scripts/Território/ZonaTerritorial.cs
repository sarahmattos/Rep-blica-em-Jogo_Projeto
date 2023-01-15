
using Game.Tools;
using UnityEngine;

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
        private void Awake()
        {
            bairros = GetComponentsInChildren<Bairro>();
             projeto = GameObject.FindObjectOfType<Projeto>();
        }

        public void verificarPlayerNasZonas(ulong client)
        {
            foreach(Bairro bairro in bairros)
            {
                 
                if(bairro.playerIDNoControl.Value == (int)client)
                {
                   
                    Debug.Log("bairroNome: "+bairro.Nome);
                    projeto.playerInZona=true;
                    //x++;
                    
                }
            }
        }
        public void adicionarEleitoresZona(int valor){
            foreach(Bairro bairro in bairros)
            {
                bairro.SetUpBairro.Eleitores?.MudaValorEleitores(valor);
            }

        }


    }


}
