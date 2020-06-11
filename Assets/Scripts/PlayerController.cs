using UnityEngine;

public sealed class PlayerController : MonoBehaviour {
    private Animator animator = null;
    private CharacterController characterController = null;
    private Vector3 moveDir = Vector3.zero;

    [Header("Status Settings")]
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float moveSpeed = 120.0f;
    [SerializeField] private float rotationSpeed = 120.0f;
    [SerializeField] private float threshold = 0.5f;

    private bool IsGrounded {
        get {
            return Physics.Raycast(transform.position, Vector3.down, 1.0f);
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate() {
        if (!IsGrounded) {
            fallingDown();
            return;
        }

        if (CameraFollow.EnabledCursor) {
            moveDir = Vector3.zero;
            animator.SetFloat("moveAmount", 0f);
            return;
        }

        move();
        lookRotation();
    }

    private void fallingDown() {
        characterController.Move(Vector3.down * gravity * Time.deltaTime);
    }

    private void move() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 h = inputX * Camera.main.transform.right;
        Vector3 v = inputY * Camera.main.transform.forward;
        moveDir = (h + v).normalized;
        moveDir.y = 0f;

        float moveAmount = Mathf.Clamp01(new Vector2(inputX, inputY).sqrMagnitude);
        animator.SetFloat("moveAmount", moveAmount);

        if (moveAmount < threshold) return;

        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void lookRotation() {
        if (moveDir == Vector3.zero) {
            moveDir = transform.forward;
        }

        Quaternion lookAngle = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, lookAngle, rotationSpeed);
    }
}