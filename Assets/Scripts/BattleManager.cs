using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {
    private bool isWin = false;
    private GameRoot gameRoot = null;

    [SerializeField] private GameObject progress;
    [SerializeField] private Image progressbar;

    [SerializeField] private GameObject result;
    [SerializeField] private Text description;
    [SerializeField] private Transform rewardsNode;

    private void OnDisable() {
        progress.SetActive(false);
        result.SetActive(false);

        foreach (Transform slot in rewardsNode) {
            slot.GetChild(0).gameObject.SetActive(false);
        }

        if (GameData.Instance.Stage == 7) {
            GameData.Instance.CurrentEquipment = null;
            GameData.Instance.IsWin = this.isWin;
            SceneManager.LoadScene("Result");
            return;
        }

        gameRoot.elapsedDays();
    }

    private IEnumerator Battle() {
        progressbar.fillAmount = 0f;
        while (progressbar.fillAmount < 1f) {
            progressbar.fillAmount += Time.deltaTime * 0.25f;
            yield return null;
        }

        this.progress.SetActive(false);
        this.result.SetActive(true);

        this.isWin = getBattleResult();
        if (!isWin) {
            description.text = "용사는 마물과의 전투에서 패배했습니다.";
            yield break;
        }

        description.text = "용사는 마물과의 전투에서 승리하여 전리품을 획득했습니다.";
        List<Item> rewards = this.getRewards();
        for (int i = 0; i < rewards.Count; ++i) {
            Transform slot = rewardsNode.GetChild(i);
            UIItem uiItem = slot.GetChild(0).GetComponentInChildren<UIItem>();
            uiItem.gameObject.SetActive(true);
            uiItem.ItemData = rewards[i];

            Item reward = ItemDatabase.Instance.createItem(rewards[i].id);
            reward.amount = rewards[i].amount;
            Inventory.Instance.addItem(reward);
        }
    }

    private List<Item> getRewards() {
        List<int> itemsId = new List<int>(){ 0, 6, 5, 1, 2, 3, 4 };
        List<Item> rewards = new List<Item>();
        for (int i = 0; i <= GameData.Instance.Stage; ++i) {
            int stage = Mathf.Clamp(GameData.Instance.Stage, GameData.Instance.Stage + 1, 7);
            int itemId = itemsId[Random.Range(0, stage)];
            Item reward = ItemDatabase.Instance.createItem(itemId);
            reward.amount = Random.Range(5, GameData.Instance.Stage * 7);
            rewards.Add(reward);
        }

        return rewards;
    }

    private bool getBattleResult() {
        float random = Random.Range(0.0f, 1.0f);
        return random >= (1.0f - GameData.Instance.WinChance / 100);
    }

    public void startBattle(GameRoot gameRoot) {
        if (this.gameObject.activeSelf) return;

        this.gameRoot = gameRoot;
        this.gameObject.SetActive(true);
        this.progress.SetActive(true);
        StartCoroutine(Battle());
    }    
}