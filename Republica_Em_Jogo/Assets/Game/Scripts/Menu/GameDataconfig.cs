using Game.Tools;
using System.Collections.Generic;
using UnityEngine;

public class GameDataconfig : Singleton<GameDataconfig> 
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameSceneName;
    [SerializeField] private List<Color> playerColorOrder;
    [SerializeField] private string tagParticipante;
    [SerializeField] private int maxRodadasParaEleicoes;
    public int territoriosInScene;
    public int MaxConnections => maxConnections;
    public string GameSceneName => gameSceneName;
    public string MenuSceneName => menuSceneName;
    public List<Color> PlayerColorOrder  => playerColorOrder;

    public string TagParticipante => tagParticipante;

    public int MaxRodadasParaEleicoes => maxRodadasParaEleicoes; 

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
