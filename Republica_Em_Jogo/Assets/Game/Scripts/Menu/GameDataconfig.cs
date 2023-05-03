using Game;
using Game.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataconfig : Singleton<GameDataconfig>
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private List<Color> playerColorOrder;
    [SerializeField] private string tagParticipante;
    [SerializeField] private int maxRodadasParaEleicoes;
    [SerializeField] private List<Color> zonaColorOutline;

    public int territoriosInScene;
    public int MaxConnections => maxConnections;
    public List<Color> PlayerColorOrder => playerColorOrder;
    public List<Color> ZonaColorOutline => zonaColorOutline;
    public string TagParticipante => tagParticipante;
    public int MaxRodadasParaEleicoes => maxRodadasParaEleicoes;

    public string MenuSceneName => menuSceneName;
    public string GameplaySceneName => gameplaySceneName;

    public string TagPlayerColorizada(int playerID)
    {
        string playerHexColor = ColorUtility.ToHtmlStringRGB(
            (GameDataconfig.Instance.PlayerColorOrder[playerID]));

        return string.Format("<color=#{0}>{1}</color>",
            playerHexColor,
            string.Concat(GameDataconfig.Instance.TagParticipante, " ", playerID));
    }

    public string TagPlayerAtualColorizada()
    {
        int playerAtualID = TurnManager.Instance.PlayerAtual;
        return TagPlayerColorizada(playerAtualID);
    }

    private void Start()
    {

        DontDestroyOnLoad(gameObject);
    }

}
