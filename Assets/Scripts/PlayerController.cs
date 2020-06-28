using UnityEngine;

public sealed class PlayerController : MonoBehaviour {
    private Animator animator = null;
    private CharacterController characterController = null;
    private Vector3 moveDir = Vector3.zero;
    private float moveSpeed = 0f;

    private bool isInAction = false;

    [Header("Item")]
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject hammer;
    
    [Header("Modals")]
    [SerializeField] private GameObject statusWindow;
    [SerializeField] private GameObject inventoryWindow;

    [Header("Status Settings")]
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float normalMoveSpeed = 12.0f;
    [SerializeField] private float sprintMoveSpeed = 18.0f;
    [SerializeField] private float rotationSpeed = 120.0f;
    [SerializeField] private float threshold = 0.5f;

    private AudioSource footStep;

    private bool IsGrounded {
        get {
            return Physics.Raycast(transform.position, Vector3.down, 1.0f);
        }
    }

    public InteractableObject InteractableObject { get; set; }

    private void Start() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        moveSpeed = normalMoveSpeed;
        footStep = GetComponent<AudioSource>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            statusWindow.SetActive(!statusWindow.activeSelf);
            inventoryWindow.SetActive(statusWindow.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryWindow.SetActive(!inventoryWindow.activeSelf);
        }

        CameraFollow.EnabledInput = !statusWindow.activeSelf && !inventoryWindow.activeSelf;

        if (!IsGrounded || isInAction) return;

        if (Input.GetButtonDown("Sprint")) this.startSprint();
        else if (Input.GetButtonUp("Sprint")) this.stopSprint();

        if (!InteractableObject) return;

        if (Input.GetKeyDown(KeyCode.F)) {
            this.stopSprint();
            animator.SetFloat("moveAmount", 0f);
            footStep.Stop();

            InteractableObject.doAction();
        }        
    }

    private void FixedUpdate() {
        if (!IsGrounded) {
            fallingDown();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            InteractableObject?.stopAction();
            if (InteractableObject is PopupWindowObject) {
                inventoryWindow.SetActive(false);
                UIManager.Instance.Enhancement.gameObject.SetActive(false);
                UIManager.Instance.Repairment.gameObject.SetActive(false);
            }
        }

        if (CameraFollow.EnabledCursor) {
            moveDir = Vector3.zero;
            animator.SetFloat("moveAmount", 0f);
            footStep.Stop();
            return;
        }

        if (isInAction) return;

        move();
        lookRotation();
    }

    private void startSprint() {
        moveSpeed = sprintMoveSpeed;
        animator.SetBool("Sprint", true);
        footStep.pitch = 1.3f;
    }

    private void stopSprint() {
        moveSpeed = normalMoveSpeed;
        animator.SetBool("Sprint", false);
        footStep.pitch = 1f;
    }

    private void fallingDown() {
        characterController.Move(Vector3.down * gravity * Time.deltaTime);
    }

    private void move() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 h = inputX * Camera.main.transform.right;
        Vector3 v = inputY * Camera.main.transform.forward;
        v.y = 0f;

        moveDir = (h + v).normalized;
        moveDir.y = 0f;

        float moveAmount = Mathf.Clamp01(new Vector2(inputX, inputY).sqrMagnitude);
        animator.SetFloat("moveAmount", moveAmount);

        if (moveAmount < threshold) {
            footStep.Stop();
            return;
        }

        if (!footStep.isPlaying) {
            footStep.volume = GameData.Instance.FXVolume;
            footStep.Play();
        }

        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void lookRotation() {
        if (moveDir == Vector3.zero) {
            moveDir = transform.forward;
        }

        Quaternion lookAngle = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, lookAngle, rotationSpeed);
    }
    
    public void controlAnimation(InteractType type, bool enable) {
        isInAction = enable;
        animator.SetBool(type.ToString(), enable);

        switch (type) {
            case InteractType.Mining:
                pickaxe.SetActive(enable); break;
            case InteractType.Hammering:
                hammer.SetActive(enable); break;
            default: break;
        }
    }
}