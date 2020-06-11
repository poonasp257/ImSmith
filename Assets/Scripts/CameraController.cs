using UnityEngine;

public sealed class CameraController : MonoBehaviour {
    private GameObject playerObj = null;
    private Vector3 rotation = Vector2.zero;

    [Header("Camera Settings")]
    [SerializeField] private float cameraHeight = 1.0f;
    [SerializeField] private float distanceToPlayer = -1.0f;
    [SerializeField] private float followSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 1.0f;
 
    private void Start() {
        playerObj = GameObject.FindWithTag("Player");
    }

    private void Update() {
        if(playerObj == null) return;

        Vector3 playerPosition = playerObj.transform.position;
        followPlayer(playerPosition);
        rotateCamera(playerPosition);
    }

    private void followPlayer(Vector3 playerPosition) {
        playerPosition.y += cameraHeight;
        playerPosition.z += distanceToPlayer;
        this.transform.position = Vector3.MoveTowards(this.transform.position, 
            playerPosition, followSpeed * Time.deltaTime);
    }

    private void rotateCamera(Vector3 playerPosition) {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        rotation.y += h * rotationSpeed * Time.deltaTime;
        rotation.x += v * rotationSpeed * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -30f, 30f);
        Vector3 look = Quaternion.Euler(-rotation.x, rotation.y, 0) * Vector3.forward;
        playerPosition.y += cameraHeight;
        transform.position = playerPosition + look * distanceToPlayer;
        this.transform.LookAt(playerObj.transform);
    }
}