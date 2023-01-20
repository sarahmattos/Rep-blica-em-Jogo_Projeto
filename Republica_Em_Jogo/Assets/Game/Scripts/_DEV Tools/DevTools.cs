using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Tools
{
    public class DevTools : Singleton<DevTools>
    {

        float fixedDt = 0f;
        float dt = 0.0f;
        float fps = 0f;
        public bool showGUI = false;

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
            if (Input.GetKeyDown(KeyCode.Backspace)) Logger.Instance.ResetLoggger();

            TimeUpdate();

        }


        void OnGUI()
        {

            int w = Screen.width, h = Screen.height;
            GUIStyle style = new GUIStyle();
            if (showGUI)
            {
                Rect rect = new Rect(0, 0, w, h * 2 / 90);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = h * 2 / 90;
                style.normal.textColor = new Color(1f, 1f, 1f, 1f);
                string text = string.Concat("FPS: ", fps);
                GUI.Label(rect, text, style);

            }

        }

        void TimeUpdate()
        {
            fps = (float)System.Math.Round(1.0f / dt, 1);
            dt += (Time.deltaTime - dt) * 0.1f;
            fixedDt += (Time.fixedDeltaTime - fixedDt);
        }



    }

}


