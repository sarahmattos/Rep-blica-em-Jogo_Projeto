using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIIconeCorJogador : MonoBehaviour
    {
        [SerializeField] private int playerID;
        [SerializeField][Range(0.2f, 1)] private float MinWidthPercentage = 0.5f;
        [SerializeField][Range(0.2f, 1)] private float MaxWidthPercentage = 1;
        public RectTransform rectTransform;
        public RectTransform parentRectTransform;
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
            TurnManager.Instance.turnoMuda += OnTurnoMuda;

            // if(transform.childCount == 0) return;
            // for (int i = 0; i < transform.childCount; i++)
            // {
            //     Destroy(transform.GetChild(i).gameObject);
            // }

        }

        private void OnDestroy()
        {
            TurnManager.Instance.turnoMuda -= OnTurnoMuda;
        }



        public void Intialize(Color color, int playerIndex)
        {
            GetReferenceComponents();
            image.color = color;
            this.playerID = playerIndex;

            //Set RectTransform largura inicial de acordo com o player Atual
            if (TurnManager.Instance.PlayerAtual != playerID) SetRectWidth(MinWidth);
            else SetRectWidth(MaxWidth);

        }


        private void UpdateRectTransform()
        {
            if (PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().playerID == playerID)
            {
                SetRectWidth(MaxWidth);
            }
            else
                SetRectWidth(MinWidth);
        }


        private void OnTurnoMuda(int previousPlayerID, int nextPlayerID)
        {
            //     Tools.Logger.Instance.LogInfo("Player atual :"+TurnManager.Instance.PlayerAtual);
            //     Tools.Logger.Instance.LogInfo(string.Concat("turno muda ", previousPlayerID, " : ", nextPlayerID));
            if (playerID == nextPlayerID)
            {
                SetRectWidth(MaxWidth);
                return;
            }
            if (playerID == previousPlayerID)
            {
                SetRectWidth(MinWidth);
                return;
            }

        }

        private void SetRectWidth(float width)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = width;
            rectTransform.sizeDelta = sizeDelta;
        }



    }
}
