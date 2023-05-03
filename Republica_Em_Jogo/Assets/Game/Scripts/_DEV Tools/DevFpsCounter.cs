using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tools
{
    public class DevFpsCounter : MonoBehaviour
    {
        float fixedDt = 0f;
        float dt = 0.0f;
        float fps = 0f;

        void Update()
        {
            if (!DevToolsHandler.Instance.DevToolActive) return;

            TimeUpdate();

        }

        private void OnGUI()
        {
            if (!DevToolsHandler.Instance.DevToolActive) return;

            int w = Screen.width, h = Screen.height;
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 90);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 90;
            style.normal.textColor = new Color(1f, 1f, 1f, 1f);
            string text = string.Concat("FPS: ", fps);
            GUI.Label(rect, text, style);

        }




        void TimeUpdate()
        {
            fps = (float)System.Math.Round(1.0f / dt, 1);
            dt += (Time.deltaTime - dt) * 0.1f;
            fixedDt += (Time.fixedDeltaTime - fixedDt);
        }



    }
}
