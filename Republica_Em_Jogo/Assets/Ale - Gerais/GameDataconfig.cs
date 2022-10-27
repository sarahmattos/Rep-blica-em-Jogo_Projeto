using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class GameDataconfig : Singleton<GameDataconfig> 
{
    public int maxConnections;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {

            NetworkDriver networkDriver = new NetworkDriver();
            NetworkConnection nc = new NetworkConnection();
            nc.Disconnect(networkDriver);        }

    }


}
