using UnityEngine;

public class KnightController : MonoBehaviour {
    private GameObject previousWeapon = null;
    private GameObject currentWeapon = null;
    private EquipmentItem currentItem = null;
    [SerializeField] private GameObject weapons;

    private void Update() {
        if (GameData.Instance.CurrentEquipment == null) {
            if (currentWeapon != null) {
                var currentAura = currentWeapon.transform.GetChild(0).gameObject;
                currentAura.SetActive(false);
            }

            currentWeapon?.SetActive(false);
            currentWeapon = null;
            currentItem = null;
            return;
        }

        if (currentItem != GameData.Instance.CurrentEquipment) {
            currentItem = GameData.Instance.CurrentEquipment;

            int index = currentItem.id - 100;
            previousWeapon = currentWeapon;
            currentWeapon = weapons.transform.GetChild(index).gameObject;

            if (previousWeapon != null) {
                var prevAura = previousWeapon.transform.GetChild(0).gameObject;
                previousWeapon.SetActive(false);
                prevAura.SetActive(false);
            }

            currentWeapon.SetActive(true);
        }

        if (currentItem == null) return;

        var aura = currentWeapon.transform.GetChild(0).gameObject;
        if (currentItem.enhancingLevel > 5) {
            aura.SetActive(true);
        }
        else {
            aura.SetActive(false);
        }
    }
}
