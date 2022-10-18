using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    public GameObject clientPrefab;
    public GameObject serverPrefab;
    public void OnClientClick()
    {
        DontDestroyOnLoad(Instantiate(clientPrefab).gameObject);
        SceneManager.LoadScene("teste");
    }
    public void OnServerClick()
    {
        DontDestroyOnLoad(Instantiate(serverPrefab).gameObject);
        SceneManager.LoadScene("teste");
    }
    public void OnClientServerClick()
    {
        OnServerClick();
        OnClientClick();
        SceneManager.LoadScene("teste");
    }
}
