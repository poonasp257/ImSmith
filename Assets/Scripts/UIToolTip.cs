using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIToolTip : MonoBehaviour {
    private UIItem currentITem = null;
    private AudioSource enableSound = null;

    [SerializeField] private Text itemName;
    [SerializeField] private Text grade;
    [SerializeField] private Text type;
    [SerializeField] private Text description;
    [SerializeField] private Image image;
    [SerializeField] private GameObject durability;
    [SerializeField] private Text durabilityValue;
    [SerializeField] private Image durabilityBar;

    public Item ItemData { get; set; }

    private void OnEnable() {
        if (enableSound == null) {
            enableSound = GetComponent<AudioSource>();
        }

        enableSound.volume = GameData.Instance.FXVolume;
        enableSound.Play();

        itemName.text = ItemData.name;
        grade.text = ItemDatabase.getGradeName(ItemData.grade);
        type.text = ItemDatabase.getTypeName(ItemData.type);
        description.text = string.Format("설명: {0}", ItemData.description);
        image.sprite = ItemDatabase.Instance.getImage(ItemData.id);

        if (ItemData.type != ItemType.Equipment) {
            return;
        }

        durability.SetActive(true);

        EquipmentItem equipment = ItemData as EquipmentItem;
        durabilityValue.text = string.Format("내구도 {0}/{1}",
            equipment.durability, equipment.maxDurability);
        durabilityBar.fillAmount = (float)equipment.durability / (float)equipment.maxDurability;
    }

    private void OnDisable() {
        durability.SetActive(false);
        UIManager.Instance.changeCursor(CursorType.Default);
    }

    private void adjustPosition(RectTransform slotTransform) {
        float screenHalfWidth = Screen.width * 0.5f;
        float screenHalfHeight = Screen.height * 0.5f;

        RectTransform toolTipTransform = this.transform as RectTransform;
        Vector2 newPivot, slotPos = slotTransform.position;
        newPivot.x = slotPos.x > screenHalfWidth ? 1 : 0;
        newPivot.y = slotPos.y > screenHalfHeight ? 1 : 0;
        toolTipTransform.pivot = newPivot;

        float slotHalfWidth = slotTransform.rect.width * 0.5f;
        Vector2 newPos = slotPos;
        newPos.x += slotPos.x > screenHalfWidth ? -slotHalfWidth : slotHalfWidth;
        toolTipTransform.position = newPos;
    }

    public void showToolTip(UIItem uiItem) {
        adjustPosition(uiItem.transform as RectTransform);

        ItemData = uiItem.ItemData;
        currentITem = uiItem;
        gameObject.SetActive(true);
    }

    public void hideToopTip() {
        currentITem = null;
        ItemData = null;
        gameObject.SetActive(false);
    }

    public void hideToopTip(UIItem uiItem) {
        if (currentITem != uiItem) return;

        this.hideToopTip();
    }
}