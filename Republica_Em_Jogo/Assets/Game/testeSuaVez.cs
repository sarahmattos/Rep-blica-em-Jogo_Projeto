using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Game.managers;

public class testeSuaVez : NetworkBehaviour
{

    public GameObject button;

    public override void OnNetworkSpawn()
    {
        TurnManager.Instance.isLocalPlayerTurn += (bool value) => { button.SetActive(value); };

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        TurnManager.Instance.isLocalPlayerTurn -= (bool value) => { button.SetActive(value); };
    }

    private void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(() => { TurnManager.Instance.NextTurnServerRpc(); });
        if(TurnManager.Instance.IsCurrent)
        {
            button.SetActive(true);
        }
    }

}
