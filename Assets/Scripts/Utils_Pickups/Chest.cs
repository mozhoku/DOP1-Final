using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chest : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent LootFromThisChest;
    public UnityEngine.Events.UnityEvent CheckIfLooted;
    // Start is called before the first frame update

    void Start() {
        CheckIfLooted.Invoke();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            LootFromThisChest.Invoke();
            GetComponent<Animator>().SetTrigger("open");
        }
    }
}
