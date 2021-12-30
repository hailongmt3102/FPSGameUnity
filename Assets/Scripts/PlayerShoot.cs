using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Transform cam;

    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        if (cam == null) {
            Debug.LogError("Player shoot: No camera reference");
            enabled = false;
        }
    }

    [Client]
    public void Shoot(int damage, float range) {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask)) {
            if (hit.collider.tag == "Player") {
                CmdPlayerShoot(hit.collider.name, damage);
            }
            Debug.Log("hit " + hit.collider.name);
        }
    }

    [Command]
    private void CmdPlayerShoot(string Id, int damage) {
        PlayerInformation player = GameManager.getPlayer(Id);
        player.TakeDamage(damage);
    }
}
