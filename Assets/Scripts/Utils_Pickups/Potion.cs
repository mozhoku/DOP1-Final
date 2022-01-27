using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().health += 20.0f;
            Destroy(this.gameObject);
        }
    }
}
