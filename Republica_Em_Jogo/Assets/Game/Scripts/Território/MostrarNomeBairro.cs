using UnityEngine;
using Game.UI;
using TMPro;

namespace Game.Territorio
{
    public class MostrarNomeBairro : MonoBehaviour
    {
        //[SerializeField] private Material highlightMaterial;
        [SerializeField] private GameObject nomeBairro;
        private TMP_Text textNomeBairro;
        private Bairro bairro;
        private Projeto projeto;
        private HudStatsJogador hs;
        private RecursosCartaManager rc;
        void Awake()
        {
            bairro = GetComponentInParent<Bairro>();
            textNomeBairro = nomeBairro.gameObject.GetComponent<TMP_Text>();
            projeto = FindObjectOfType<Projeto>();
            hs = FindObjectOfType<HudStatsJogador>();

        }


        void Start()
        {
            nomeBairro.SetActive(false);
            textNomeBairro.SetText(bairro.Nome);
            bairro.Interagivel.Click += EscolherBairroNoProjeto;
            rc = FindObjectOfType<RecursosCartaManager>();
        }




        void OnDestroy()
        {
            bairro.Interagivel.Click -= EscolherBairroNoProjeto;
        }

        private void OnMouseEnter()
        {
            nomeBairro.SetActive(true);
        }

        private void OnMouseExit()
        {
            nomeBairro.SetActive(false);
        }

        private void HabilitandoNomeObj(bool value)
        {
            nomeBairro.SetActive(value);
        }


        //TODO: Realocar este metodo; MostarNomeBairro deveria estar encarregado de somente "mostrar o nome do bairro"
        private void EscolherBairroNoProjeto(Bairro _)
        {
            if(!rc.chamarDistribuicao2){
                if (hs.distribuicaoGeral == true)
                {
                    if (projeto.distribuicaoProjeto == true)
                    {
                        if (bairro.bairroNaZonaEscolhida == true)
                        {
                            bairro.EscolherBairroEleitor();
                        }
                    }
                    else
                    {
                        //fazer restricao
                            bairro.EscolherBairroEleitor();
                        
                        
                    }
                    
                }
            }else{
                if(rc.novosSaude>0){
                    bairro.saude.adicionarSaude();
                }else{
                    bairro.edu.adicionarEdu();
                }
                   
            }
             
        }



    }

}
