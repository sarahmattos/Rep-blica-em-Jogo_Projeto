using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using Game.Territorio;
 using Game.UI;

public class MostrarNomeBairro : MonoBehaviour
{
    private Renderer renderer;
    //[SerializeField] private Material highlightMaterial;
    [SerializeField] private GameObject nomeBairro;
    private Bairro bairro;
    private Projeto projeto;
    private HudStatsJogador hs;
    //[SerializeField] private Material defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        bairro = GetComponentInParent<Bairro>();
        projeto = FindObjectOfType<Projeto>();
        hs = FindObjectOfType<HudStatsJogador>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        //renderer.material = highlightMaterial;
        nomeBairro.SetActive(true);
    }
    private void OnMouseExit()
    {
        //renderer.material = defaultMaterial;
        nomeBairro.SetActive(false);
    }
    private void OnMouseDown()
        {
            if(hs.distribuicaoGeral==true){
            if(projeto.distribuicaoProjeto==true){
                if(bairro.bairroNaZonaEscolhida==true){
                    Debug.Log("projeto distribuicao");
                bairro.EscolherBairroEleitor();
                }
            }else{
                //fazer restricao
                
                    bairro.EscolherBairroEleitor();
                }
                
            }
            
            
            
        }
}
