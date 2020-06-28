using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {
    [SerializeField] private GameObject winBackground;
    [SerializeField] private GameObject loseBackground;
    [SerializeField] private Text endingMessage;
    [SerializeField] private string winMessage;
    [SerializeField] private string loseMessage;

    private void Start() {
        if (GameData.Instance.IsWin) {
            winBackground.SetActive(true);
            endingMessage.text = winMessage;
        }
        else {
            loseBackground.SetActive(true);
            endingMessage.text = loseMessage;
        }
    }

    private void Update() {
        endingMessage.transform.position += Vector3.up * Time.deltaTime * 50.0f;
    }
}