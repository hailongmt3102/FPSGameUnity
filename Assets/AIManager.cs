using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private List<GameObject> enemies;

    private bool isWating = false;
    public int createTime = 10;
    public int maxEnemy = 3;
    public GameObject enemyPrefab;
    public Transform startPosCreation;

    private void Start()
    {
        enemies = new List<GameObject>();
    }


    public void Clear() {
        enemies.Clear();
    }

    public void Remove(GameObject gameObject) {
        enemies.Remove(gameObject);
    }

    private void Create() {
        GameObject newEnemy = Instantiate(enemyPrefab, startPosCreation.position, Quaternion.identity);
        enemies.Add(newEnemy);
        isWating = true;
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
}
