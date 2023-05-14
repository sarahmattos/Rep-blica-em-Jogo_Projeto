using Game.Player;
using UnityEngine;

namespace Game.Tools
{

    //mostra dos sobre a sessao de jogo (rodada, turno atual, stado atual, player atual, etc...)
    public class DevGUISessionData : MonoBehaviour
    {

        [SerializeField] private RodadaController rodadaController;

        int w => Screen.width;
        int h => Screen.height;

        GuiLabelParams guiLabelParams;

        private void Start()
        {
            guiLabelParams = new GuiLabelParams()
            {
                rect = new Rect(0, h / 2, w / 2, h * 2 / 90),
                guiStyle = new GUIStyle()
            };
            guiLabelParams.guiStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
            guiLabelParams.guiStyle.alignment = TextAnchor.MiddleLeft;

        }

        void OnGUI()
        {
            if (!DevToolsHandler.Instance.DevToolActive) return;
            guiLabelParams.guiStyle.fontSize = h * 2 / 90;


            //Rodada
            guiLabelParams.rect.y = h / 2;
            string message = string.Concat("Rodada: ", rodadaController.Rodada.ToString());
            GUI.Label(guiLabelParams.rect, message, guiLabelParams.guiStyle);

            //GameState
            guiLabelParams.rect.y = h / 2 + 2 * 2 * h / 90;
            string message2 = string.Concat("Rodada: ", GameStateHandler.Instance.StateMachineController.GetCurrentState().name);
            GUI.Label(guiLabelParams.rect, message2, guiLabelParams.guiStyle);

            //CoreLoopstate
            guiLabelParams.rect.y = h / 2 + 3 * 2 * h / 90;
            string message3 = string.Concat("Rodada: ", CoreLoopStateHandler.Instance.CurrentState.name);
            GUI.Label(guiLabelParams.rect, message3, guiLabelParams.guiStyle);
            //ordem dos jogadores

            guiLabelParams.rect.y = h / 2 + 4 * 2 * h / 90;
            string message4 = "jogadores: ";
            foreach (int playerID in TurnManager.Instance.ordemPlayersID)
            {
                message4 = string.Concat(message4, playerID, "_");
            }
            GUI.Label(guiLabelParams.rect, message4, guiLabelParams.guiStyle);

            //Jogador Atual
            guiLabelParams.rect.y = h / 2 + 5 * 2 * h / 90;
            string message5 = string.Concat("Jogador ATUAL: ", TurnManager.Instance.PlayerAtual);
            GUI.Label(guiLabelParams.rect, message5, guiLabelParams.guiStyle);

            //Jogador Local
            guiLabelParams.rect.y = h / 2 + 6 * 2 * h / 90;
            string message6 = string.Concat("Jogador LOCAL: ", PlayerStatsManager.Instance.GetLocalPlayerStats().Nome);
            GUI.Label(guiLabelParams.rect, message6, guiLabelParams.guiStyle);
            
            //Turno
            guiLabelParams.rect.y = h / 2 + 7 * 2 * h / 90;
            string message7 = string.Concat("Turno: ", TurnManager.Instance.TurnCount);
            GUI.Label(guiLabelParams.rect, message7, guiLabelParams.guiStyle);


        }



    }

    public struct GuiLabelParams
    {
        public Rect rect;
        public GUIStyle guiStyle;
    }
}
