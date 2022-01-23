using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{

    public Transform player;
    public GameObject Image;
    // public Animator anim;
    public Camera cam;
    bool loading_level;
    GameManager gm;

    void Start()
    {
        //Check if player is null, if so then find it
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //Check if cam is null, if so then find it
        if (cam == null) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        Image.transform.position = cam.WorldToScreenPoint(player.transform.position); 
        LeanTween.scale(Image, new Vector3(20f,20f,20f), 1.0f);
    }

    void Update()
    {
        //This is to give the effect of "closing in" towards the player
        //Its also in coroutine but it doesnt want to work that way
        Image.transform.position = cam.WorldToScreenPoint(player.transform.position); 
    }

    public void LoadBeforeLevel() {
        player.GetComponent<PlayerController>().lockPlayer = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(LoadLevel(false));
        SceneManager.sceneLoaded += SceneLoadLeft;
    }

    void SceneLoadLeft(Scene scene, LoadSceneMode mode) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject go = GameObject.FindGameObjectWithTag("level_after_tag");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.down*10.0f;
        player.transform.position = go.transform.position + new Vector3(-2.5f, 0.0f, 0.0f);
        SceneManager.sceneLoaded -= SceneLoadLeft;
        
    }

    public void LoadNextLevel() {
        player.GetComponent<PlayerController>().lockPlayer = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(LoadLevel(true));
        SceneManager.sceneLoaded += SceneLoadRight;
    }

    void SceneLoadRight(Scene scene, LoadSceneMode mode) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject go = GameObject.FindGameObjectWithTag("level_before_tag");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.down*10.0f;

        player.transform.position = go.transform.position + new Vector3(3.0f, 0.0f, 0.0f);
        SceneManager.sceneLoaded -= SceneLoadRight;

    }

    public IEnumerator OnDeath() {
        Image.transform.position = cam.WorldToScreenPoint(player.transform.position); 
        yield return new WaitForSeconds(1.0f);
        //close in towards player, wait 1 sec, then restart the scene
        LeanTween.scale(Image, new Vector3(0.01f,0.01f,0.01f), 1.0f).setEase(LeanTweenType.easeOutCubic);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadLevel(bool next) {
        int index = 0;
        if (next) index = 1;
        else index = -1;
        LeanTween.scale(Image, new Vector3(0.1f,0.1f,0.1f), 1.0f);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+index);

    }
}
