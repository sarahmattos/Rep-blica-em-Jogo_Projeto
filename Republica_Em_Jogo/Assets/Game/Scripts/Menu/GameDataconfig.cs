using Game.Player;
using Game.Tools;
using System.Collections.Generic;
using UnityEngine;

public class GameDataconfig : Singleton<GameDataconfig>
{

    [SerializeField] private int maxConnections;
    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private List<Color> playerColorOrder;
    [SerializeField] private string tagParticipante;
    [SerializeField] private int maxRodadasParaEleicoes;
    [SerializeField] private int cadeirasTotal = 12;
    [SerializeField] private List<Color> zonaColorOutline;
    [SerializeField] private DevConfig devConfig;
    public DevConfig DevConfig => devConfig;

    public int MaxConnections => maxConnections;
    public List<Color> PlayerColorOrder => playerColorOrder;
    public List<Color> ZonaColorOutline => zonaColorOutline;
    public string TagParticipante => tagParticipante;
    public int MaxRodadasParaEleicoes => maxRodadasParaEleicoes;
    public int CadeirasTotal => cadeirasTotal;

    public string MenuSceneName => menuSceneName;
    public string GameplaySceneName => gameplaySceneName;

    public string TagPlayerColorizada(PlayerStats playerStats)
    {
        string playerHexColor = ColorUtility.ToHtmlStringRGB((GameDataconfig.Instance.PlayerColorOrder[playerStats.playerID]));
        return string.Format("<color=#{0}>{1}</color>", playerHexColor, playerStats.PlayerName);
    }

    public string TagPlayerAtualColorizada()
    {
        PlayerStats playerAtual = PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual();

        return TagPlayerColorizada(playerAtual);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("maxConnections")) maxConnections = PlayerPrefs.GetInt("maxConnections");

        DontDestroyOnLoad(gameObject);
    }

    public void SetMaxConnections(int value)
    {
        Debug.Log("max connections updaate:" + value);
        maxConnections = value;
        PlayerPrefs.SetInt("maxConnections", value);
    }

}


[System.Serializable]
public class DevConfig
{

    [SerializeField] private bool venceConquistandoTudo;
    [SerializeField] private bool mostraUISyncCarregamentoPlayers;
    [SerializeField] private bool onlineUiButtonAtivos;
    [SerializeField] private bool mostrarLog;
    [SerializeField] private bool mostarOpcao1player;

    public bool VenceConquistandoTudo => venceConquistandoTudo;

    public bool MostraUISyncCarregamentoPlayers => mostraUISyncCarregamentoPlayers;
    public bool OnlineUiButtonAtivos => onlineUiButtonAtivos;
    public bool MostrarLog => mostrarLog;

    public bool MostarOpcao1player => mostarOpcao1player;
}
