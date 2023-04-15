using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Player;
using Game.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{

    public class UIIconeCorJogador : MonoBehaviour
    {
        [SerializeField] private int playerID;
        [SerializeField][Range(0.2f, 1)] private float MinWidthPercentage = 0.5f;
        [SerializeField][Range(0.2f, 1)] private float MaxWidthPercentage = 1;
        private RectTransform rectTransform;
        private RectTransform parentRectTransform;
        private Image image;
        private VerticalLayoutGroup verticalLayoutGroup;
        public float MinWidth => parentRectTransform.sizeDelta.x * MinWidthPercentage;
        public float MaxWidth => parentRectTransform.sizeDelta.x * MaxWidthPercentage - verticalLayoutGroup.padding.left;


        private void GetReferenceComponents()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
            verticalLayoutGroup = GetComponentInParent<VerticalLayoutGroup>();
        }

        private void Start()
        {
            TurnManager.Instance.turnoMuda += OnTurnoMudaAsync;

            // if(transform.childCount == 0) return;
            // for (int i = 0; i < transform.childCount; i++)
            // {
            //     Destroy(transform.GetChild(i).gameObject);
            // }

        }

        private void OnDestroy()
        {
            TurnManager.Instance.turnoMuda -= OnTurnoMudaAsync;
        }



        public async void Intialize(Color color, int playerIndex)
        {
            GetReferenceComponents();
            image.color = color;
            this.playerID = playerIndex;

            //Set RectTransform largura inicial de acordo com o player Atual
            if (TurnManager.Instance.PlayerAtual != playerID) await SetRectWidthAsync(MinWidth);
            else await SetRectWidthAsync(MaxWidth);

        }


        private async void UpdateRectTransform()
        {
            if (PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().playerID == playerID)
            {
                await SetRectWidthAsync(MaxWidth);
            }
            else
                await SetRectWidthAsync(MinWidth);
        }


        private async void OnTurnoMudaAsync(int previousPlayerID, int nextPlayerID)
        {
            if (playerID == nextPlayerID)
            {
                await SetRectWidthAsync(MaxWidth);
                return;
            }
            if (playerID == previousPlayerID)
            {
                await SetRectWidthAsync(MinWidth);
                return;
            }

        }

        private async Task SetRectWidthAsync(float width)
        {

            Vector2 sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
            await rectTransform.SetRectTransfomSizeDeltaSmoothAsync(sizeDelta);

        }


        // private async void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.N))
        //     {
        //         await SetRectWidthAsync(MinWidth);
        //     }
        //     if (Input.GetKeyDown(KeyCode.M))
        //     {
        //         await SetRectWidthAsync(MaxWidth);
        //     }
        // }



    }
}
