using Game.Networking;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Tools
{
    public class DevToolsHandler : Singleton<DevToolsHandler>
    {
        [SerializeField] private bool devToolActive = false;
        public bool DevToolActive => devToolActive;


        [SerializeField] private DevInputShortcut devInputShortcut;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            devInputShortcut = new DevInputShortcut();
        }


        private void Update()
        {
            if (Input.GetKey(devInputShortcut.DefaultInput) && Input.GetKeyDown(devInputShortcut.EnableDisableToolsKey))
            {
                devToolActive = !devToolActive;
                Debug.Log(devToolActive.ToString());
            }

            if (!devToolActive) return;
            devInputShortcut.InputCalls();



        }








    }

}


