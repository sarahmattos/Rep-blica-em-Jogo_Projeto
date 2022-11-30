using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using Logger = Game.Tools.Logger;

public class GameDataconfig : Singleton<GameDataconfig> 
{
    [SerializeField] private int maxConnections;
    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameSceneName;
    [SerializeField] private List<Color> playerColorOrder;
    public int territoriosInScene = 14;
    public int MaxConnections => maxConnections;
    public string GameSceneName => gameSceneName;
    public string MenuSceneName => menuSceneName;
    public List<Color> PlayerColorOrder  => playerColorOrder;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
            Logger.Instance.gameObject.SetActive(!Logger.Instance.gameObject.activeSelf);
        }
    }
}
