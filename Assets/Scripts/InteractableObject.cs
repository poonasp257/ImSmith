using UnityEngine;

public enum InteractType {
    None,
    Mining,
    Gathering,
    Hammering
}

public class InteractableObject : MonoBehaviour {
    protected AudioSource actionSound;
    protected GameObject ui = null;
    protected GameObject uiIcon = null;

    [SerializeField] protected InteractType type;

    protected PlayerController player = null;

    protected virtual void Start() {
        actionSound = GetComponent<AudioSource>();
        uiIcon = transform.Find("Interaction Icon/Icons").gameObject;
        ui = uiIcon.transform.parent.gameObject;
        ui.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.GetComponent<PlayerController>();
            player.InteractableObject = this;
            ui.SetActive(true);
            uiIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            player.InteractableObject = null;
            player = null;
            ui.SetActive(false);
        }
    }

    public virtual void doAction() {
        if (player == null) return;

        uiIcon.SetActive(false);
        Quaternion lookAngle = Quaternion.LookRotation(
            transform.position - player.transform.position);
        player.transform.rotation = new Quaternion(0, lookAngle.y, 0, lookAngle.w);
        actionSound.volume = GameData.Instance.FXVolume;
        actionSound.Play();
    }

    public virtual void stopAction() {
        uiIcon.SetActive(true);
        actionSound.Stop();
    }
}