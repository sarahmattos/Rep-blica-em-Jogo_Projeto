using UnityEngine;
using UnityEngine.UI;

namespace Game.Player
{
    [RequireComponent(typeof(Image))]
    public class ColorizeByPlayer : MonoBehaviour
    {
        private Image image;

        public PlayerStats PlayerStatsLocal => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual();
        public State InicializacaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO);

        void Start()
        {
            image = GetComponent<Image>();
            InicializacaoState.Entrada += ApplyPlayerColor;
        }

        private void OnDestroy()
        {
            InicializacaoState.Entrada -= ApplyPlayerColor;
        }


        private void ApplyPlayerColor()
        {
            image.color = PlayerStatsLocal.Cor;
        }


    }
}
