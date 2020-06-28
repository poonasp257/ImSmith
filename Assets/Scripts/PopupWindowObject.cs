using UnityEngine;

public class PopupWindowObject : InteractableObject {
    [SerializeField] private GameObject window;

    public override void doAction() {
        base.doAction();

        window.SetActive(true);
        CameraFollow.ShowCursor();
        CameraFollow.EnabledInput = false;
        player.controlAnimation(this.type, true);
    }

    public override void stopAction() {
        base.stopAction();

        window.SetActive(false);
        CameraFollow.HideCursor();
        CameraFollow.EnabledInput = true;
        player.controlAnimation(this.type, false);
    }
}