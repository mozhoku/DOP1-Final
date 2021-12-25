using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDemon : Enemy
{
    // Start is called before the first frame update
    private const string ATTACK_ANIM = "blue_demon_fly_attack";
    private const string IDLE = "blue_demon_idle";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Attack");
    }

    IEnumerator Attack() {
        while (true) {
            this.GetComponent<Animator>().Play(ATTACK_ANIM);    
            yield return new WaitForSeconds(3.0f);   
        }
    }

    void EndOfAttackAnimation() {
        this.GetComponent<Animator>().Play(IDLE);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
