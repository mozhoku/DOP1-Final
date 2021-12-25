using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float damage;
    public void GetDamaged(float damage2) {
        health -= damage2;
    }

}
