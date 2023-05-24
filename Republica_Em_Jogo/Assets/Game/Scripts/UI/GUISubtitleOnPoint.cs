using System;
using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class GUISubtitleOnPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string subtitle;
        private new Camera camera;

        private bool mouseIn = false;

        public Vector3 MousePosition => Input.mousePosition;
        [SerializeField] private Vector2 offSet = new Vector2(10, 15);
        int w => Screen.width;
        int h => Screen.height;

        GuiLabelParams guiLabelParams;

        private void Start()
        {
            camera = Camera.main.gameObject.GetComponent<Camera>();

            guiLabelParams = new GuiLabelParams()
            {
                rect = new Rect(0, h / 2, w / 2, h * 2 / 90),
                guiStyle = new GUIStyle()
            };
            guiLabelParams.guiStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
            guiLabelParams.guiStyle.alignment = TextAnchor.MiddleLeft;

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseIn = false;
        }

        private void OnGUI()
        {
            if (!mouseIn) return;
            UpdateFontSize();
            UpdateRect();
            GUI.Label(guiLabelParams.rect, subtitle, guiLabelParams.guiStyle);


        }

        private void UpdateFontSize()
        {
            guiLabelParams.guiStyle.fontSize = h * 2 / 90;
        }
        private void UpdateRect()
        {
            guiLabelParams.rect = new Rect(MousePosition.x + offSet.x, -MousePosition.y + camera.pixelHeight - offSet.y, 150, 50);
        }

    }
}
