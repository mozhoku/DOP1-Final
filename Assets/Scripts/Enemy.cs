using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : HealthSystem
{
    public GameObject death_anim;
    public float max_health;
    private float health_before;
    public SpriteRenderer sr;

    [Header("AI")]
    public bool enableAI;
    public Transform target;
    public float next_waypoint_distance = 1.0f;
    public float speed = 10.0f;
    Path path;
    int current_waypoint;
    bool reached_end_of_path;
    Seeker seeker;
    Rigidbody2D rb2d;

    protected virtual void Start() {
        max_health = health;
        StartCoroutine("DeathLookup");
        sr = GetComponent<SpriteRenderer>();
        health_before = health;
        if (enableAI) {
            seeker = GetComponent<Seeker>();
            rb2d = GetComponent<Rigidbody2D>();
            InvokeRepeating("UpdatePath", 0.0f, 0.5f);
        }
    }

    protected virtual void Update()
    {
        if (enableAI) UpdateFunction_Pathfinding();
        Health_Damage();
    }

    #region PATHFINDING

    void UpdatePath() {
        if (seeker.IsDone())
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p) {
        if (!p.error) {path = p;current_waypoint = 0;}
    }

    void UpdateFunction_Pathfinding() {
        if (rb2d.velocity.x > 0) sr.flipX = true; else sr.flipX = false;
        if (path == null) return;
        if (current_waypoint >= path.vectorPath.Count) {
            reached_end_of_path = true;
            return;
        } else {
            reached_end_of_path = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[current_waypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb2d.AddForce(force);

        float dist = Vector2.Distance(rb2d.position, path.vectorPath[current_waypoint]);
        if (dist < next_waypoint_distance) {
            current_waypoint++;
        }
    }

    #endregion

    
    void Health_Damage() {
        if (health != health_before) {
            base.GetHurt(sr);
        } 

        health_before = health;
    }
    IEnumerator DeathLookup() {
        while (health > 0) {
            yield return new WaitForSeconds(0.1f);
        }
        Instantiate(death_anim, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
