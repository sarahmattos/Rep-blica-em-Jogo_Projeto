using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class Viewable : MonoBehaviour, IViewable
    {
        public UnityEvent OnShow;
        public UnityEvent OnHide;

        public virtual void Hide()
        {
            OnShow?.Invoke();
        }

        public virtual void Show()
        {
            OnHide?.Invoke();
        }
    }
}
