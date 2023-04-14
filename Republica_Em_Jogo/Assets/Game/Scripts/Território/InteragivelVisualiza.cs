using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace Game.Territorio
{
    public class InteragivelVisualiza : MonoBehaviour
    {
        private Interagivel interagivel;
        private Bairro bairro;
        private new Renderer renderer;
        private Material baseMaterial;
        private Material hoverMaterial;
        [SerializeField] private Color _defaultColor = new Color(58, 58, 58);

        [SerializeField] private InteragivelColorMasks colorMasks;

        private static readonly int baseColorID = Shader.PropertyToID("_BaseColor");
        // private MaterialPropertyBlock

        private Color ColorByPointerState => interagivel.PointerState == PointerState.ENTER ?
                        colorMasks.MouseIn :
                        colorMasks.MouseOut;

        //Apenas para testes
        private Vector3 scalaInicial;
        private void Awake()
        {
            interagivel = GetComponent<Interagivel>();
            renderer = GetComponent<Renderer>();
            bairro = GetComponentInParent<Bairro>();
            // propertyBlockABase = new MaterialPropertyBlock();



        }

        private void Start()
        {
            baseMaterial = renderer.materials[0];
            hoverMaterial = renderer.materials[1];
            scalaInicial = transform.localScale;

            interagivel.MouseEnter += MouseInVisualiza;
            interagivel.MouseExit += MouseOutVisualiza;
            interagivel.Click += MouseClickVisualiza;
            interagivel.MudaHabilitado += OnMudaHabilitado;
            interagivel.SelectBairro += OnSelectBairro;
            bairro.PlayerIDNoControl.OnValueChanged += SetBairroColorFromPlayer;
            
            //await SetMouseColorTaks(baseMaterial, _defaultColor, 0.1f);

            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO).Entrada += () => {
                GetComponent<Outline>().enabled = true;
            };

        }

        private void OnDestroy()
        {
            interagivel.MouseEnter -= MouseInVisualiza;
            interagivel.MouseExit -= MouseOutVisualiza;
            interagivel.Click -= MouseClickVisualiza;
            interagivel.MudaHabilitado -= OnMudaHabilitado;
            interagivel.SelectBairro -= OnSelectBairro;
            bairro.PlayerIDNoControl.OnValueChanged -= SetBairroColorFromPlayer;

                        GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO).Entrada += () => {
                GetComponent<Outline>().enabled = true;
            };
        }

        private void MouseInVisualiza()
        {
            SetHoverColor(colorMasks.MouseIn);
        }
        private void MouseOutVisualiza()
        {
            SetHoverColor(colorMasks.MouseOut);
        }
        private async void MouseClickVisualiza(Bairro _)
        {
            await SetClickColorTaks(0.1f);
        }
        private async void SetBairroColorFromPlayer(int _, int playerID)
        {
            Color playerColor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            // SetBairroColor(colorMasks.MouseOut);
            await SetColorTaks(baseMaterial, playerColor, 0.5f);

        }
        private async void SetHoverColor(Color color)
        {
            await SetMouseColorTaks(hoverMaterial, color, 0.1f);
        }
        private void OnSelectBairro(bool value)
        {
            Tools.Logger.Instance.LogPlayerAction("Selecionou o bairro " + value + " : " + bairro.Nome);
        }
        
        private void OnMudaHabilitado(bool value)
        {
            transform.localScale = value ?
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z * 4f) :
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z);

            if (!value)
            {
                SetHoverColor(colorMasks.MouseOut);
            }
        }


        private async Task SetClickColorTaks(float time)
        {
            float elapsedTime = 0f;
            hoverMaterial.SetColor(baseColorID, colorMasks.MouseClick);
            while (elapsedTime < time)
            {
                hoverMaterial.SetColor(baseColorID,
                    Color.LerpUnclamped(hoverMaterial.color,
                    colorMasks.MouseIn,
                    elapsedTime / time
                    )
                );

                elapsedTime += Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));

            }
            hoverMaterial.SetColor(baseColorID, ColorByPointerState);

        }

        private async Task SetMouseColorTaks(Material material, Color color, float time)
        {

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                material.SetColor(baseColorID,
                    Color.LerpUnclamped(material.color,
                    color,
                    elapsedTime / time
                    )
                );

                elapsedTime += Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));

            }
            material.SetColor(baseColorID, ColorByPointerState);
        }


        private async Task SetColorTaks(Material material, Color color, float time)
        {

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                material.SetColor(baseColorID,
                    Color.LerpUnclamped(material.color,
                    color,
                    elapsedTime / time
                    )
                );

                elapsedTime += Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));

            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.I)) {
                TakeAnimationClientRpc();
            }
        }

        [ClientRpc]
        public void TakeAnimationClientRpc() {
            Tools.Logger.Instance.LogInfo("animation");
        }

    }



    [Serializable]
    public class InteragivelColorMasks
    {

        [SerializeField] private Color mouseIn = new Color(255, 255, 255, 45);
        [SerializeField] private Color mouseOut = new Color(255, 255, 255, 0);
        [SerializeField] private Color mouseClick = new Color(255, 255, 255, 180);

        public Color MouseIn { get => mouseIn; set => mouseIn = value; }
        public Color MouseOut { get => mouseOut; set => mouseOut = value; }
        public Color MouseClick { get => mouseClick; set => mouseClick = value; }
    }
}
