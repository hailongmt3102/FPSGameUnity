using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[RequireComponent(typeof(PlayerInformation))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(PlayerInformation))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] disableBehavior;

    [SerializeField]
    private GameObject[] disableGameObject;

    private Camera SceneCamera;

    private string RemoteLayerName = "RemotePlayer";

    public GameObject crosshair;

    private GameObject crosshairInstance;

    private void Start()
    {
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
        }
        gameObject.GetComponent<PlayerInformation>().islocalPlayer = isLocalPlayer;
    }

    public override void OnStartClient()
    {
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerInformation player = GetComponent<PlayerInformation>();
        GameManager.RegisterPlayer(netID, player);
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
    }


    private void OnDisable()
    {
        // when opponents leave game or founded winner, set camera activation
        if (SceneCamera != null)
            SceneCamera.gameObject.SetActive(true);

        GameManager.RemovePlayer(transform.name);
        Destroy(crosshairInstance);
    }
}
