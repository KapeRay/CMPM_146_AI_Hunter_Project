using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel.Cameras
{
    // https://www.youtube.com/watch?v=5djfhgQkrk0
    public class IP_TopDown_camera : MonoBehaviour
    {
        public Transform m_Target;
        public float m_Height = 10f;
        public float m_Distance = 20f;
        public float m_Angle = 45f;

        // Start is called before the first frame update
        void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
        }

        protected virtual void HandleCamera()
        {
            if (!m_Target)
            {
                return;
            }

            // Build world position vector
            Vector3 worldPosition = (Vector3.forward * m_Distance) + (Vector3.up * m_Height);
            Debug.DrawLine(m_Target.position, worldPosition, Color.red);

            // Build our rotated vector
            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;
            Debug.DrawLine(m_Target.position, rotatedVector, Color.green);
        }
    }
}

