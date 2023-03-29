using System.Linq;
using TMPro;
using UnityEngine;
using System;


namespace Game.Tools
{
    public class Logger : Singleton<Logger>
    {
        [SerializeField]
        private TextMeshProUGUI debugAreaText = null;

        // [SerializeField]
        // private bool enableDebug = false;

        [SerializeField]
        private int maxLines = 15;

        void Awake()
        {
            if (debugAreaText == null)
            {
                debugAreaText = GetComponent<TextMeshProUGUI>();
            }
            debugAreaText.text = string.Empty;
        }

        void OnEnable()
        {

            if (enabled)
            {
                debugAreaText.text += $"<color=\"white\"> {this.GetType().Name} enabled</color>\n";
            }

            DontDestroyOnLoad(gameObject);
        }

        public void LogPlayerAction(string message)
        {
            ClearLines();
            int playerID = TurnManager.Instance.PlayerAtual;
            // string playerColor = (GameDataconfig.Instance.PlayerColorOrder[playerID]);
            // ColorUtility
            string playerColor = "yellow";
            debugAreaText.text += $"<color=\"{playerColor}\">{string.Concat(GameDataconfig.Instance.TagParticipante," ",playerID," : ")}</color>";
            debugAreaText.text += $"<color=\"white\"> {message} </color> \n";

        }


        public void LogInfo(string message)
        {
            ClearLines();

            debugAreaText.text += $"<color=\"white\"> {message}</color>\n";
        }

        public void LogError(string message)
        {
            ClearLines();
            debugAreaText.text += $"<color=\"red\">{message}</color>\n";
        }

        public void LogWarning(string message)
        {
            ClearLines();
            debugAreaText.text += $"<color=\"yellow\">{message}</color>\n";
        }

        private void ClearLines()
        {
            if (debugAreaText.text.Split('\n').Count() > maxLines)
            {
                debugAreaText.text = (debugAreaText.text.Substring(debugAreaText.text.IndexOf("\n") + 1));

            }
        }

        public void ResetLoggger()
        {
            debugAreaText.text = string.Empty;
        }

    }
}
