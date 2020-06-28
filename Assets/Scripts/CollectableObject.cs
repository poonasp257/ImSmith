using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CollectableObject : InteractableObject {
    private GameObject item = null;
    private Image progress = null;
    private Collider recognizeCollider = null;
    private Coroutine collectCoroutine = null;

    [SerializeField] private float respawnTime = 1.0f;
    [SerializeField] private float collectSpeed = 1.0f;

    [SerializeField] private int itemId;
    [SerializeField] private int maxDropAmount;

    protected override void Start() {
        base.Start();

        item = transform.Find("Item").gameObject;
        progress = ui.transform.Find("Progress").GetComponent<Image>();
        recognizeCollider = GetComponent<Collider>();
    }

    private MiscItem dropItem() {
        var item = ItemDatabase.Instance.createItem(this.itemId) as MiscItem;
        item.amount = Random.Range(1, maxDropAmount);
        return item;
    }

    private void resetObject() {
        progress.fillAmount = 1f;
        progress.gameObject.SetActive(false);
        player.controlAnimation(this.type, false);
        collectCoroutine = null;
    }

    private IEnumerator Respawn() {
        float timer = 0f;
        while (timer < respawnTime) {
            timer += Time.deltaTime;
            yield return null;
        }

        item.SetActive(true);
        recognizeCollider.enabled = true;
    }

    private IEnumerator Collect() {
        while (progress.fillAmount > 0f) {
            progress.fillAmount -= collectSpeed * Time.deltaTime;
            yield return null;
        }

        if (Inventory.Instance.IsFullSlots &&
            !Inventory.Instance.isExistStackableSlot(this.itemId)) {
            this.stopAction();
            yield break;
        }

        this.resetObject();
        Inventory.Instance.addItem(this.dropItem());

        item.SetActive(false);
        recognizeCollider.enabled = false;
        player.InteractableObject = null;
        actionSound.Stop();

        StartCoroutine(Respawn());
    }

    public override void doAction() {
        base.doAction();

        progress.gameObject.SetActive(true);
        player.controlAnimation(this.type, true);

        collectCoroutine = StartCoroutine(Collect());
    }

    public override void stopAction() {
        if (collectCoroutine == null) return;

        base.stopAction();

        StopCoroutine(collectCoroutine);

        this.resetObject();
    }
}
