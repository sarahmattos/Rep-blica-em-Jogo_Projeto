using System;
using Game.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class InteragivelBackground : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool habilitado = false;
        private Material material;
        [SerializeField] private Color blinkColor;
        private Color initialColor;
        private static readonly int baseColorID = Shader.PropertyToID("_BaseColor");
        public event Action Click;

        private void Start()
        {
            material = GetComponent<Renderer>().material;
            initialColor = material.GetColor(baseColorID);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!habilitado) return;
            Click?.Invoke();
            Blink();

        }

        public void MudaHabilitado(bool value) {
            habilitado = value;
        }



        private void Blink()
        {
            material.SetColor(baseColorID, blinkColor);
            SetColorLerp(material, initialColor, 0.1f);
        }


        public async void SetColorLerp(Material material, Color color, float time)
        {
            await material.EaseOutColor(color, time);
        }

    }
}
