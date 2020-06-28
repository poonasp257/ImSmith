using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    protected Item item = null;
    protected Image image = null;
    protected Text valueText = null;

    public Item ItemData {
        get { return item; }
        set {
            item = value;

            if (image != null) {
                image.sprite = ItemDatabase.Instance.getImage(ItemData.id);
            }
            if (valueText != null) {
                valueText.text = string.Format("X{0}", ItemData.amount);
            }
        }
    }

    protected virtual void Start() {
        var imageNode = transform.Find("Image");
        if (imageNode != null) {
            image = imageNode.GetComponent<Image>();

            if (ItemData != null) {
                image.sprite = ItemDatabase.Instance.getImage(ItemData.id);
            }
        }

        var valueNode = transform.Find("Value");
        if (valueNode != null) {
            valueText = valueNode.GetComponent<Text>();
        }
    }

    protected void OnDisable() {
        if (UIManager.Instance == null) return;

        UIManager.Instance.ToolTip.hideToopTip(this);
    }

    protected virtual void Update() {
        if (item == null || item.amount == 0) return;

        valueText.text = string.Format("X{0}", ItemData.amount);
    }


    public virtual void OnPointerClick(PointerEventData eventData) {

    }

    public virtual void OnPointerEnter(PointerEventData eventData) {
        UIManager.Instance.changeCursor(CursorType.Pointer);
        UIManager.Instance.ToolTip.showToolTip(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        UIManager.Instance.changeCursor(CursorType.Default);
        UIManager.Instance.ToolTip.hideToopTip();
    }
}
