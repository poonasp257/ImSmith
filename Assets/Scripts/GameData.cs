using UnityEngine;

public class GameData {
    private static GameData instance = null;

    public static GameData Instance {
        get {
            if (instance == null) {
                instance = new GameData();
            }

            return instance;
        }
    }

    public float FXVolume { get; set; } = 1f;

    public float MusicVolume { get; set; } = 1f;

    public EquipmentItem CurrentEquipment { get; set; }

    public float WinChance {
        get {
            if (CurrentEquipment == null) return 0f;
            float chance = CurrentEquipment.winChance + (10 + (int)CurrentEquipment.grade * 1.25f) 
                * CurrentEquipment.enhancingLevel - 3.5f * Stage;
            chance = Mathf.Clamp(chance, 0, 100f);
            return chance;
        }
    }
    public bool IsWin { get; set; }
    public int Stage { get; set; }
}