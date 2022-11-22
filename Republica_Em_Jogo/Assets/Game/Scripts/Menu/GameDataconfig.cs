using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class GameDataconfig : Singleton<GameDataconfig> 
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string gameSceneName;
    [SerializeField] private List<Color> playerColorOrder;

    public int MaxConnections => maxConnections; 
    public string GameSceneName  => gameSceneName;
    public List<Color> PlayerColorOrder  => playerColorOrder;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
