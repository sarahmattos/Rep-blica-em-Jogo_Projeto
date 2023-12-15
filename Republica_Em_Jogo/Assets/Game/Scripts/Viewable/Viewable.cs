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
            

            OnHide?.Invoke();
        }

        public virtual void Show()
        {
            OnShow?.Invoke();
        }
    }
}
