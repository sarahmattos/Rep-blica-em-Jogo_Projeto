using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using UnityEngine.UI;
using TMPro;
using System;
using Game.Tools;

public class RecursosCartaManager : Singleton<RecursosCartaManager>
{
    private HudStatsJogador hs;
    [SerializeField] private GameObject semTroca;
    [SerializeField] private GameObject comTroca;
    // [SerializeField] private GameObject avisoDistribuicao;
    [SerializeField] private GameObject avisoDistribuicaoFinal;
    [SerializeField] private GameObject avisoRetiradaFinal;
    [SerializeField] private GameObject retirada;
    [SerializeField] private GameObject semcarta;
    [SerializeField] private Button eduTrocaBtn;
    [SerializeField] private Button eduTrocaBtn2;
    [SerializeField] private Button saudeTrocaBtn;
    [SerializeField] private Button saudeTrocaBtn2;
    [SerializeField] private TMP_Text text_edu;
    [SerializeField] private TMP_Text text_saude;
    // [SerializeField] private TMP_Text text_recursoDistribuicao;
    [SerializeField] private TMP_Text text_quantidaderetirada;
    public int novosEdu;
    public int novosSaude;
    public bool chamarDistribuicao = false;
    public bool chamarDistribuicao2 = false;
    private int quantidade;
    public bool comTrocaTrue = false;
    public event Action<string, int> IniciaDistribuicaoRecursos; //<recursotipo, quantidade>
    public event Action<string> DistribuiuRecurso;  //string com o tipo de recurso distribuido ("Saúde","Educação")

    //usei para saber quando acaba a distribuicao de recursos de saude, 
    //pois precisava saber para poder trocar o ícone de distribuicao do currency
    //ja que a distribuicao ocorrece automaticamente na sequencia: primeiro saude e depois educacao
    public event Action<string> FimDistricaoRecursos;

    void Start()
    {
        hs = FindObjectOfType<HudStatsJogador>();
    }
    public void Update()
    {
        if (chamarDistribuicao2 == true)
        {
            distribuiNovosRecursos();
        }
        if (quantidade == 1)
        {
            if (hs.saudeQuant < 1 && hs.eduQuant < 1)
            {
                semcarta.SetActive(true);
                retirada.SetActive(false);
                quantidade = 0;
            }
        }
    }

    public void RemoveNovosSaude()
    {
        novosSaude--;
        DistribuiuRecurso?.Invoke("Saúde");
        if (novosSaude == 0) FimDistricaoRecursos?.Invoke("Saúde");
    }

    public void RemoveNovosEdu()
    {
        novosEdu--;
        DistribuiuRecurso?.Invoke("Educação");
        if (novosEdu == 0) FimDistricaoRecursos?.Invoke("Educação");
    }



    //funcao que mexe na interface na hora da troca chamada pelo botao
    public void conferirQuantidade()
    {
        //interface na troca de carta por recurso
        text_edu.SetText("+" + novosEdu);
        text_saude.SetText("+" + novosSaude);
        if (hs.eduQuant >= 3 || hs.saudeQuant >= 3)
        {
            //interface troca
            comTroca.SetActive(true);
            comTrocaTrue = true;

            //deixar botao interagivel ou nao
            if (hs.eduQuant >= 3)
            {
                eduTrocaBtn.interactable = true;
            }
            else
            {
                eduTrocaBtn.interactable = false;
            }
            if (hs.saudeQuant >= 3)
            {
                saudeTrocaBtn.interactable = true;
            }
            else
            {
                saudeTrocaBtn.interactable = false;
            }
        }
        else
        {
            //interface sem troca
            semTroca.SetActive(true);
        }

    }
    public void panelFalse(GameObject panel)
    {
        panel.SetActive(false);

    }
    //chamada pelo botao de fechar e inicia a distribucao dps das trocas
    public void distribuicaoChamada()
    {
        // IniciaDistribuicaoRecursos?.Invoke(); // TODO: PRECISA SER CHAMADO AO ENTRAR E AO SAIR. PRESSA PASSAR O TIPO DE RECURSOS E A QUANTIDADE
        chamarDistribuicao = true;

    }
    public void distribuicaoChamada2()
    {
        StartCoroutine(EsperaChamarDistribuicai2(1f));

        if (novosSaude > 0)
        {
            IniciaDistribuicaoRecursos?.Invoke("Saúde", novosSaude);
            return;
        }
        if (novosEdu > 0)
        {
            IniciaDistribuicaoRecursos?.Invoke("Educação", novosEdu);
        }

    }

