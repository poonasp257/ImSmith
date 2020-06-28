using UnityEngine;

public class Tutorials : MonoBehaviour {
    private int page = 0;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject nextSceneButton;

    private void OnDisable() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }

        page = 0;
        transform.GetChild(page).gameObject.SetActive(true);
    }

    private void Update() {
        if (page == 0) {
            prevButton.SetActive(false);
        }
        else prevButton.SetActive(true);

        if (page == transform.childCount - 1) {
            nextButton.SetActive(false);
            nextSceneButton?.SetActive(true);
        }
        else {
            nextButton.SetActive(true);
            nextSceneButton?.SetActive(false);
        }
    }

    public void prev() {
        if (page - 1 <= 0) return;

        transform.GetChild(page).gameObject.SetActive(false);
        transform.GetChild(page - 1).gameObject.SetActive(true);
        --page;
    }

    public void next() {
        if (transform.childCount <= page + 1) return;

        transform.GetChild(page).gameObject.SetActive(false);
        transform.GetChild(page + 1).gameObject.SetActive(true);
        ++page;
    }
}