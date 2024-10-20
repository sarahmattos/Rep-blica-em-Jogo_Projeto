using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player;

namespace Game.UI
{
    public class UIAvancoState : MonoBehaviour
    {
        [SerializeField] private TMP_Text text_dadosVizinhos;
        [SerializeField] private TMP_Text text_dadosPlayerAtual;
        [SerializeField] private AvancoState avancoState;
        [SerializeField] private Image[] dadosImageSeus;
        [SerializeField] private Image[] dadosImageVizinhos;
        [SerializeField] private Sprite[] dadosSprite;
        DadosUiGeral dadosUiGeral;
        public AvancoData AvancoData => avancoState.AvancoData;
        public PlayerStats PlayerStatsLocal => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual();

        private void Start()
        {
            AvancoData.dadosLancados += UpdateTextDados;
            dadosUiGeral=FindObjectOfType<DadosUiGeral>();
        }
        private void OnDestroy()
        {
            AvancoData.dadosLancados -= UpdateTextDados;

        }

        private void UpdateTextDados() {
            
            //string dadosPlayerAtual = "Ataque: \n";
            //string dadosVizinhos = "Defesa: \n";
             string dadosPlayerAtual="";
             string dadosVizinhos="";
            
            foreach(int value in AvancoData.DadosPlayerAtual)
                dadosPlayerAtual = string.Concat(dadosPlayerAtual, value ,";");
            foreach(int value in AvancoData.DadosVizinhos)
                dadosVizinhos = string.Concat(dadosVizinhos, value ,";");
            
            //text_dadosPlayerAtual.SetText(dadosPlayerAtual);
            //text_dadosVizinhos.SetText(dadosVizinhos);  
            dadosUiGeral.reseta2UiDadosServerRpc();
            dadosUiGeral.atualizaUiDadosServerRpc(dadosPlayerAtual,dadosVizinhos);

        }
        public void desativaDados(){
            foreach(Image img in dadosImageSeus){
                img.gameObject.SetActive(false);
            }
            foreach(Image img in dadosImageVizinhos){
                img.gameObject.SetActive(false);
            }
        }
        public void UpdateTextDados2(string _p,string _v, Color corVizinho){

            desativaDados();

            string[] valoresSeparados = _p.Split(';');
            string[] valoresSeparados2 = _v.Split(';');

            
                for(int i=0; i<valoresSeparados.Length-1;i++){
                    dadosImageSeus[i].sprite = dadosSprite[int.Parse(valoresSeparados[i])];
                    dadosImageSeus[i].gameObject.SetActive(true);
                    dadosImageSeus[i].color = PlayerStatsLocal.Cor;
                    //Debug.Log(valoresSeparados[i]);
                }
            
                for(int i=0; i<valoresSeparados2.Length-1;i++){
                    dadosImageVizinhos[i].sprite =  dadosSprite[int.Parse(valoresSeparados2[i])];
                    dadosImageVizinhos[i].gameObject.SetActive(true);
                    dadosImageVizinhos[i].color =corVizinho;
                    //Debug.Log(valoresSeparados2[i]);
                            
                    
                }
                 
            
            
            
            text_dadosPlayerAtual.SetText("Ataque:");
            text_dadosVizinhos.SetText("Defesa:");   
            
        }

    }
}
