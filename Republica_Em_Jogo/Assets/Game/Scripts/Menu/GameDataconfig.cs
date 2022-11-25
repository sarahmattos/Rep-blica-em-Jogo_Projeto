using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class GameDataconfig : Singleton<GameDataconfig> 
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string gameSceneName;
    public List<Color> playerColorOrder;
    //public List<Color> playerColorOrder;
    //FF4242
    //7BFF50
    //FF00EB
    //FFEB00

    public int territoriosInScene=14;
    public int MaxConnections => maxConnections; 
    public string GameSceneName  => gameSceneName;
    public List<Color> PlayerColorOrder  => playerColorOrder;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
