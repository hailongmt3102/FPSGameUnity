using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 100f || Mathf.Abs(transform.position.y) > 100f || Mathf.Abs(transform.position.z) > 100f)
            Destroy(gameObject);
    }
}
