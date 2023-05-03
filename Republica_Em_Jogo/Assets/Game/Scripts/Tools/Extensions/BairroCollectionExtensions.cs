using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using UnityEngine;

namespace Game.Tools
{
    public static class BairroCollectionExtensions
    {

        public static void MudarInativity(this IEnumerable<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarInativity(value);
            }

        }

        public static void MudarInteragivel(this IEnumerable<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        public static void MudarHabilitado(this IEnumerable<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        public static void MudarSeleced(this IEnumerable<Bairro> bairros, bool value)
        {
            foreach (Bairro bairro in bairros)
            {
                bairro.Interagivel.ChangeSelectedBairro(value);
            }
        }


    }
}
