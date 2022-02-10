using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public GameObject PauseObj;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                isPaused = false;
                ContinueGame();
            } else {
                isPaused = true;
                PauseGame();
            }
        }
    }

    void ContinueGame() {
        Time.timeScale = 1;
        PauseObj.SetActive(false);
    }

    void PauseGame() {
        Time.timeScale = 0;
        PauseObj.SetActive(true);
    }
}
