using Game.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameDataconfig : Singleton<GameDataconfig> 
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameSceneName;
    [SerializeField] private List<Color> playerColorOrder;
    [SerializeField] private string tagParticipante;
    [SerializeField] private int maxRodadasParaEleicoes;
    [SerializeField] private SceneAsset menuScene;
    [SerializeField] private SceneAsset gameplayScene;

    public int territoriosInScene;
    public int MaxConnections => maxConnections;


    public List<Color> PlayerColorOrder  => playerColorOrder;

    public string TagParticipante => tagParticipante;

    public int MaxRodadasParaEleicoes => maxRodadasParaEleicoes;

    public SceneAsset MenuScene => menuScene;

    public SceneAsset GameplayScene => gameplayScene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
