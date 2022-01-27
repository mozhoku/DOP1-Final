using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerEffectors : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool bdid_pick_up_health_booster;
    public static bool bdid_get_damage_boost;
    public static bool bdid_enable_damage_boost;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += this.CheckBoosters;      
    }

    public void CheckBoosters(Scene scene, LoadSceneMode sceneMode) {
        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (bdid_pick_up_health_booster) {
            pc.max_health = 60;
            pc.health = 60;
        } if (PlayerEffectors.bdid_get_damage_boost && PlayerEffectors.bdid_enable_damage_boost) {
            pc.EnableDamageBoost = 1;   
        }

    }
}
