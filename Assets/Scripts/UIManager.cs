using UnityEngine;

public enum CursorType {
    Default,
    Pointer
}

public class UIManager : MonoBehaviour {
    private static UIManager instance = null;

    public static UIManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField] private UIInventory inventory;
    [SerializeField] private UIStatusWindow statusWindow;
    [SerializeField] private UIEnhancement enhancement;
    [SerializeField] private UIRepairment repairment;
    [SerializeField] private UICraftWindow craftWindow;
    [SerializeField] private UIToolTip toolTip;
    [SerializeField] private InformHandler informHandler;
     
    [Header("Cursor")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D pointerCursor;

    public UIInventory Inventory { get { return this.inventory; } }
    public UIStatusWindow StatusWindow { get { return this.statusWindow; } }
    public UIEnhancement Enhancement { get { return this.enhancement; } }
    public UIRepairment Repairment { get { return this.repairment; } }
    public UICraftWindow CraftWindow { get { return this.craftWindow; } }
    public UIToolTip ToolTip { get { return this.toolTip; } }
    public InformHandler InformHandler { get { return this.informHandler; } }

    public void changeCursor(CursorType type) {
        switch(type) {
            case CursorType.Default:
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.Pointer:
                Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}