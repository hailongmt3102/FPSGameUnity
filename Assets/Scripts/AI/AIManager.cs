using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private List<GameObject> enemies;

    private bool isWating = false;
    public int createTime = 30;
    public int maxEnemy = 3;
    public GameObject enemyPrefab;
    public Transform startPosCreation;
    public GameObject Explosion;


    public float createItemTime = 30;

    // list of item
    private Item[] items;

    public Transform[] ListItemStartPosition;
    public GameObject heathItemPrefab;
    public GameObject shieldItemPrefab;

    private void Start()
    {
        enemies = new List<GameObject>();

        // create new dynamic array
        items = new Item[ListItemStartPosition.Length];
        // appen all start position to items
        int i = 0;
        foreach (Transform startPos in ListItemStartPosition) {
            items[i++] = new Item(startPos);
        }
        StartCoroutine(CreateItemWating());
    }


    public void Clear() {

        foreach (GameObject enemy in enemies) {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        enemies.Clear();
        Create();
    }

    public void Remove(GameObject gameObject) {
        // create explosion effect and destroy affter 1s
        Destroy(Instantiate(Explosion, gameObject.transform.position, Quaternion.identity), 1);
        enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    private void Create() {
        GameObject newEnemy = Instantiate(enemyPrefab, startPosCreation.position, Quaternion.identity);
        enemies.Add(newEnemy);
        isWating = true;
        StartCoroutine(Created());
    }

    IEnumerator Created() {
        yield return new WaitForSeconds(createTime);
        isWating = false;
    }

    void Update()
    {
        if (!isWating && enemies.Count < maxEnemy) {
            Create();
        }
    }

    IEnumerator CreateItemWating() {
        yield return new WaitForSeconds(createItemTime);
        CreateNewItem();
    }

    private void CreateNewItem() {
        foreach (Item item in items) {
            if (item.item == null) {
                // can add new gameObject here
                // random two item 
                int i = Random.Range(0, 2);
                switch (i)
                {
                    case 0:
                        item.item = Instantiate(heathItemPrefab, item.position.position, Quaternion.identity);
                        break;
                    case 1:
                        item.item = Instantiate(shieldItemPrefab, item.position.position, Quaternion.identity);
                        break;
                    default:
                        break;
                }
                break;
            }
        }
        StartCoroutine(CreateItemWating());
    }

    public void RemoveItem(GameObject gameObject) {
        foreach (Item item in items) {
            if (item.item == gameObject) {
                Destroy(item.item);
                item.item = null;
                return;
            }
        }
        Debug.Log("AI manager on remove Item: Item not found");
    }
}
