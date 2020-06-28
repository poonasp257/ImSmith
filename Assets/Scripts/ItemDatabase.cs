using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;

public class ItemDatabase : MonoBehaviour {
    private static ItemDatabase instance = null;

    private Dictionary<int, Item> itemTable = new Dictionary<int, Item>();

    [Serializable]
    private class ItemImagePair {
        public int id;
        public Sprite sprite;
    }

    [SerializeField] private ItemImagePair[] itemImages;

    public static ItemDatabase Instance {
        get {
            if (instance) return instance;

            instance = FindObjectOfType<ItemDatabase>();
            return instance;
        }
    }

    private void Awake() {
        string jsonData = Util.LoadJsonFile("/Data", "/item");
        JObject jObject = JObject.Parse(jsonData);

        foreach (var pair in jObject) {
            int itemId = int.Parse(pair.Key);
            Item itemData = generateItemData(pair.Value);
            itemData.id = itemId;
            itemTable.Add(itemId, itemData);
        }
    }

    private Item generateItemData(JToken jsonData) {
        ItemType itemType = jsonData["type"].ToObject<ItemType>();
        switch (itemType) {
            case ItemType.Misc:
                var miscItem = jsonData.ToObject<MiscItem>();
                miscItem.isStackable = true;
                return miscItem;
            case ItemType.Equipment:
                var equipmentItem = jsonData.ToObject<EquipmentItem>();
                equipmentItem.isStackable = false;
                equipmentItem.durability = equipmentItem.maxDurability;
                return equipmentItem;
            default:
                return null;
        }
    }

    public Sprite getImage(int itemId) {
        ItemImagePair foundPair = itemImages.First((pair) => pair.id == itemId);
        if (foundPair == null) return null;

        return foundPair.sprite;
    }

    public Item getItem(int itemId) {
        if (!itemTable.ContainsKey(itemId)) {
            return null;
        }

        return itemTable[itemId];
    }

    public Item createItem(int itemId) {
        if (!itemTable.ContainsKey(itemId)) {
            return null;
        }

        return itemTable[itemId].Clone() as Item;
    }

    public static string getGradeName(ItemGrade grade) {
        switch(grade) {
            case ItemGrade.Common:
                return "일반 등급";
            case ItemGrade.Uncommon:
                return "고급 등급";
            case ItemGrade.Rare:
                return "희귀 등급";
            case ItemGrade.Epic:
                return "에픽 등급";
            case ItemGrade.Legendary:
                return "전설 등급";
            default:
                return "Unknown";
        }
    }

    public static string getTypeName(ItemType type) {
        switch (type) {
            case ItemType.Misc:
                return "기타 아이템";
            case ItemType.Equipment:
                return "장착 아이템";
            default:
                return "Unknown";
        }
    }
}
