using Game.Territorio;

namespace Game
{
    public class AvancoData
    {
        private Bairro bairroEscolhido;
        private Bairro vizinhoEscolhido;

        public Bairro BairroEscolhido
        {
            get => bairroEscolhido;
            set => bairroEscolhido = value;
        }
        public Bairro VizinhoEscolhido
        {
            get => vizinhoEscolhido;
            set => vizinhoEscolhido = value;
        }

        public void ClearData()
        {
            bairroEscolhido = null;
            vizinhoEscolhido = null;
        }
    }
}