    private IEnumerator EsperaChamarDistribuicai2(float s)
    {
        yield return new WaitForSeconds(s);

        chamarDistribuicao2 = true;
    }
    //quando clicado no botao de troca
    public void trocarSaude()
    {
        hs.saudeQuant -= 3;//dimuinui carta
        novosSaude++;//ganha recurso
        text_saude.SetText("+" + novosSaude);//atualiza interface
        if (hs.saudeQuant < 3)
        {
            saudeTrocaBtn.interactable = false;
        }
        hs.atualizarRecursoAposTroca();
    }
    //quando clicado no botao de troca
    public void trocarEdu()
    {
        hs.eduQuant -= 3;
        novosEdu++;
        text_edu.SetText("+" + novosEdu);
        if (hs.eduQuant < 3)
        {
            eduTrocaBtn.interactable = false;
        }
        hs.atualizarRecursoAposTroca();
    }
    public void perderEdu()
    {
        hs.eduQuant -= 1;
        quantidade -= 1;
        text_quantidaderetirada.text = quantidade.ToString();
        if (hs.eduQuant < 1)
        {
            eduTrocaBtn2.interactable = false;
        }
        hs.atualizarRecursoAposTroca();
        if (quantidade <= 0)
        {
            avisoRetiradaFinal.SetActive(true);
            retirada.SetActive(false);
        }

    }
    public void verificacaoInicial(int penalidade)
    {
        hs.atualizarRecursoAntesTroca();
        if (hs.saudeQuant > 0)
        {
            saudeTrocaBtn2.interactable = true;
        }
        else
        {
            saudeTrocaBtn2.interactable = false;
        }
        if (hs.eduQuant > 0)
        {
            eduTrocaBtn2.interactable = true;
        }
        else
        {
            eduTrocaBtn2.interactable = false;
        }
        if (hs.eduQuant > 0 || hs.saudeQuant > 0)
        {
            //interface troca
            retirada.SetActive(true);
        }
        else
        {
            semcarta.SetActive(true);
        }

        quantidade = penalidade;
        text_quantidaderetirada.text = quantidade.ToString();

    }
    public void perderSaude()
    {
        hs.saudeQuant -= 1;//dimuinui carta
        quantidade -= 1;
        text_quantidaderetirada.text = quantidade.ToString();
        if (hs.saudeQuant < 1)
        {
            saudeTrocaBtn2.interactable = false;
        }
        hs.atualizarRecursoAposTroca();
        if (quantidade <= 0)
        {
            avisoRetiradaFinal.SetActive(true);
            retirada.SetActive(false);
        }

    }

    //distribui esses novos recursos (interface)
    public void distribuiNovosRecursos()
    {
        if (novosEdu > 0 || novosSaude > 0)
        {
            // avisoDistribuicao.SetActive(true);
            // if(novosSaude>0)text_recursoDistribuicao.SetText("Distribua seu(s) recurso(s) de saúde! Clique no bairro que deseja adicionar!"+ "\n" +" Saúde: "+novosSaude);
            // if(novosSaude<=0)text_recursoDistribuicao.SetText("Distribua seu(s) recurso(s) de educação! Clique no bairro que deseja adicionar!"+ "\n" +" Educação: "+novosEdu );
        }
        else
        {
            // avisoDistribuicao.SetActive(false);
            avisoDistribuicaoFinal.SetActive(true);
            chamarDistribuicao = false;
            chamarDistribuicao2 = false;
        }
    }
}
