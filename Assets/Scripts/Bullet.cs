using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 dir;
    private float speed;
    private int damage;

    public LayerMask obstacleLayer;
    public void Setup(Vector3 dir, float speed, int damage) {
        this.dir = dir;
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.Translate(dir * Time.deltaTime * speed);

        if (Physics.Raycast(transform.position, transform.forward, 1, obstacleLayer)) {
            Destroy(gameObject);
        }
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Character") {
            other.GetComponent<PlayerInAIMode>().ReveiveDamage(damage);
        }
        Destroy(gameObject);
    }
}
