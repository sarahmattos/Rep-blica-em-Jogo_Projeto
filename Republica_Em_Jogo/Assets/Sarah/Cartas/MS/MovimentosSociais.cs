using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;
using Game.Territorio;
using Game.Player;

public class MovimentosSociais : NetworkBehaviour
{
    private NetworkVariable<FixedString4096Bytes> movimentoSocialNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
    private NetworkVariable<int> idPlayerMS = new NetworkVariable<int>(-1);
    public MovimentoSociaisObject MovimentoSociaisManager;
    private string movimento;
    private string recursoTipo;
    private int quantidadeRecurso;
    private int quantidadeEleitor;
    [SerializeField] private TMP_Text text_msCarta;
    [SerializeField] private TMP_Text text_aviso;
    [SerializeField] private GameObject cartaMV;
    [SerializeField] private GameObject btnFechar;
    [SerializeField] private GameObject btnOk;
    private HudStatsJogador hs;
    private RecursosCartaManager rc;
    private Baralho baralho;
    public bool distribuicaoRecompensaRecurso = false;

    public void Start()
    {
        hs = FindObjectOfType<HudStatsJogador>();
        rc = FindObjectOfType<RecursosCartaManager>();
        baralho = FindObjectOfType<Baralho>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AtualizaTextoServerRpc(string textoTotal2, int id)
    {
        movimentoSocialNetworkTexto.Value = textoTotal2;
        idPlayerMS.Value = id;
    }

    public void sortearMS()
    {
        HabilitarBairrosPlayerAtual(true);
        int aux = Random.Range(0, MovimentoSociaisManager.movimento.Length);
        movimento = MovimentoSociaisManager.movimento[aux];
        recursoTipo = MovimentoSociaisManager.recursoTipo[aux];
        quantidadeRecurso = MovimentoSociaisManager.quantidadeRecurso;
        quantidadeEleitor = MovimentoSociaisManager.quantidadeEleitor;
        if (recursoTipo == "Educação") rc.novosEdu = quantidadeRecurso;
        if (recursoTipo == "Saúde") rc.novosSaude = quantidadeRecurso;
        string textoTotal = "\n" + movimento + "\n" + "\n" + "Ganhe " + quantidadeRecurso + " recurso de " + recursoTipo.ToString() + " e " + quantidadeEleitor + " eleitores";
        int id = (int)NetworkManager.Singleton.LocalClientId;
        AtualizaTextoServerRpc(textoTotal, id);
        baralho.baralhoManager(false);

    }
    private void OnEnable()
    {
        movimentoSocialNetworkTexto.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                if (newValue != "")
                {
                    cartaMV.SetActive(true);
                    text_msCarta.text = newValue.ToString();

                }

            };
        idPlayerMS.OnValueChanged += (int previousValue, int newValue) =>
        {
            if (newValue != (int)NetworkManager.Singleton.LocalClientId)
            {
                btnOk.SetActive(false);
                btnFechar.SetActive(true);
                text_aviso.text = "Movimento Social retirado pelo jogador: " + newValue;
            }
            else
            {
                text_aviso.text = " ";
                btnOk.SetActive(true);
                btnFechar.SetActive(false);
            }
        };
    }
    public void chamarRecompensasEleitor()
    {
        distribuicaoRecompensaRecurso = true;
        //receber valor uma vez
        hs.playerRecebeEleitor = true;
        //chama funcao pra receber esse valor
        hs.ValorEleitoresNovos(quantidadeEleitor);
    }
    public void chamarRecompensasRecurso()
    {
        if (distribuicaoRecompensaRecurso == true)
        {
            rc.distribuicaoChamada();
            distribuicaoRecompensaRecurso = false;
        }

    }

    public void panelFalse(GameObject panel)
    {
        panel.SetActive(false);
    }

    private void HabilitarBairrosPlayerAtual(bool value)
    {
        List<Bairro> bairros = PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;
        foreach (Bairro bairro in bairros)
        {
            bairro.Interagivel.MudarHabilitado(value);
        }
    }
}
