
using Game.Tools;
using UnityEngine;

namespace Game.Territorio
{
    public class ZonaTerritorial : MonoBehaviour
    {
        [SerializeField] private string nome;
        private Bairro[] bairros;
        int x;
        public bool playerInZona=false;

        public Bairro[] Bairros => bairros;
        public string Nome { get => nome; }

        private void Awake()
        {
            bairros = GetComponentsInChildren<Bairro>();
        }

        public void verificarPlayerNasZonas(ulong client)
        {
            foreach(Bairro bairro in bairros)
            {
                 Debug.Log("cliente: "+(int)client);
                 Debug.Log("playerIDNoControl: "+bairro.playerIDNoControl.Value);
                 
                if(bairro.playerIDNoControl.Value == (int)client)
                {
                   
                    Debug.Log("bairroNome: "+bairro.Nome);
                    //x++;
                    playerInZona=true;
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
