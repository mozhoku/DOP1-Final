using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverCheck : MonoBehaviour
{
    public GameObject boss;
    private bool isItOver;
    public GameObject srFadeBlack;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OverCoro());
        isItOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss==null) {
            isItOver = true;
        }
    }
    IEnumerator OverCoro() {
        while (true) {
            Debug.Log("Heya");
            if (isItOver) {
                Debug.Log("Waov");
                LeanTween.alpha(srFadeBlack, 1.0f, 1.0f).setOnComplete(LoadNext);
                yield return new WaitForSeconds(50.0f);
            }
            else yield return new WaitForSeconds(0.1f);
        }
    }
    void LoadNext() {
        SceneManager.LoadScene("Level_6");
    }
}
