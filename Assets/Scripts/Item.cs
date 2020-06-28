using System;

public enum ItemType {
    Misc,
    Equipment
}

public enum ItemGrade {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class Item : ICloneable {
    public int id;
    public ItemType type;
    public ItemGrade grade;
    public string name;
    public string description;
    public bool isStackable;
    public int amount;

    public virtual object Clone() {
        return this.MemberwiseClone();
    }
}

[System.Serializable]
public sealed class MiscItem : Item {

}

[System.Serializable]
public sealed class EquipmentItem : Item {
    public int enhancingLevel;
    public int maxEnhancingLevel;
    public int durability;
    public int maxDurability;
    public float winChance;
    public int repairItemId;
    public int enchantItemId;
}