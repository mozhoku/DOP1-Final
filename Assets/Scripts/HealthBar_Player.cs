using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Player : MonoBehaviour
{
    public Slider slider;
    public PlayerController player_script;


    void Start() {

    }

    void Update()
    {
        //slider.gameObject.SetActive(player_script.health < player_script.max_health);
        slider.value = CalculateHealth();
        //slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);         
    }

    float CalculateHealth() {
        return player_script.health / player_script.max_health;
    }
}
