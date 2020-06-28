using UnityEngine;

public class Minimap : MonoBehaviour {
    private Transform player = null;

    private void Start() {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        Vector3 playerPos = player.position;
        playerPos.y = this.transform.position.y;
        this.transform.position = playerPos;
    }
}