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
    [SerializeField] private Button eduTrocaBtn;
    [SerializeField] private Button saudeTrocaBtn;
    void Start()
    {
        hs = FindObjectOfType<HudStatsJogador>();
    }

    // Update is called once per frame
   public void conferirQuantidade(){
    if(hs.eduQuant>=3 || hs.saudeQuant>=3){
        comTroca.SetActive(true);
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
        semTroca.SetActive(true);
    }
    
   }
   public void panelFalse(GameObject panel){
        panel.SetActive(false);
   }

   public void trocarSaude(){
        hs.saudeQuant-=3;
        if(hs.saudeQuant<3){
         saudeTrocaBtn.interactable = false;
    }
   }
   public void trocarEdu(){
    hs.eduQuant-=3;
    if(hs.eduQuant<3){
         eduTrocaBtn.interactable = false;
    }
     
   }
}
