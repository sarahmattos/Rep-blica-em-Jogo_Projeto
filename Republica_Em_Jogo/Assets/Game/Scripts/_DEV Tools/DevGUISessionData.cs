using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Game.Tools
{
    public class DevGUISessionData : MonoBehaviour
    {

        [SerializeField] private RodadaController rodadaController;

        void OnGUI()
        {
            if (!DevToolsHandler.Instance.DevToolActive) return;


            int w = Screen.width, h = Screen.height;
            GUIStyle guiStyle = new GUIStyle();

            Rect rect = new Rect(0, h/2, w/2, h * 2 / 90);

            guiStyle.alignment = TextAnchor.MiddleLeft;
            guiStyle.fontSize = h * 2 / 90;
            guiStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);

            GUI.Label(rect, string.Concat("rodada: ", rodadaController.Rodada), guiStyle);

            //GameState
            //CoreLoopstate
            //ordem dos jogadores
            //Jogador Atual


        }


    }
}
