using UnityEngine;
using UnityEngine.UI;

public class UIStatusWindow : MonoBehaviour {
    [SerializeField] private GameObject currentEquipment;
    [SerializeField] private Image currentEquipmentImage;
    [SerializeField] private Text enhancingLevel;
    [SerializeField] private Text winChance;
    [SerializeField] private GameRoot gameRoot;

    private void OnDisable() {
        UIManager.Instance.changeCursor(CursorType.Default);
    }

    private void Update() {
        var equipment = GameData.Instance.CurrentEquipment;
        if (equipment == null) {
            winChance.text = string.Format("전투 승리 확률: 0%");
            currentEquipment.SetActive(false);
            return;
        }
        
        currentEquipment.SetActive(true);
        
        currentEquipmentImage.sprite = ItemDatabase.Instance.getImage(equipment.id);
        enhancingLevel.text = string.Format("+{0}", equipment.enhancingLevel);
        winChance.text = string.Format("전투 승리 확률: {0:0.0}%", GameData.Instance.WinChance);
    }
}