using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] disableBehavior;

    [SerializeField]
    private GameObject[] disableGameObject;

    private Camera SceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour item in disableBehavior)
            {
                item.enabled = false;
            }

            foreach (GameObject obj in disableGameObject)
            {
                obj.SetActive(false);
            }
        }
        else {
            SceneCamera = Camera.main;
            SceneCamera.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // when opponents leave game or founded winner, set camera activation
        if (SceneCamera != null)
            SceneCamera.gameObject.SetActive(true);
    }
}
