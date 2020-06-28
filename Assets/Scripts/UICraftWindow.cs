using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class UICraftWindow : MonoBehaviour {
    private Item selectedItem = null;
    [SerializeField] private GameObject selectedItemNode;

    private List<KeyValuePair<int, int>> needItemList = null;
    [SerializeField] private Transform needItemListNode;

    [SerializeField] private Transform cratableItemListNode;
    [SerializeField] private GameObject cratableItemPrefab;

    private void OnEnable() {
        UIManager.Instance.Inventory.gameObject.SetActive(true);
    }

    private void OnDisable() {
        selectedItem = null;
        selectedItemNode.SetActive(false);
        foreach(Transform slot in needItemListNode) {
            var itemInSlot = slot.GetChild(0);
            itemInSlot.gameObject.SetActive(false);
        }

        needItemList = null;
    }

    private void Start() {
        this.initializeCraftableItemList();
    }

    private void initializeCraftableItemList() {
        string jsonData = Util.LoadJsonFile("/Data", "/item_craft");
        JObject jObject = JObject.Parse(jsonData);

        foreach (var pair in jObject) {
            int craftableItemId = int.Parse(pair.Key);
            var craftableItem = Instantiate(cratableItemPrefab, cratableItemListNode);
            var craftableItemSlot = craftableItem.AddComponent<UICratableItemSlot>();
            craftableItemSlot.ItemData = ItemDatabase.Instance.createItem(craftableItemId);

            JArray needs = pair.Value["needs"].ToObject<JArray>();
            foreach (var needItem in needs) {
                int itemId = needItem["id"].ToObject<int>();
                int amount = needItem["amount"].ToObject<int>();
                craftableItemSlot.NeedItems.Add(new KeyValuePair<int, int>(itemId, amount));
            }
        }
    }

    private void updateNeedItemList(List<KeyValuePair<int, int>> needItems) {
        for (int i = 0; i < needItemListNode.childCount; ++i) {
            var slot = needItemListNode.GetChild(i);
            var itemInSlot = slot.GetChild(0);
            if (i >= needItems.Count) {
                itemInSlot.gameObject.SetActive(false);
                continue;
            }

            var uiItem = itemInSlot.GetComponent<UIItem>();
            uiItem.ItemData = ItemDatabase.Instance.getItem(needItems[i].Key);
            
            var amount = itemInSlot.GetComponentInChildren<Text>();
            int inventoryItemCount = Inventory.Instance.getItemCount(needItems[i].Key);
            amount.text = string.Format("{0}/{1}", inventoryItemCount, needItems[i].Value);

            itemInSlot.gameObject.SetActive(true);
        }

        this.needItemList = needItems;
    }

    public void selectItem(Item item, List<KeyValuePair<int, int>> needItems) {
        var uiItem = selectedItemNode.GetComponent<UIItem>();
        uiItem.ItemData = item;
        selectedItem = item;
        selectedItemNode.SetActive(true);

        Image image = selectedItemNode.GetComponentInChildren<Image>();
        Text description = selectedItemNode.GetComponentInChildren<Text>();
        image.sprite = ItemDatabase.Instance.getImage(item.id);
        description.text = item.description;

        this.updateNeedItemList(needItems);
    }

    public void craftItem() {
        if (selectedItem == null) {
            UIManager.Instance.InformHandler.showMessage("제작할 아이템을 선택해주세요.");
            return;
        }

        foreach (var needItem in needItemList) {
            var foundItem = Inventory.Instance.findStackableItem(needItem.Key);
            if (foundItem == null || foundItem.amount < needItem.Value) {
                UIManager.Instance.InformHandler.showMessage("재료가 부족합니다.");
                return;
            }
        }

        foreach (var needItem in needItemList) {
            Inventory.Instance.removeItem(needItem.Key, needItem.Value);
        }

        var newItem = ItemDatabase.Instance.createItem(selectedItem.id);
        newItem.amount = 1;
        Inventory.Instance.addItem(newItem);

        this.updateNeedItemList(needItemList);
    }
}