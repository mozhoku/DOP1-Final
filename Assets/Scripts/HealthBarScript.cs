using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset;
    private Enemy enemy_script;

    void Start() {
        enemy_script = GetComponent<BlueDemon>();
    }

    void Update()
    {
        slider.gameObject.SetActive(enemy_script.health < enemy_script.max_health);
        slider.value = CalculateHealth();
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);         
    }

    float CalculateHealth() {
        return enemy_script.health / enemy_script.max_health;
    }
}
