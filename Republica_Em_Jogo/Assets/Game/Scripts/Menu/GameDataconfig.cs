using Game.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataConfig : Singleton<GameDataConfig> 
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private List<Color> playerColorOrder;
    [SerializeField] private string tagParticipante;
    [SerializeField] private int maxRodadasParaEleicoes;


    public int territoriosInScene;
    public int MaxConnections => maxConnections;
    public List<Color> PlayerColorOrder  => playerColorOrder;
    public string TagParticipante => tagParticipante;
    public int MaxRodadasParaEleicoes => maxRodadasParaEleicoes;

    public string MenuSceneName  => menuSceneName;
    public string GameplaySceneName  => gameplaySceneName; 

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
