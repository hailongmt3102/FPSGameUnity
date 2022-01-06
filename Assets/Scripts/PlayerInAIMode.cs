using UnityEngine;
using System.Collections;


public class PlayerInAIMode : MonoBehaviour
{
    private int maxHeath = 100;
    public int currentHeath = 100;

    [SerializeField]
    private CanvasInformation canvasInfor;

    public GameObject blood;
    public GameObject diePannel;

    private void Start()
    {
        if (canvasInfor == null) {
            Debug.LogError("Player in AI mode: No canvas infor reference");
        }
    }

    public void UpdateBullet(int value) {
        canvasInfor.UpdateCurrentBullet(value);
    }

    public void ReveiveDamage(int amount) {
        currentHeath -= amount;
        if (currentHeath < 0) {
            currentHeath = 0;
            Die();
        }
        blood.SetActive(true);
        // update heath information
        canvasInfor.UpdateCurrentHeath((float)currentHeath / maxHeath);
    }

    IEnumerator DamagedFinish() {
        yield return new WaitForSeconds(1);
        blood.SetActive(false);
    }

    [System.Obsolete]
    private void Die() {
        // disable some componenet
        transform.GetComponent<PlayerMovement>().enabled = false;
        canvasInfor.gameObject.SetActive(false);

        diePannel.SetActive(true);
        StartCoroutine(Respawn());
    }

    [System.Obsolete]
    IEnumerator Respawn() {
        yield return new WaitForSeconds(3);
        PlayerMovement player = transform.GetComponent<PlayerMovement>();
        player.enabled = true;
        player.Respawn(new Vector3(0, 0.3f, 0));

        canvasInfor.UpdateCurrentBullet(7);
        canvasInfor.UpdateCurrentHeath(1);
        canvasInfor.gameObject.SetActive(true);
    }

    public void Shoot(int damage, float range) {
        // fire event
        Debug.Log("fire");
    }
}
