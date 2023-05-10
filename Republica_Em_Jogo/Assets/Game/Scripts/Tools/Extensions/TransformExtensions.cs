using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tools
{
    public static class TransformExtensions 
    {
        public static void ClampPositionXZ(this Transform transform,  Bounds value)
        {
            Vector3 vector3 = new Vector3();
            vector3.x = Mathf.Clamp(transform.position.x, value.min.x, value.max.x);
            vector3.y = value.center.y;
            vector3.z = Mathf.Clamp(transform.position.z, value.min.z, value.max.z);
            
            transform.position = vector3;
        }


        public static void Clamp(this Vector3 vector3,  Vector3 value)
        {
            vector3.x = Mathf.Clamp(vector3.x, -value.x, value.x);
            vector3.y = Mathf.Clamp(vector3.y, -value.y, value.y);
            vector3.z = Mathf.Clamp(vector3.z, -value.z, value.z);
        }

    }
}
