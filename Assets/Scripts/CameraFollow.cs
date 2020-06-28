using UnityEngine;

public class CameraFollow : MonoBehaviour {
	private float inputX = 0f;
	private float inputY = 0f;
	private Vector3 currentRotation;
	private bool enabledRotating = false;

	private Transform target = null;

	[SerializeField] private float inputSensitivity = 1.0f;
	[SerializeField] private float clampAngle = 60.0f;
	[SerializeField] private float moveSpeed = 60.0f;
	[SerializeField] private float cameraHeight = 1.0f;

	public static bool EnabledCursor {
		get { return Cursor.visible; }
	}

	public static bool EnabledInput { get; set; }

	private void Start() {
		GameObject player = GameObject.FindWithTag("Player");
		if (player == null) return;

		target = this.createFollowTarget(player);

		currentRotation = transform.localRotation.eulerAngles;

		ShowCursor();
		EnabledInput = true;
	}

    private void Update() {
		if (!EnabledCursor || enabledRotating) {
			processInput();
		}

		if (Input.GetButtonDown("Cursor")) {
			if (EnabledCursor) {
				HideCursor();
				return;
			}

			ShowCursor();
		}

		if (!EnabledInput) return;

		if (Input.GetMouseButtonDown(1) && EnabledCursor) {
			HideCursor();
			enabledRotating = true;
		}
		else if (Input.GetMouseButtonUp(1) && enabledRotating) {
			ShowCursor();
			enabledRotating = false;
		}		
	}

    private void LateUpdate() {
		followTarget();
	}

	private void processInput() {
		inputX = Input.GetAxis("Mouse X");
		inputY = -Input.GetAxis("Mouse Y");

		currentRotation.y += inputX * inputSensitivity * Time.deltaTime;
		currentRotation.x += inputY * inputSensitivity * Time.deltaTime;
		currentRotation.x = Mathf.Clamp(currentRotation.x, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler(currentRotation);
		transform.rotation = localRotation;
	}

    private Transform createFollowTarget(GameObject parentObj) {
        var targetObj = new GameObject("Camera Follow Target");
        targetObj.transform.parent = parentObj.transform;
        targetObj.transform.localPosition = new Vector3(0f, cameraHeight, 0f);
        targetObj.transform.localRotation = Quaternion.identity;
        return targetObj.transform;
    }

    private void followTarget() {
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position,
			target.position, step);
	}

	public static void ShowCursor() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public static void HideCursor() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
