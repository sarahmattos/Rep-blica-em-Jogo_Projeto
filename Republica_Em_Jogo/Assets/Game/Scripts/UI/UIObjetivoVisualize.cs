using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UIObjetivoVisualize : MonoBehaviour
    {
        [SerializeField] private RectTransform objetivoRectTransform;
        [SerializeField] private TMP_Text textObjetivo;

        [SerializeField] private float minHeigth;
        private float maxHeigth;

        // private void Awake()
        // {
        //     objetivoRectTransform = GetComponent<RectTransform>();
        // }

        private void Start()
        {
            maxHeigth = objetivoRectTransform.sizeDelta.y;
        }

        public void ShowObjetivo()
        {
            SetHeigthObjetivoRectTransform(maxHeigth);
            textObjetivo.enabled = true;
        }

        public void HideObjetivo()
        {
            SetHeigthObjetivoRectTransform(minHeigth);
            textObjetivo.enabled = true;

        }

        private void SetHeigthObjetivoRectTransform(float value)
        {
            Vector2 sizeDelta = new Vector2(objetivoRectTransform.sizeDelta.x, value);
            objetivoRectTransform.sizeDelta = sizeDelta;
            // await objetivoRectTransform.EaseOutQuadraticSizeDelta(sizeDelta, 0.1f);
        }


    }
}

