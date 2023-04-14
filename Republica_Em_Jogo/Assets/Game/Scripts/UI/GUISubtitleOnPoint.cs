using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Vector2 offSet = new Vector2(10, 30);


        private void Start()
        {
            camera = Camera.main;
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
            if(!mouseIn) return;
            GUI.Label(new Rect(MousePosition.x+offSet.x, -MousePosition.y+camera.pixelHeight-offSet.y, 150, 50), subtitle);

        }


    }
}
