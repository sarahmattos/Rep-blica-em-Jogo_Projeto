using UnityEngine;

namespace Game.Tools
{
    public class DevToolsHandler : Singleton<DevToolsHandler>
    {
        [SerializeField] private bool devToolActive = false;
        [SerializeField] private DevInputShortcut devInputShortcut;

        public bool DevToolActive => devToolActive;

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
            }

            if (!devToolActive) return;
            devInputShortcut.InputCalls();



        }








    }

}


