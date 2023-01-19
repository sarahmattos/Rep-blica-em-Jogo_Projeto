using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using UnityEngine.UI;
using TMPro;

public class RecursosCartaManager : MonoBehaviour
{
    private HudStatsJogador hs;
    [SerializeField] private GameObject semTroca;
    [SerializeField] private GameObject comTroca;
    [SerializeField] private GameObject avisoDistribuicao;
    [SerializeField] private GameObject avisoDistribuicaoFinal;
    [SerializeField] private Button eduTrocaBtn;
    [SerializeField] private Button saudeTrocaBtn;
    [SerializeField] private TMP_Text text_edu;
    [SerializeField] private TMP_Text text_saude;
    [SerializeField] private TMP_Text text_recursoDistribuicao;
    public int novosEdu;
    public int novosSaude;
    public bool chamarDistribuicao=false;
    void Start()
    {
        hs = FindObjectOfType<HudStatsJogador>();
    }
    public void Update(){
        if(chamarDistribuicao==true){
            distribuiNovosRecursos();
        }
    }

    //funcao que mexe na interface na hora da troca chamada pelo botao
   public void conferirQuantidade(){
        //interface na troca de carta por recurso
        text_edu.SetText("+"+novosEdu);
        text_saude.SetText("+"+novosSaude);

        if(hs.eduQuant>=3 || hs.saudeQuant>=3){
            //interface troca
            comTroca.SetActive(true);

            //deixar botao interagivel ou nao
            if(hs.eduQuant>=3){
                eduTrocaBtn.interactable = true;
            }else{
                eduTrocaBtn.interactable = false;
            }
            if(hs.saudeQuant>=3){
                saudeTrocaBtn.interactable = true;
            }else{
                saudeTrocaBtn.interactable = false;
            }
        }else{
            //interface sem troca
            semTroca.SetActive(true);
        }
    
   }
   public void panelFalse(GameObject panel){
        panel.SetActive(false);
       
   }
   //chamada pelo botao de fechar e inicia a distribucao dps das trocas
   public void distribuicaoChamada(){
         chamarDistribuicao=true;
         
   }
    //quando clicado no botao de troca
   public void trocarSaude(){
        hs.saudeQuant-=3;//dimuinui carta
        novosSaude++;//ganha recurso
        text_saude.SetText("+"+novosSaude);//atualiza interface
        if(hs.saudeQuant<3){
            saudeTrocaBtn.interactable = false;
        }
        hs.atualizarRecursoAposTroca();
    }
    //quando clicado no botao de troca
    public void trocarEdu(){
        hs.eduQuant-=3;
        novosEdu++;
        text_edu.SetText("+"+novosEdu);
        if(hs.eduQuant<3){
            eduTrocaBtn.interactable = false;
        }
        hs.atualizarRecursoAposTroca();
    }

    //distribui esses novos recursos (interface)
   public void distribuiNovosRecursos(){
    if(novosEdu>0||novosSaude>0){
        avisoDistribuicao.SetActive(true);
        text_recursoDistribuicao.SetText("Distribuia seus recursos! Clique no recurso do bairro que deseja adicionar!"+ "\n" +" Educação: "+novosEdu +" Saúde: "+novosSaude);
    }else{
        avisoDistribuicao.SetActive(false);
        avisoDistribuicaoFinal.SetActive(true);
        chamarDistribuicao=false;
    }
   }
}
