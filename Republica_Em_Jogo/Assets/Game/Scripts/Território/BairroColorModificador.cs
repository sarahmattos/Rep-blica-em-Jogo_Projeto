using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Territorio
{
    public class BairroColorModificador : MonoBehaviour
    {
        private new Renderer renderer;
        private Material baseMaterial;
        private Material hoverMaterial;
        private Material inativityMaterial;
        private Interagivel interagivel;
        [SerializeField] private InteragivelColorMasks colorMasks;
        private static readonly int baseColorID = Shader.PropertyToID("_BaseColor");
        public InteragivelColorMasks ColorMasks => colorMasks;
        private Color ColorByPointerState => interagivel.PointerState == PointerState.ENTER ?
                                                ColorMasks.MouseEnter :
                                                ColorMasks.MouseExit;
        private void Start()
        {
            interagivel = GetComponent<Interagivel>();
            renderer = GetComponent<Renderer>();
            baseMaterial = renderer.materials[0];
            hoverMaterial = renderer.materials[1];
            inativityMaterial = renderer.materials[2];

        }

        public void ResetColorMasking()
        {
            SetInativityColor(false);
            hoverMaterial.SetColor(baseColorID, ColorMasks.MouseExit);
        }

        public async void SetBairroColorByPlayer(int previousPlayerID, int nextPlayerID)
        {
            Color playerColor = GameDataconfig.Instance.PlayerColorOrder[nextPlayerID];
            // SetColor(baseMaterial, playerColor);
            await SetColorLerpTaks(baseMaterial, playerColor, 0.5f);

        }

        public void SetMouseEnterColor()
        {
            SetColor(hoverMaterial, colorMasks.MouseEnter);
        }
        public void SetMouseOutColor()
        {
            SetColor(hoverMaterial, colorMasks.MouseExit);

        }


        public void SetInativityColor(bool value)
        {
            if (value) SetColor(inativityMaterial, colorMasks.InativityColor);
            else SetColor(inativityMaterial, colorMasks.ActivityColor);
        }

        public void SetColor(Material material, Color color)
        {
            material.SetColor(baseColorID, color);
        }

        public async Task BlinkBairroColorTask()
        {
            float elapsedTime = 0f;
            float maxTime = 0.1f;
            hoverMaterial.SetColor(baseColorID, colorMasks.BlinkColor);
            while (elapsedTime < maxTime)
            {
                hoverMaterial.SetColor(baseColorID,
                    Color.Lerp(hoverMaterial.color,
                            colorMasks.MouseEnter,
                            maxTime
                    )
                );
                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }
            hoverMaterial.SetColor(baseColorID, ColorByPointerState);

        }

        public async Task SetColorLerpTaks(Material material, Color color, float time)
        {

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                material.SetColor(baseColorID,
                    Color.Lerp(material.color,
                            color,
                            time
                    )
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));

            }
        }




    }
}
