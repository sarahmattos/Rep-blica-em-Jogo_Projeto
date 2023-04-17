using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Territorio
{
    //controla a modificação de 3 materiais aplicados ao bairro.
    //base: material que define a cor do bairro (cor dos jogadores/times)
    //hover: material que define modificação da variação da cor base quando o jogador interage com o bairro pelo interagivelVisualiza)
    //faz isso adicionado variações de cores brancas com alpha reduzido.
    //as interações são: quando o jogador coloca o mouse sobre o bairro ou quando clica.
    //inativity: quando o jogador precisa interagir com o bairro, o bairros que NÃO podem interagir são assinalados como inativos, pelo interagivel.
    //como respostas, bairros inativos também recem modificações adicionaod variações de cores escuras com alpha reduzido. 
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
        // private Color ColorByPointerState => interagivel.PointerState == PointerState.ENTER ?
        //                                         ColorMasks.MouseEnter :
        //                                         ColorMasks.MouseExit;
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
            await SetColorLerp(baseMaterial, playerColor);

        }

        public void SetMouseEnterColor()
        {
            hoverMaterial.SetColor(baseColorID, colorMasks.MouseEnter);
        }
        public void SetMouseOutColor()
        {
            hoverMaterial.SetColor(baseColorID, colorMasks.MouseExit);

        }

        public void SetHoverColor(Color color)
        {
            hoverMaterial.SetColor(baseColorID, color);
        }

        public void SetInativityColor(bool value)
        {
            if (value)
            {
                inativityMaterial.SetColor(baseColorID, colorMasks.InativityColor);
            }
            else
            {
                inativityMaterial.SetColor(baseColorID, colorMasks.ActivityColor);
                hoverMaterial.SetColor(baseColorID, ColorMasks.MouseExit);
            }
        }


        public async Task BlinkBairroColorTask()
        {
            float elapsedTime = 0f;
            float maxTime = 0.3f;
            hoverMaterial.SetColor(baseColorID, colorMasks.BlinkColor);
            while (elapsedTime < maxTime)
            {
                hoverMaterial.SetColor(baseColorID,
                    Color.Lerp(hoverMaterial.color,
                            colorMasks.MouseExit,
                            maxTime
                    )
                );
                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

            SetHoverColorByPointerState();
        }

        public void SetHoverColorByPointerState()
        {
            //ajustar aqui
            if (interagivel.PointerState == PointerState.ENTER)
            {
                hoverMaterial.SetColor(baseColorID, colorMasks.MouseEnter);
            }
            else
            {
                hoverMaterial.SetColor(baseColorID, colorMasks.MouseExit);
            }
        }

        public async Task SetColorLerp(Material material, Color color)
        {

            float elapsedTime = 0f;
            float maxTime = 0.5f;
            while (elapsedTime < maxTime)
            {
                material.SetColor(baseColorID,
                    Color.Lerp(material.color,
                            color,
                            maxTime
                    )
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));

            }
        }




    }
}
