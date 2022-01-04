using UnityEngine;

public class PlayerInAIMode : MonoBehaviour
{
    private int maxHeath = 100;
    public int currentHeath = 100;

    [SerializeField]
    private CanvasInformation canvasInfor;

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
        // update heath information
        canvasInfor.UpdateCurrentHeath((float)currentHeath / maxHeath);
    }

    private void Die() {
        Debug.Log("die");
    }

    public void Shoot(int damage, float range) {
        // fire event
        Debug.Log("fire");
    }
}
