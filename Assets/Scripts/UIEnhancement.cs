using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEnhancement : MonoBehaviour {
    [SerializeField] private GameObject buttons;

    private Coroutine enhanceCoroutine = null;
    [SerializeField] private Image arrow;
    [SerializeField] private Toggle skipAnimationToggle;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject cancelButton;

    private AudioSource enhanceSound = null;
    [SerializeField] private AudioClip progressSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    private EquipmentItem targetEquiment = null;
    [SerializeField] private Text successChance;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject ingredient;
    [SerializeField] private Text ingredientAmount;

    private void OnEnable() {
        UIManager.Instance.Inventory.gameObject.SetActive(true);
    }

    private void OnDisable() {
        buttons.SetActive(true);
        target.SetActive(false);
        ingredient.SetActive(false);

        this.stopEnhance();

        targetEquiment = null;
    }

    private void Start() {
        enhanceSound = GetComponent<AudioSource>();
    }

    private void Update() {
        if (targetEquiment == null) return;

        successChance.text = string.Format("강화확률: {0:0.0}%",
            this.calculateEnhanceChane(targetEquiment) * 100);
    }

    private void reset() {
        arrow.fillAmount = 0f;
        enhanceCoroutine = null;
        startButton.SetActive(true);
        cancelButton.SetActive(false);
    }

    private IEnumerator Enhance() {
        while (arrow.fillAmount < 1) {
            arrow.fillAmount += Time.deltaTime * 0.172f;

            yield return null;
        }

        this.enhance();
        this.reset();
    }

    public void startEnhance() {
        if (targetEquiment == null) {
            UIManager.Instance.InformHandler.showMessage("강화할 무기를 선택해주세요.");
            return;
        }

        if (targetEquiment.enhancingLevel == targetEquiment.maxEnhancingLevel) {
            UIManager.Instance.InformHandler.showMessage("선택한 무기는 최대 강화단계입니다.");
            return;
        }

        if (targetEquiment.durability < 10) {
            UIManager.Instance.InformHandler.showMessage("내구도가 부족합니다.");
            return;
        }

        int inventoryItemCount = Inventory.Instance.getItemCount(targetEquiment.enchantItemId);
        if (inventoryItemCount == 0) {
            UIManager.Instance.InformHandler.showMessage("재료가 부족합니다.");
            return;
        }

        if (skipAnimationToggle.isOn) {
            this.enhance();
            return;
        }

        startButton.SetActive(false);
        cancelButton.SetActive(true);

        enhanceSound.volume = GameData.Instance.FXVolume;
        enhanceSound.PlayOneShot(progressSound);
        enhanceCoroutine = StartCoroutine(Enhance());
    }

    public void stopEnhance() {
        if (enhanceCoroutine == null) return;

        StopCoroutine(enhanceCoroutine);
        this.reset();
        if (enhanceSound.isPlaying) enhanceSound.Stop();
    }

    public void selectItem(EquipmentItem item) {
        target.SetActive(true);
        ingredient.SetActive(true);

        var uiTarget = target.GetComponent<UIEquipmentItem>();
        if (uiTarget == null) uiTarget = target.AddComponent<UIEquipmentItem>();
        uiTarget.ItemData = item;
        targetEquiment = item;

        var uiIngredient = ingredient.GetComponent<UIItem>();
        if (uiIngredient == null) uiIngredient = ingredient.AddComponent<UIItem>();
        uiIngredient.ItemData = ItemDatabase.Instance.getItem(targetEquiment.enchantItemId);

        int inventoryItemCount = Inventory.Instance.getItemCount(targetEquiment.enchantItemId);
        ingredientAmount.text = string.Format("{0}/{1}",
            inventoryItemCount, 1);
    }

    private float calculateEnhanceChane(EquipmentItem item) {
        float chance = 100f;
        chance -= 5 * ((int)item.grade);
        chance -= 8.5f * ((int)item.enhancingLevel);
        return chance / 100;
    }

    private bool tryEnhance(float chance) {
        float random = Random.Range(0.0f, 1.0f);
        return random >= (1.0f - chance);
    }

    private void enhance() {
        enhanceSound.volume = GameData.Instance.FXVolume;

        float chance = this.calculateEnhanceChane(targetEquiment);
        if (this.tryEnhance(chance)) {
            UIManager.Instance.InformHandler.showMessage("강화 성공!");
            enhanceSound.PlayOneShot(successSound);
            targetEquiment.enhancingLevel += 1;
        }
        else {
            UIManager.Instance.InformHandler.showMessage("강화 실패...");
            enhanceSound.PlayOneShot(failSound);
            targetEquiment.durability -= 10;
            if (targetEquiment.enhancingLevel > 0) {
                targetEquiment.enhancingLevel -= 1;
            }
        }

        Inventory.Instance.removeItem(targetEquiment.enchantItemId, 1);
        int inventoryItemCount = Inventory.Instance.getItemCount(targetEquiment.enchantItemId);
        ingredientAmount.text = string.Format("{0}/{1}",
            inventoryItemCount, 1);
    }
}