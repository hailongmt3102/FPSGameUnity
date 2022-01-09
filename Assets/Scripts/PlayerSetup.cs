using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[RequireComponent(typeof(PlayerInformation))]
[RequireComponent(typeof(NetworkIdentity))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] disableBehavior;

    [SerializeField]
    private GameObject[] disableGameObject;

    [SerializeField]
    private GameObject[] disableLocalComponents;

    private Camera SceneCamera;

    private string RemoteLayerName = "RemotePlayer";

    public GameObject crosshair;

    private GameObject crosshairInstance;

    // canvas have information of player
    private CanvasInformation playerInformation;

    private Vector3 startPos;
    private GameObject ExitBtn;

    private GameObject EXIT;

    private void Start()
    {
        ExitBtn = GameObject.Find("EXIT");
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else {
            SceneCamera = Camera.main;
            SceneCamera.gameObject.SetActive(false);
            // create crosshair
            crosshairInstance = Instantiate(crosshair);
            DisableLocalObject();
            ActiveMinimapRef();
            // disable exit button
            if (ExitBtn != null)
                ExitBtn.SetActive(false);
        }
        gameObject.GetComponent<PlayerInformation>().islocalPlayer = isLocalPlayer;

        playerInformation = GameObject.Find("_playerInformation").GetComponent<CanvasInformation>();
        if (playerInformation == null)
        {
            Debug.LogError("Player setup : Can't find information canvas");
        }
        else {
            // anable information canvas and refresh all value.
            playerInformation.Refresh();
            playerInformation.canvas.gameObject.SetActive(true);
        }
    }

    private void ActiveMinimapRef() {
        try { 
            GameObject.Find("MinimapCamera").GetComponent<Minimap>().playerRef = transform;
        }catch{
            Debug.LogError("Player setup: No 'MinimapCamera' object");
        }
    }

    public override void OnStartClient()
    {
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerInformation player = GetComponent<PlayerInformation>();
        startPos = GameManager.RegisterPlayer(netID, player);
        transform.GetComponent<PlayerInformation>().startPos = startPos;

        GameObject.Find("LockCursor").GetComponent<LockCursor>().LockCursorSetup();
        EXIT = GameObject.Find("EXIT");
        EXIT.SetActive(false);
    }

    private void DisableLocalObject() {
        foreach (GameObject obj in disableLocalComponents) {
            obj.SetActive(false);
        }
    }


    private void DisableComponents() {
        foreach (Behaviour item in disableBehavior)
        {
            item.enabled = false;
        }

        foreach (GameObject obj in disableGameObject)
        {
            obj.SetActive(false);
        }
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
        foreach (Transform child in transform.GetComponentsInChildren<Transform>()) 
        {
            child.gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
        }
    }


    private void OnDisable()
    {
        // when opponents leave game or founded winner, set camera activation
        if (SceneCamera != null)
            SceneCamera.gameObject.SetActive(true);

        GameManager.RemovePlayer(transform.name);
        Destroy(crosshairInstance);
        playerInformation.canvas.gameObject.SetActive(false);

        // enable exit button
        if (ExitBtn != null)
            ExitBtn.SetActive(true);
        GameObject.Find("LockCursor").GetComponent<LockCursor>().LockCursorRelease();
        EXIT.SetActive(true);
    }
}
