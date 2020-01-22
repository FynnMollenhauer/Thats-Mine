using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class CameraStatic : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);

        private void LateUpdate()
        {
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);
            transform.position = (target.position + offset);

        }
    }
}
