using UnityEngine;

public class CameraFollow2D : MonoBehaviour {
    public Transform target;
    public Transform selfT;
    private const float SMOOTH_TIME = 18F;
    private Vector3 velocity = Vector3.zero;

    private float originalZ;
    private void Awake() {
        originalZ = selfT.position.z;
    }

    void FixedUpdate() {
        // Smoothly move the camera towards that target position
        Vector3 cameraPosition = Vector3.SmoothDamp(selfT.position, target.position, ref velocity, SMOOTH_TIME * Time.fixedDeltaTime);
        cameraPosition.z = originalZ;
        selfT.position = cameraPosition;
    }
}