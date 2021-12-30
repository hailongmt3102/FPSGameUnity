using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInformation : NetworkBehaviour
{
    [SerializeField]
    private int maxHeath = 100;

    [SyncVar]
    private int currentHeath;

    [SyncVar]
    private int kill;

    [SyncVar]
    private int dead;

    public bool islocalPlayer;
    private CanvasInformation canvasInformation;

    private void Awake()
    {
        currentHeath = maxHeath;
        kill = dead = 0;
        canvasInformation = GameObject.Find("_playerInformation").GetComponent<CanvasInformation>();
    }

    public void TakeDamage(int amount) {
        currentHeath -= amount;
        if (currentHeath < 0) {
            currentHeath = 0;
            GameManager.AnyoneDead(transform.name);
        }
        canvasInformation.UpdateCurrentHeath((float)currentHeath / maxHeath);
    }

    public void GoodGame(bool iswin)
    {
        Debug.Log(transform.name + " win ? " + iswin.ToString());
        if (iswin)
        {
            kill += 1;
        }
        else {
            dead += 1;
        }
        if (islocalPlayer) {
            canvasInformation.UpdateLocalPlayerKill(kill);
            canvasInformation.UpdateRemotePlayerKill(dead);
        }
        Respawn();
    }

    private void Respawn() {
        Transform respawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnPoint.position;
        transform.rotation = Quaternion.identity;

        // update some variable
        transform.GetComponent<PlayerMovement>().Respawn();
        if (islocalPlayer) {
            canvasInformation.UpdateCurrentBullet(7);
            canvasInformation.UpdateCurrentHeath(1);
        }
    }

    public void UpdateCurrentBullet(int amount)
    {
        if (islocalPlayer)
            canvasInformation.UpdateCurrentBullet(amount);
    }
}
