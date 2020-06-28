using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour {
    private float remainTime;
    [SerializeField] private float initRemainTime = 10.0f;
    [SerializeField] private Text remainTimeText;
    [SerializeField] private Text daysText;

    [SerializeField] private GameObject[] stages;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private AudioSource bgm;

    private void Awake() {
        GameData.Instance.Stage = 1;
        remainTime = initRemainTime * 60;
    }

    private void Start() {
        UIManager.Instance.Inventory.initialize();
    }

    private void Update() {
        bgm.volume = GameData.Instance.MusicVolume;

        if (remainTime < 0) {
            remainTime = 0f;
            this.battleManager.startBattle(this);
            return;
        }

        remainTime -= Time.deltaTime;
        int minutes = (int)(remainTime / 60);
        int seconds = (int)(remainTime % 60);
        remainTimeText.text = string.Format("{0}:{1}", minutes, seconds);
        daysText.text = string.Format("Day {0}", GameData.Instance.Stage);
    }

    public void elapsedDays() {
        ++GameData.Instance.Stage;
        remainTime = initRemainTime * 60;

        if (GameData.Instance.Stage >= 2) {
            stages[0]?.SetActive(true);
        }
        else if (GameData.Instance.Stage >= 4) {
            stages[1]?.SetActive(true);
        }
        else if (GameData.Instance.Stage >= 6) {
            stages[2]?.SetActive(true);
        }

        if (GameData.Instance.Stage == 7) remainTime *= 2f;
    }

    public void pause() {
        Time.timeScale = 0f;
    }

    public void resume() {
        Time.timeScale = 1f;
    }

    public void quit() {
        Application.Quit();
    }
}