using UnityEngine;

public class DisplayUI : MonoBehaviour {
    private GameObject mainCamera = null;

    private void Start() {
        mainCamera = Camera.main.gameObject;
    }

    private void LateUpdate() {
        if (!mainCamera) return;

        Quaternion lookAngle = Quaternion.LookRotation(
            mainCamera.transform.position - transform.position);
        transform.rotation = new Quaternion(0, lookAngle.y, 0, lookAngle.w);
    }
}