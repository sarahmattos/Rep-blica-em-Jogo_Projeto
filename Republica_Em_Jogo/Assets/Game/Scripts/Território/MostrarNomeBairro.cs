using UnityEngine;
using Game.UI;

namespace Game.Territorio {
    public class MostrarNomeBairro : MonoBehaviour
{
    //[SerializeField] private Material highlightMaterial;
    [SerializeField] private GameObject nomeBairro;
    private Bairro bairro;
    private Projeto projeto;
    private HudStatsJogador hs;


    void Awake()
    {
        bairro = GetComponentInParent<Bairro>();
        projeto = FindObjectOfType<Projeto>();
        hs = FindObjectOfType<HudStatsJogador>();
    }


    void Start()
    {
        nomeBairro.SetActive(false);
        bairro.Interagivel.mouseEnter += () => { nomeBairro.SetActive(true); };
        bairro.Interagivel.mouseExit += () => { nomeBairro.SetActive(false); };
        bairro.Interagivel.click += EscolherBairroNoProjeto;
    }

    void OnDestroy()
    {
        bairro.Interagivel.mouseEnter -= () => { nomeBairro.SetActive(true); };
        bairro.Interagivel.mouseExit -= () => { nomeBairro.SetActive(false); };
        bairro.Interagivel.click -= EscolherBairroNoProjeto;
    }

    private void EscolherBairroNoProjeto()
    {
        if (hs.distribuicaoGeral == true)
        {
            if (projeto.distribuicaoProjeto == true)
            {
                if (bairro.bairroNaZonaEscolhida == true)
                {
                    Debug.Log("projeto distribuicao");
                    bairro.EscolherBairroEleitor();
                }
            }
            else
            {
                //fazer restricao
                bairro.EscolherBairroEleitor();
            }

        }
    }



}

}
