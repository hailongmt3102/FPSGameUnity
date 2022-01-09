using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject about;
    public GameObject tut;

    private void Start()
    {
        if (menu != null && about != null && tut != null)
        {
            menu.SetActive(true);
            about.SetActive(false);
            tut.SetActive(false);
        }
    }

    public void BackToMenu() {
        menu.SetActive(true);
        about.SetActive(false);
        tut.SetActive(false);
    }

    public void GoToAbout() {
        menu.SetActive(false);
        about.SetActive(true);
        tut.SetActive(false);
    }

    public void GoToTutorial() {
        menu.SetActive(false);
        about.SetActive(false);
        tut.SetActive(true);
    }

    public void Quit() {
        Application.Quit();
    }

    public void TwoPlayer()
    {
        SceneManager.LoadScene(2);
    }

    public void OnePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void MenuScene() {
        SceneManager.LoadScene(0);
    }
}
