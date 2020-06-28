using UnityEngine;
using UnityEngine.EventSystems;

public class UICurrentEquimentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public virtual void OnPointerEnter(PointerEventData eventData) {
        UIManager.Instance.changeCursor(CursorType.Pointer);
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        UIManager.Instance.changeCursor(CursorType.Default);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left
            && eventData.clickCount == 2) {
            UIManager.Instance.Inventory.playEquipSound();
            Inventory.Instance.unequipItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            UIManager.Instance.Inventory.playEquipSound();
            Inventory.Instance.unequipItem();
        }
    }
}
