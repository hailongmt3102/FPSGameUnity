using UnityEngine;

[SerializeField]
public class Item {
    public Transform position;
    public GameObject item;

    public Item(Transform position) {
        this.position = position;
        item = null;
    }

    public Item(Transform position, GameObject item) {
        this.position = position;
        this.item = item;
    }

}