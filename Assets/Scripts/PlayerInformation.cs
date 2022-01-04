using System.Collections;
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

    private bool cheating = false;
    private LineRenderer lineCheating;
    public GameObject linePrefabs;

    public Camera playerCamera;

    public Vector3 startPos;
    private GameObject finishMatch;

    private GameObject bloodPanel;

    private void Awake()
    {
        currentHeath = maxHeath;
        kill = dead = 0;
        canvasInformation = GameObject.Find("_playerInformation").GetComponent<CanvasInformation>();
        finishMatch = GameObject.Find("ObjectReference").GetComponent<ObjectReference>().finishMatch;
        if (canvasInformation == null)
        {
            Debug.LogError("Player Information: No canvasInformation reference");
        }
        if (finishMatch == null) {
            Debug.LogError("Player Information: No finishmatch reference");
        }
        bloodPanel = GameObject.Find("ObjectReference").GetComponent<ObjectReference>().bloodPannel;
        if (bloodPanel == null) { 
            Debug.LogError("Player Information: No blood panel reference");
        }
        else
        {
            bloodPanel.SetActive(false);
        }
    }

    private void OpenCheatingMode() {
        cheating = true;
        if (lineCheating == null) {
            lineCheating = Instantiate(linePrefabs, transform.position, transform.rotation).GetComponent<LineRenderer>();
            lineCheating.transform.position = new Vector3(0, 1.8f, 0);
            lineCheating.transform.rotation = Quaternion.identity;
        }
    }

    private void CloseCheatingMode() {
        cheating = false;
        Destroy(lineCheating.gameObject);
        lineCheating = null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && islocalPlayer) {
            if (cheating) CloseCheatingMode(); 
            else OpenCheatingMode();
        }

        //if (Input.GetKeyDown(KeyCode.K)) {
        //    RpcTakeDamage(1000);
        //}

        if (cheating)
        {
            for (int i = 0; i < 2; i++)
            {
                PlayerInformation player = GameManager.getPlayerByIndex(i);
                Vector3 newpos;
                if (player != null)
                    newpos = player.transform.position;
                else
                    newpos = Vector3.zero;
                lineCheating.SetPosition(i, newpos);
            };
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount) {
        currentHeath -= amount;
        if (currentHeath < 0) {
            currentHeath = 0;
            GameManager.AnyoneDead(transform.name);
        }
        if (islocalPlayer)
        {
            canvasInformation.UpdateCurrentHeath((float)currentHeath / maxHeath);
            bloodPanel.SetActive(true);
            StartCoroutine("DamagedFinish");
        }
    }

    IEnumerator DamagedFinish() {
        yield return new WaitForSeconds(1);
        bloodPanel.SetActive(false);
    }

    [ClientRpc]
    public void RpcGoodGame(bool iswin)
    {
        if (!islocalPlayer) return;
        if (iswin)
        {
            kill += 1;
        }
        else {
            dead += 1;
        }
        canvasInformation.UpdateLocalPlayerKill(kill);
        canvasInformation.UpdateRemotePlayerKill(dead);

        // show finish UI
        ShowUIfinishMatch(iswin);
    }

    private void ShowUIfinishMatch(bool iswin) {
        StartCoroutine("Respawn");
        finishMatch.SetActive(true);
    }


    IEnumerator Respawn() {
        yield return new WaitForSeconds(2);
        currentHeath = maxHeath;
        transform.GetComponent<PlayerMovement>().Respawn(startPos);

        // update some variable on UI
        canvasInformation.UpdateCurrentBullet(7);
        canvasInformation.UpdateCurrentHeath(1);
        finishMatch.SetActive(false);
    }

    public void UpdateCurrentBullet(int amount)
    {
        if (islocalPlayer)
            canvasInformation.UpdateCurrentBullet(amount);
    }
}
