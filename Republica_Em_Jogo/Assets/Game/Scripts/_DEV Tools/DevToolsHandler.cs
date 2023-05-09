using Game.Networking;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Tools
{
    public class DevToolsHandler : Singleton<DevToolsHandler>
    {
        [SerializeField] private KeyCode enableDisableToolsKey;

        [SerializeField] private bool devToolActive = false;
        public bool DevToolActive => devToolActive;
        [SerializeField] private DevInputShortcut devInputShortcut ;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            devInputShortcut = new DevInputShortcut();
        }


        private void Update()
        {
            if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(enableDisableToolsKey))
            {
                devToolActive = !devToolActive;
            }

            if (!devToolActive) return;
            devInputShortcut.InputCalls();



        }








    }

}


