using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;
using UnityEngine;

namespace Game
{
    // [System.Serializable]
    public class RemanejamentoData
    {

        private Dictionary<Bairro, int> parBairroEleitor = new Dictionary<Bairro, int>();
        private PlayerStats playerStatsAtual => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual();

        private Bairro bairroEscolhido;
        private Bairro vizinhoEscolhido;

        public Dictionary<Bairro, int> ParBairroEleitor => parBairroEleitor;
        public Bairro BairroEscolhido { get => bairroEscolhido; set => bairroEscolhido = value; }
        public Bairro VizinhoEscolhido { get => vizinhoEscolhido; set => vizinhoEscolhido = value; }


        public void ClearData()
        {
            parBairroEleitor.Clear();
            ResetSelectedBairros();
        }

        public void ResetSelectedBairros()
        {
            
            bairroEscolhido?.Interagivel.ChangeSelectedBairro(false);
            VizinhoEscolhido?.Interagivel.ChangeSelectedBairro(false);
            BairroEscolhido = null;
            VizinhoEscolhido = null;
        }

        public void ArmazenarBairrosRemanejaveis()
        {
            foreach (Bairro bairro in playerStatsAtual.BairrosInControl)
            {
                if (bairro.SetUpBairro.Eleitores.contaEleitores <= 1) continue;
                parBairroEleitor.Add(bairro, bairro.SetUpBairro.Eleitores.contaEleitores - 1);
            }
        }

        public void RemoveBairro(Bairro bairro)
        {
            parBairroEleitor.Remove(bairro);
        }

        // public int GetIndexInParBairroEleitor(Bairro bairro)
        // {
        //     return parBairroEleitor.Keys.ToList().IndexOf(bairro);
        // }

        // public void DecrementaEleitorInBairroDictionary(Bairro bairro)
        // {
        //     parBairroEleitor[bairro] += -1;

        // }

        public int GetEleitor(Bairro bairro)
        {
            return ParBairroEleitor[bairro];


        }




    }
}
