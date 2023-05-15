using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;
using Game.Player;
using Game.Territorio;

public class Corrupcao : NetworkBehaviour
{
    private NetworkVariable<FixedString4096Bytes> corrupcaoNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
    private NetworkVariable<int> idPlayerCorrupcao = new NetworkVariable<int>(-1);
    public CorrupcaoObject CorrupcaoManager;
    private string corrupcao;
    private int penalidade;
    private int penalidade2;
    private string complementText;
    [SerializeField] private TMP_Text text_CorrupcaoCarta;
    [SerializeField] private GameObject cartaCorrupcao;
    [SerializeField] private TMP_Text text_aviso;
    [SerializeField] private GameObject btnFechar;
    [SerializeField] private GameObject btnOk;
    private HudStatsJogador hs;
    private RecursosCartaManager rc;
    private Baralho baralho;

    public void Start()
    {
        hs = FindObjectOfType<HudStatsJogador>();
        rc = FindObjectOfType<RecursosCartaManager>();
        baralho = FindObjectOfType<Baralho>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AtualizaTextoServerRpc(string textoTotal2, int id)
    {
        corrupcaoNetworkTexto.Value = textoTotal2;
        idPlayerCorrupcao.Value = id;
    }

    public void sortearCorrupcao()
    {
        //defaultValues();
        AtualizaTextoServerRpc("", -1);
        HabilitarBairrosPlayerAtual(true);
        corrupcao = CorrupcaoManager.corrupcao[Random.Range(0, CorrupcaoManager.corrupcao.Length)];
        complementText = CorrupcaoManager.complementText;
        penalidade = CorrupcaoManager.penalidade;
        penalidade2 = penalidade;
        string textoTotal = "\n" + corrupcao + "\n" + "\n" + complementText;
        int id = (int)NetworkManager.Singleton.LocalClientId;
        AtualizaTextoServerRpc(textoTotal, id);
        // baralho.baralhoManager(false);

    }
    private void OnEnable()
    {
        corrupcaoNetworkTexto.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                if (newValue != "")
                {
                    cartaCorrupcao.SetActive(true);
                    text_CorrupcaoCarta.text = newValue.ToString();
                }
            };
        idPlayerCorrupcao.OnValueChanged += (int previousValue, int newValue) =>
             {
                if (newValue != -1){
                    if (newValue != (int)NetworkManager.Singleton.LocalClientId)
                    {
                        btnOk.SetActive(false);
                        btnFechar.SetActive(true);
                        text_aviso.text = "Corrupção retirado pelo jogador: " + newValue;
                    }
                    else
                    {
                        text_aviso.text = " ";
                        btnOk.SetActive(true);
                        btnFechar.SetActive(false);
                    }
                }
             };
    }
    public void chamarPenalidade()
    {
       StartCoroutine(EsperaEVai1(0.1f));
    }
    public void chamarPenalidade2()
    {
        rc.verificacaoInicial(penalidade2);
    }
    private IEnumerator EsperaEVai1(float s)
        {
            yield return new WaitForSeconds(s);
             hs.playerDiminuiEleitor = true;
            PlayerStats ps = hs.GetPlayerStats();
            if(ps.EleitoresTotais>ps.BairrosInControl.Count){
            hs.playerRecebeEleitor = true;
            hs.ValorEleitoresNovos(penalidade);
            }else{
            hs.text_naotemeleitorpraretirar.text="Não possui eleitores suficientes para retirada!";
            hs.AtualizaUIAposDistribuicao();
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
