using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICratableItemSlot : MonoBehaviour, IPointerClickHandler {
    private Text itemName = null;

    public Item ItemData { get; set; }
    public List<KeyValuePair<int, int>> NeedItems { get; } = new List<KeyValuePair<int, int>>();

    private void Start() {
        itemName = transform.Find("Name").GetComponent<Text>();
        itemName.text = ItemData.name;
    }

    public void OnPointerClick(PointerEventData eventData) {
        UIManager.Instance.CraftWindow.selectItem(ItemData, NeedItems);
    }
}