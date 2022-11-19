using Game.Tools;
using TMPro;
using UnityEngine;

namespace Game.Territorio
{
    public class Bairro : MonoBehaviour
    {
        [SerializeField] private DadoBairro dados;
        //[SerializeField] private Jogador jogadorConquistando;
        [SerializeField] private Material material;
        [SerializeField] private TMP_Text text_nome;
        [SerializeField] private Eleitores eleitores;

        public DadoBairro Dados=> dados;

        private void Awake()
        {
            text_nome = GetComponentInChildren<TMP_Text>();
            material = GetComponentInChildren<MeshRenderer>().material;
            eleitores = GetComponentInChildren<Eleitores>();
        }

        private void Start()
        {
            text_nome.SetText(dados.Nome);
            material.MudarCor(Color.gray);

        }

        //MOVI PARA DadosBairro
        //public int id;
        //public string nome;
        ////public ZonaTerritorial zonaTerritorial; // Faz sentido a zona que ter os bairros, não o inverso
        //public Vector2 posicao;
        //public Eleitores eleitor;
        //public int qntEleitor;
        //public int recurso;


    }

}
