using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Tools
{
    public class DevTools : Singleton<DevTools>
    {

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Logger.Instance.gameObject.SetActive(!Logger.Instance.gameObject.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.F1))
                
            {
                NetworkManager.Singleton.Shutdown();
                SceneManager.LoadScene(0);
                
            }



        }
    }
}

