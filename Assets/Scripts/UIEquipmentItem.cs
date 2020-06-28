using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentItem : UIItem {
    protected override void Update() {
        var equipment = this.item as EquipmentItem;
        valueText.text = string.Format("+{0}", equipment.enhancingLevel);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        UIRepairment repairment = UIManager.Instance.Repairment;
        UIEnhancement enhancement = UIManager.Instance.Enhancement;

        var equipment = this.item as EquipmentItem;
        if (eventData.button == PointerEventData.InputButton.Left
            && eventData.clickCount == 2) {
            UIManager.Instance.Inventory.playEquipSound();

            if (repairment.gameObject.activeSelf) {
                repairment.selectItem(equipment);
                return;
            }
            else if (enhancement.gameObject.activeSelf) {
                enhancement.selectItem(equipment);
                return;
            }

            Inventory.Instance.equipItem(equipment);
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            UIManager.Instance.Inventory.playEquipSound();

            if (repairment.gameObject.activeSelf) {
                repairment.selectItem(equipment);
                return;

            }
            else if (enhancement.gameObject.activeSelf) {
                enhancement.selectItem(equipment);
                return;
            }

            Inventory.Instance.equipItem(equipment);
        }
    }
}
