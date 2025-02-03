using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    PlayerControl player;

    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.health;
    }
}
