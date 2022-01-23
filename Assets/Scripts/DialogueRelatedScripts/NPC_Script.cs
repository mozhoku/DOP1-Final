using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script : MonoBehaviour
{
    public Dialogue diyalog;
    bool bIsInTrigger;
    // Start is called before the first frame update
    public void TriggerDialogue()
    {
        // FindObjectOfType<DialogueManager>().StartDialogue(diyalog);
    }
    private void Update() {
        if (bIsInTrigger) {
            if (Input.GetKeyDown(KeyCode.E)) {
                FindObjectOfType<DialogueManager>().StartDialogue(diyalog);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                FindObjectOfType<DialogueManager>().CloseDialogue();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bIsInTrigger = true;
        } else {bIsInTrigger = false;}
    }
}
