using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

namespace Game.Territorio
{
    public class CamaraMunicipal : MonoBehaviour
    {
        [SerializeField] private Renderer[] rendererPeoes;

        private Color defaultColor;

        private MaterialPropertyBlock materialPropertyBlock;

        private State InicializacaoState => GameStateHandler.Instance.GetState(GameState.GAMEPLAY_SCENE_LOAD);
        private static readonly int colorID = Shader.PropertyToID("_BaseColor");

        private void Start()
        {
            defaultColor = rendererPeoes[0].material.color;
            materialPropertyBlock = new MaterialPropertyBlock();
            InicializacaoState.Saida += SubscribeOnNumCadeirasMuda;
        }

        private void OnDestroy()
        {
            InicializacaoState.Saida -= SubscribeOnNumCadeirasMuda;
            UnsbscribeOnNumCadeirasMuda();

        }

        private void SubscribeOnNumCadeirasMuda()
        {
            foreach (PlayerStats playerStats in PlayerStatsManager.Instance.AllPlayerStats)
            {
                playerStats.NumCadeiras.OnValueChanged += ColorirPeao;
            }
        }
        private void UnsbscribeOnNumCadeirasMuda()
        {
            foreach (PlayerStats playerStats in PlayerStatsManager.Instance.AllPlayerStats)
            {
                playerStats.NumCadeiras.OnValueChanged -= ColorirPeao;
            }
        }

        private void ColorirPeao(int _, int __)
        {
            int peaoIndexAtual = 0;
            foreach (PlayerStats playerStats in PlayerStatsManager.Instance.AllPlayerStats)
            {
                for (int i = 0; i < playerStats.NumCadeiras.Value; i++)
                {
                    SetColor(rendererPeoes[peaoIndexAtual], playerStats.Cor);
                    peaoIndexAtual++;
                }
            }

            //Reset cor dos peoes restantes
            for (int i = peaoIndexAtual; i < rendererPeoes.Length; i++)
            {
                SetColor(rendererPeoes[i], defaultColor);
            }

            #region  antiga implementacao
            // // Debug.Log("coloriu");
            // for (int i = 0; i < cadeirasCamara.Length; i++)
            // {
            //     if (i == 0)
            //     {
            //         valorPeao = cadeirasCamara[i] * i;
            //     }
            //     else
            //     {
            //         valorPeao = cadeirasCamara[i - 1] * i;
            //     }

            //     PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            //     foreach (PlayerStats stats in allPlayerStats)
            //     {
            //         if (stats.playerID == i)
            //         {
            //             for (int j = 0; j < cadeirasCamara[i]; j++)
            //             {

            //                 Material material = peosCamara[j + (int)valorPeao].GetComponent<MeshRenderer>().material;
            //                 material.SetColor("_BaseColor", stats.Cor);
            //             }
            //         }
            //     }
            // }
            #endregion

        }



        private void SetColor(Renderer renderer, Color color)
        {
            materialPropertyBlock.SetColor(colorID, color);
            renderer.SetPropertyBlock(materialPropertyBlock);

        }

    }
}
