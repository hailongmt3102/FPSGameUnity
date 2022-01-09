using UnityEngine;
using System.Collections;


public class PlayerInAIMode : MonoBehaviour
{
    private int maxHeath = 100;
    public int currentHeath = 100;

    [SerializeField]
    private CanvasInformation canvasInfor;

    public GameObject blood;

    public Transform playerCamera;
    public LayerMask hitMask;

    private RaycastHit hit;

    public AIManager aimanager;
    public PlayerMovement player;

    public GameObject Shield;

    private bool isShieldOn = false;
    private float shieldTime = 10;

    public GameObject ShieldEffectImage;

    private bool isDead = false;

    public GameObject diePanel;
    public GameObject playerInfor;

    private void Start()
    {
        if (canvasInfor == null) {
            Debug.LogError("Player in AI mode: No canvas infor reference");
        }
        if (aimanager == null) { 
            Debug.LogError("Player in AI mode: No Ai manager reference");

        }
    }
    private void Update()
    {
        if (isShieldOn) {
            shieldTime -= Time.deltaTime;
            if (shieldTime <= 0)
                CloseShield();
        }
    }

    public void UpdateBullet(int value) {
        canvasInfor.UpdateCurrentBullet(value);
    }

    [System.Obsolete]
    public void ReveiveDamage(int amount) {
        if (isDead) return;
        if (isShieldOn) {
            // no damaged when shied is openning
            ShieldEffect();
            return;
        }
        blood.SetActive(true);
        // update heath information
        canvasInfor.UpdateCurrentHeath((float)currentHeath / maxHeath);
        StartCoroutine(DamagedFinish());

        // update current heath
        currentHeath -= amount;
        if (currentHeath <= 0) {
            currentHeath = 0;
            Die();
            Debug.Log("die");
        }
    }

    IEnumerator DamagedFinish() {
        yield return new WaitForSeconds(1);
        blood.SetActive(false);
    }

    private void Die() {
        // disable some componenet
        player.enabled = false;
        playerInfor.SetActive(false);

        diePanel.SetActive(true);
        StartCoroutine(Respawn());

        isDead = true;
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(3);
        player.enabled = true;
        player.Respawn(new Vector3(0, 0.3f, 0));
        currentHeath = maxHeath;

        diePanel.SetActive(false);
        canvasInfor.UpdateCurrentBullet(7);
        canvasInfor.UpdateCurrentHeath(1);
        playerInfor.SetActive(true);

        // clear all enemy
        aimanager.Clear();

        isDead = false;
    }

    public void Shoot(int damage, float range) {
        // use raycast to check eny object 
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, hitMask))
        {
            if (hit.collider.tag == "Enemy") {
                hit.collider.GetComponent<StateMachine>().Damaged(damage);
            }
        }
    }
    private void ShieldEffect() {
        ShieldEffectImage.SetActive(true);
        StartCoroutine(CloseShieldEffect());
    }

    IEnumerator CloseShieldEffect() {
        yield return new WaitForSeconds(1);
        ShieldEffectImage.SetActive(false);
    }

    private void OpenShield() {
        shieldTime = 10f;
        isShieldOn = true;
        Shield.SetActive(true);
    }

    private void CloseShield() {
        isShieldOn = false;
        Shield.SetActive(false);
    }

    private void CollectHeathItem() {
        currentHeath = maxHeath;
        canvasInfor.UpdateCurrentHeath(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HeathItem")
        {
            // collision with heath item
            CollectHeathItem();
            aimanager.RemoveItem(other.gameObject);
        }
        else if (other.tag == "ShieldItem") {
            // collision with heath item
            OpenShield();
            aimanager.RemoveItem(other.gameObject);
        }

    }
}
