using UnityEngine;
using UnityEngine.UI;

public class CanvasInformation : MonoBehaviour
{
    public Canvas canvas;
    public Text txtCurrentBullet;
    public Text yourkill;
    public Text enemykill;
    public Slider sldCurrentHeath;
    public void UpdateCurrentBullet(int amount)
    {
        txtCurrentBullet.text = amount.ToString() + "/7";
    }

    public void UpdateCurrentHeath(float value)
    {
        sldCurrentHeath.value = value;
    }

    public void UpdateLocalPlayerKill(int value) {
        yourkill.text = value.ToString();
    }

    public void UpdateRemotePlayerKill(int value)
    {
        enemykill.text = value.ToString();
    }

    public void Refresh() {
        txtCurrentBullet.text = "7/7";
        sldCurrentHeath.value = 1;
        yourkill.text = "0";
        enemykill.text = "0";
    }
}
