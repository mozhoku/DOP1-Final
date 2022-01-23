using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text name_text;
    public Text diyalog_text;
    private Queue<string> sentencesQueue; 
    private Dialogue diyalogPUB;
    public GameObject DialogueBox;
    public PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        sentencesQueue = new Queue<string>();
    }

    public void StartDialogue(Dialogue diyalog) {
        if (pc.lockPlayer == true) return;
        pc.LockPlayerDialogue();
        LeanTween.move(DialogueBox.GetComponent<RectTransform>(), Vector3.zero, 1.0f).setEase(LeanTweenType.easeOutExpo);
        diyalogPUB = diyalog;
        sentencesQueue.Clear();
        foreach (string sentence in diyalog.sentences) {
            sentencesQueue.Enqueue(sentence);
        }
        name_text.text = diyalog.name;
        DisplayNextSentence(); 
    }

    public void DisplayNextSentence() {
        if (sentencesQueue.Count == 0) {
            EndDialogue();
            return;
        }
        string currsentence = sentencesQueue.Dequeue();
        diyalog_text.text = currsentence;
    }

    void EndDialogue() {
        foreach (string sentence in diyalogPUB.end_strings) {
            sentencesQueue.Enqueue(sentence);
        }
        if (sentencesQueue.Count != 0) {
            DisplayNextSentence();
        }    
    }

    public void CloseDialogue() {
        if (pc.lockPlayer == false) {return;}
        pc.lockPlayer = false;
        sentencesQueue.Clear();
        LeanTween.move(DialogueBox.GetComponent<RectTransform>(), new Vector3(0,274f,0), 1.0f).setEase(LeanTweenType.easeOutExpo);
    }
}
