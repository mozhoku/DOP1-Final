using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBlock : MonoBehaviour
{
    public LevelLoader lvlLoad;
    public bool bLoadNext;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!bLoadNext) lvlLoad.LoadBeforeLevel();
            if (bLoadNext) lvlLoad.LoadNextLevel();
        }
    }
}
