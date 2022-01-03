using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform playerRef;

    private Vector3 newPos;

    private void Update()
    {
        if (playerRef != null) {
            newPos = playerRef.position;
            newPos.y = transform.position.y;
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(90f, playerRef.eulerAngles.y, 0f);
        }
    }
}
