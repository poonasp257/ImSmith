using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventory : MonoBehaviour {
    private List<Transform> slotList = new List<Transform>();
    private List<UIItem> itemList = null;

    [SerializeField] private Transform uiInventoryNode;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject equipmentItemPrefab;
    [SerializeField] private GameObject miscItemPrefab;

    private AudioSource equipSound = null;

    public void OnEnable() {
        this.sort();
    }

    private void Start() {
        equipSound = GetComponent<AudioSource>();
    }

    public void initialize() {
        this.initializeItemSlots(Inventory.Instance);

        //EquipmentItem item = ItemDatabase.Instance.createItem(100) as EquipmentItem;
        //item.enhancingLevel = 7;
        //Inventory.Instance.addItem(item);

        //item = ItemDatabase.Instance.createItem(101) as EquipmentItem;
        //item.enhancingLevel = 7;
        //Inventory.Instance.addItem(item);

        //item = ItemDatabase.Instance.createItem(102) as EquipmentItem;
        //item.enhancingLevel = 7;
        //Inventory.Instance.addItem(item);

        //item = ItemDatabase.Instance.createItem(103) as EquipmentItem;
        //item.enhancingLevel = 7;
        //Inventory.Instance.addItem(item);

        //item = ItemDatabase.Instance.createItem(104) as EquipmentItem;
        //item.enhancingLevel = 7;
        //Inventory.Instance.addItem(item);

        //item = ItemDatabase.Instance.createItem(5);
        //item.amount = 999;
        //Inventory.Instance.addItem(item);
    }

    private void initializeItemSlots(Inventory inventory) {
        itemList = Enumerable.Repeat<UIItem>(null, inventory.MaxItemCount).ToList();

        for (int i = 0; i < inventory.MaxItemCount; ++i) {
            slotList.Add(Instantiate(slotPrefab, uiInventoryNode).transform);

            inventory.registerAddItemEvent((Item item, int index) => {
                itemList[index] = this.createItem(item, slotList[index]);
            });
            inventory.registerRemoveItemEvent((int index) => {
                this.removeItem(slotList[index]);
                itemList[index] = null;
            });
        }
    }

    private UIItem createItem(Item item, Transform slot) {
        GameObject itemObject = null;
        switch (item.type) {
            case ItemType.Equipment:
                itemObject = Instantiate(equipmentItemPrefab, slot);
                var equipment = itemObject.AddComponent<UIEquipmentItem>();
                equipment.ItemData = item as EquipmentItem;
                return equipment;
            default:
                itemObject = Instantiate(miscItemPrefab, slot);
                var defaultItem = itemObject.AddComponent<UIItem>();
                defaultItem.ItemData = item;
                return defaultItem;
        }
    }

    private void removeItem(Transform slot) {
        int itemIndex = slot.transform.childCount - 1;
        Transform item = slot.transform.GetChild(itemIndex);
        Destroy(item.gameObject);
    }

    public void sort() {
        if (itemList == null) return;

        Inventory.Instance.sort();

        itemList.Sort((lhs, rhs) => {
            if (lhs == null && rhs != null) return 1;
            else if (lhs != null && rhs == null) return -1;
            else if (lhs == null & rhs == null) return 0;

            if (lhs.ItemData.id > rhs.ItemData.id) return 1;
            else if (lhs.ItemData.id < rhs.ItemData.id) return -1;
            else return 0;
        });

        for (int i = 0; i < itemList.Count; ++i) {
            if (itemList[i] == null) continue;

            int oldSlotIndex = itemList[i].transform.parent.GetSiblingIndex();
            itemList[i].transform.parent.SetSiblingIndex(i);
        }

        for (int i = 0; i < uiInventoryNode.childCount; ++i) {
            slotList[i] = uiInventoryNode.GetChild(i);
        }
    }

    public void playEquipSound() {
        if (equipSound == null) return;

        equipSound.volume = GameData.Instance.FXVolume;
        equipSound.Play();
    }
}