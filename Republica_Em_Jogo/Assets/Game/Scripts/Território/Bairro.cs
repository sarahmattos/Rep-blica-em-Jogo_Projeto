using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game.Territorio
{
    public class Bairro : NetworkBehaviour
    {
        [SerializeField] private DadoBairro dados;
        private Material material;
        private TMP_Text text_nome;
         
        public DadoBairro Dados => dados;

        public event Action playerControlMuda;


        private void Awake()
        {
            text_nome = GetComponentInChildren<TMP_Text>();
            material = GetComponentInChildren<MeshRenderer>().material;
            material.color = Color.gray;
            
        }

        private void OnEnable()
        {
            dados.playerIDNoControl.OnValueChanged += onPlayerControlMuda;
        }
        private void OnDisable()
        {
            dados.playerIDNoControl.OnValueChanged -= onPlayerControlMuda;
        }

        public void SetPlayerControl(int playerID)
        {
            dados.playerIDNoControl.Value = playerID;
        }
        

        private void onPlayerControlMuda(int previousValue, int newValue)
        {
            material.color = GameDataconfig.Instance.PlayerColorOrder[newValue];
        }

        private void Start()
        {
            text_nome.SetText(dados.Nome);

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
