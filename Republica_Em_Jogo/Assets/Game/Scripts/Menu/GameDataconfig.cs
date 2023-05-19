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

    private int territoriosTotal;
    public int TerritoriosTotal => territoriosTotal;
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
        DontDestroyOnLoad(gameObject);
    }


}


[System.Serializable]
public class DevConfig
{

    [SerializeField] private bool venceConquistandoTudo;
    [SerializeField] private bool mostraUISyncCarregamentoPlayers;
    [SerializeField] private bool onlineUiButtonAtivos;

    public bool VenceConquistandoTudo => venceConquistandoTudo;

    public bool MostraUISyncCarregamentoPlayers => mostraUISyncCarregamentoPlayers;
    public bool OnlineUiButtonAtivos => onlineUiButtonAtivos;
}
