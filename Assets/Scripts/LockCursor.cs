using UnityEngine;

public class LockCursor : MonoBehaviour
{
    public GameObject[] activeWhenCursorRelease;
    public GameObject[] activeWhenCursorSetup;

    public bool ReleaseOnStart = true;

    private bool islocked;
    private void Start()
    {
        if (ReleaseOnStart)
            LockCursorRelease();
        else
            LockCursorSetup();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (islocked)
            {
                LockCursorRelease();
            }
            else {
                LockCursorSetup();
            }
        }
    }
    public void LockCursorSetup()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject gameObject in activeWhenCursorRelease)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in activeWhenCursorSetup)
        {
            gameObject.SetActive(true);
        }
        islocked = true;
    }

    public void LockCursorRelease()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        foreach (GameObject gameObject in activeWhenCursorRelease) {
            gameObject.SetActive(true);
        }
        foreach (GameObject gameObject in activeWhenCursorSetup)
        {
            gameObject.SetActive(false);
        }
        islocked = false;
    }
}
