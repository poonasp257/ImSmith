using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Inventory {
    private static Inventory instance = null;

    public static Inventory Instance {
        get {
            if (instance == null) {
                instance = new Inventory();
            }

            return instance;
        }
    }

    private int itemCount = 0;
    private int maxItemCount = 30;
    private List<Item> itemList = null;

    private List<Action<Item, int>> onAddedEvents = new List<Action<Item, int>>();
    private List<Action<int>> onRemovedEvents = new List<Action<int>>();

    public int MaxItemCount {
        get { return maxItemCount; }
    }

    public List<Item> ItemList {
        get { return itemList; }
    }

    public bool IsFullSlots {
        get { return itemCount == maxItemCount; }
    }

    private Inventory() {
        this.itemList = Enumerable.Repeat<Item>(null, maxItemCount).ToList();
    }

    public bool isExistStackableSlot(int itemId) {
        return this.findStackableItem(itemId) != null;
    }

    public int getItemCount(int itemId) {
        int count = 0;
        foreach (Item item in itemList) {
            if (item == null || item.id != itemId) continue;

            count += item.amount;
        }

        return count;
    }

    public Item findStackableItem(int itemId) {
        return itemList.Find((Item item) => {
            if (item == null || !item.isStackable) return false;
            return item.id == itemId;
        });
    }

    public void addItem(Item item) {
        int emptyIndex = itemList.FindIndex((Item element) => {
            return element == null;       
        });
        if (emptyIndex < 0) return;

        Item foundItem = this.findStackableItem(item.id);
        if (foundItem == null) {
            onAddedEvents[emptyIndex](item, emptyIndex);
            itemList[emptyIndex] = item;
            ++itemCount;
            return;
        }

        foundItem.amount += item.amount;
    }

    public void removeItem(Item removeItem) {
        int foundIndex = itemList.FindIndex((Item item) => item == removeItem);
        if (foundIndex < 0) return;

        this.onRemovedEvents[foundIndex](foundIndex);
        itemList[foundIndex] = null;
        --itemCount;
    }

    public void removeItem(int itemId, int amount) {
        int foundIndex = itemList.FindIndex((Item item) => {
            if (item == null) return false;
            return item.id == itemId;
        });
        if (foundIndex < 0) return;

        itemList[foundIndex].amount -= amount;
        if (itemList[foundIndex].amount <= 0) {
            this.onRemovedEvents[foundIndex](foundIndex);
            itemList[foundIndex] = null;
            --itemCount;
        }
    }

    public void registerAddItemEvent(Action<Item, int> action) {
        onAddedEvents.Add(action);
    }

    public void registerRemoveItemEvent(Action<int> action) {
        onRemovedEvents.Add(action);
    }

    public void sort() {
        itemList.Sort((lhs, rhs) => {
            if (lhs == null && rhs != null) return 1;
            else if (lhs != null && rhs == null) return -1;
            else if (lhs == null & rhs == null) return 0;

            if (lhs.id > rhs.id) return 1;
            else if (lhs.id < rhs.id) return -1;
            else return 0;
        });
    }

    public void equipItem(EquipmentItem item) {
        if (GameData.Instance.CurrentEquipment != null) {
            this.unequipItem();
        }

        this.removeItem(item);
        GameData.Instance.CurrentEquipment = item;
    }

    public void unequipItem() {
        this.addItem(GameData.Instance.CurrentEquipment);
        GameData.Instance.CurrentEquipment = null;
    }
}