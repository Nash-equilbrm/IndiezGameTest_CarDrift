using UnityEngine;


namespace Game.Camera
{
    public class SmoothFollowCamera : MonoBehaviour
    {
        public Transform target; // Target to follow
        public Vector3 offset = new Vector3(0, 5, -10); // Offset from the target
        public float smoothSpeed = 5f; // Smoothness factor

        void LateUpdate()
        {
            if (target == null) return;

            // Desired position
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Make the camera look at the target
            transform.LookAt(target.position);
        }
    }
}
