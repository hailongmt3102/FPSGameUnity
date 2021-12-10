using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 100f || Mathf.Abs(transform.position.y) > 100f || Mathf.Abs(transform.position.z) > 100f)
            Destroy(this);
    }
}
