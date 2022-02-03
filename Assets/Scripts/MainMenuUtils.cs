using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUtils : MonoBehaviour
{
    public GameObject but;
    // Start is called before the first frame update
    void Start()
    {
        but = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHoverRect() {
        LeanTween.scaleX(but, 1.5f, 1.0f).setEase(LeanTweenType.easeInOutExpo);
    }
    public void OnExitRect() {
        LeanTween.scaleX(but, 1.0f, 1.0f).setEase(LeanTweenType.easeInOutExpo);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadFirstLevel() {
        SceneManager.LoadScene("Level_1");
    }
}
