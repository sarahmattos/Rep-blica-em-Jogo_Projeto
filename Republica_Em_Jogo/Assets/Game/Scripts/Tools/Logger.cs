using System.Linq;
using TMPro;
using UnityEngine;
using System;


namespace Game.Tools
{
    public class Logger : Singleton<Logger>
    {
        [SerializeField] private TextMeshProUGUI debugAreaText = null;
        private LoggerDisplayer loggerDisplayer;
        public LoggerDisplayer LoggerDisplayer => loggerDisplayer;

        // [SerializeField]
        // private bool enableDebug = false;

        [SerializeField] private int maxLines = 15;

        void Awake()
        {
            loggerDisplayer = GetComponent<LoggerDisplayer>();

            debugAreaText.text = string.Empty;
        }

        void OnEnable()
        {

            if (enabled)
            {
                debugAreaText.text += "\n";
            }

            DontDestroyOnLoad(gameObject);
        }

        public void LogPlayerAction(string message)
        {
            ClearLines();
            int playerID = TurnManager.Instance.PlayerAtualID;
            string playerHexColor = ColorUtility.ToHtmlStringRGB((GameDataconfig.Instance.PlayerColorOrder[playerID]));
            debugAreaText.text += string.Concat(GameDataconfig.Instance.TagPlayerAtualColorizada()," : ");
            debugAreaText.text += string.Format("<color=#{0}>{1}</color>","#fff", string.Concat(message,"\n"));
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
            debugAreaText.text += $"<color=\"green\">{message}</color>\n";
        }

        private void ClearLines()
        {
            if (debugAreaText.text.Split('\n').Count() > maxLines)
            {
                debugAreaText.text = (debugAreaText.text.Substring(debugAreaText.text.IndexOf("\n") + 1));

            }
        }

        public void ResetLogger()
        {
            debugAreaText.text = string.Empty;
        }

    }
}
