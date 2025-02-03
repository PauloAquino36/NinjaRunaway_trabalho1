using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    PlayerControl player;

    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerControl>();
    }

    void Update()
    {
        slider.value = player.health;

        if (player.health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
