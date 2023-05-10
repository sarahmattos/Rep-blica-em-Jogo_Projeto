using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class Boundary : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private Bounds bounds;
        public Bounds Bounds => bounds;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

    }
}
