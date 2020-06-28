using UnityEngine;
using UnityEngine.UI;

public class UIRepairment : MonoBehaviour {
    private EquipmentItem repairEquipment = null;

    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject repairTarget;
    [SerializeField] private GameObject repairIngredient;
    [SerializeField] private Text ingredientAmount;

    private void OnEnable() {
        UIManager.Instance.Inventory.gameObject.SetActive(true);
    }

    private void OnDisable() {
        buttons.SetActive(true);
        repairTarget.SetActive(false);
        repairIngredient.SetActive(false);

        repairEquipment = null;
    }

    private void setIngredientCount() {
        int inventoryItemCount = Inventory.Instance.getItemCount(repairEquipment.repairItemId);
        ingredientAmount.text = string.Format("{0}/{1}",
            inventoryItemCount, 1);
    }

    public void selectItem(EquipmentItem target) {
        repairTarget.SetActive(true);
        repairIngredient.SetActive(true);

        var uiTarget = repairTarget.GetComponent<UIEquipmentItem>();
        if (uiTarget == null) uiTarget = repairTarget.AddComponent<UIEquipmentItem>();
        uiTarget.ItemData = target;
        repairEquipment = target;

        var uiIngredient = repairIngredient.GetComponent<UIItem>();
        if (uiIngredient == null) uiIngredient = repairIngredient.AddComponent<UIItem>();
        uiIngredient.ItemData = ItemDatabase.Instance.getItem(target.repairItemId);

        this.setIngredientCount();
    }

    public void repair() {
        if (repairEquipment == null) {
            UIManager.Instance.InformHandler.showMessage("수리할 무기를 선택해주세요.");
            return;
        }

        if (repairEquipment.durability == repairEquipment.maxDurability) {
            UIManager.Instance.InformHandler.showMessage("내구도가 최대치입니다.");
            return;
        }

        if (Inventory.Instance.getItemCount(repairEquipment.repairItemId) == 0) {
            UIManager.Instance.InformHandler.showMessage("재료가 부족합니다.");
            return;
        }

        repairEquipment.durability += 10;
        Inventory.Instance.removeItem(repairEquipment.repairItemId, 1);

        this.setIngredientCount();
    }
}