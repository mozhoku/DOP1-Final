using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HealthSystem
{
    public GameObject death_anim;
    public float max_health;
    private float health_before;
    public SpriteRenderer sr;
    protected virtual void Start() {
        max_health = health;
        StartCoroutine("DeathLookup");
        sr = GetComponent<SpriteRenderer>();
        health_before = health;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (health != health_before) {
            base.GetHurt(sr);
        } 
        health_before = health;
    }

    IEnumerator DeathLookup() {
        while (health > 0) {
            yield return new WaitForSeconds(0.5f);
        }
        Instantiate(death_anim, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
