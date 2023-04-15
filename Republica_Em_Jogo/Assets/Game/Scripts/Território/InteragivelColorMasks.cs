using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Territorio
{
    [Serializable]
    public class InteragivelColorMasks
    {
        [SerializeField] private Color _defaultColor = new Color(58, 58, 58);
        [SerializeField] private Color mouseEnter = new Color(255, 255, 255, 45);
        [SerializeField] private Color mouseExit = new Color(255, 255, 255, 0);
        [SerializeField] private Color blinkColor = new Color(255, 255, 255, 180);
        [SerializeField] private Color inativityColor = new Color(0,0,0, 145);
        [SerializeField] private Color activityColor = new Color(0,0,0,0);

        public Color MouseEnter => mouseEnter;
        public Color MouseExit => mouseExit;
        public Color BlinkColor => blinkColor;
        public Color InativityColor => inativityColor;
        public Color ActivityColor => activityColor;
        public Color DefaultColor => _defaultColor;
    }
}
