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
            return Physics.Raycast(transform.position, Vector3.down, 0.5f);
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

        move();
        lookRotation();
    }

    private void fallingDown() {
        characterController.Move(Vector3.down * gravity * Time.deltaTime);
    }

    private void move() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDir = new Vector3(h, 0, v).normalized;
        float moveAmount = Mathf.Clamp01(new Vector2(h, v).sqrMagnitude);
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