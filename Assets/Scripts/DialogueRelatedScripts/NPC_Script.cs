using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script : MonoBehaviour
{
    public Dialogue diyalog;
    public Dialogue NotFoundObjectDialogue;
    bool bIsInTrigger;
    public bool bIsThisEffectorNPC;
    // Start is called before the first frame update
    public void TriggerDialogue()
    {
        // FindObjectOfType<DialogueManager>().StartDialogue(diyalog);
    }
    private void Update() {
        if (bIsInTrigger) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (bIsThisEffectorNPC) {

                    if (!PlayerEffectors.bdid_get_damage_boost) {
                        FindObjectOfType<DialogueManager>().StartDialogue(NotFoundObjectDialogue);
                    } else {
                        PlayerEffectors.bdid_enable_damage_boost = true;
                    }

                    if (PlayerEffectors.bdid_get_damage_boost && PlayerEffectors.bdid_get_damage_boost) {
                        FindObjectOfType<DialogueManager>().StartDialogue(diyalog);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().EnableDamageBoost = 1;   
                    }

                } 
                else {
                    FindObjectOfType<DialogueManager>().StartDialogue(diyalog);
                }
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
