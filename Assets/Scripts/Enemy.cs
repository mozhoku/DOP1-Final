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
    public Rigidbody2D rb2d;
    private Vector3 scale_at_start;

    [Header("AI")]
    public bool enableAI;
    public Transform target;
    public float next_waypoint_distance = 1.0f;
    public float speed = 10.0f;
    Path path;
    int current_waypoint;
    bool reached_end_of_path;
    Seeker seeker;
    public bool dont_move;

    protected virtual void Start() {
        max_health = health;
        scale_at_start = transform.localScale;
        StartCoroutine("DeathLookup");
        rb2d = GetComponent<Rigidbody2D>();
        health_before = health;
        if (enableAI) {
            seeker = GetComponent<Seeker>();
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
        if (rb2d.velocity.x > 0) 
            transform.localScale = new Vector3(-scale_at_start.x, scale_at_start.y, scale_at_start.z); 
        else if (rb2d.velocity.x < 0) 
            transform.localScale = new Vector3(scale_at_start.x, scale_at_start.y, scale_at_start.z); 
        if (path == null) return;
        if (current_waypoint >= path.vectorPath.Count) {
            reached_end_of_path = true;
            return;
        } else {
            reached_end_of_path = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[current_waypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        // Vector2.Lerp((Vector2)transform.position, direction, 0.1f);
        // rb2d.MovePosition(Vector2.Lerp((Vector2)transform.position, path.vectorPath[current_waypoint], 0.1f));
        // transform.Translate(direction);
        // if (!dont_move) rb2d.AddForce(force);
        if (!dont_move) rb2d.velocity = force;

        float dist = Vector2.Distance(rb2d.position, path.vectorPath[current_waypoint]);
        if (dist < next_waypoint_distance) {
            current_waypoint++;
        }
    }

    #endregion

    
    protected virtual void Health_Damage(string animationToPlay = null, Animator anim = null) {
        if (health != health_before) {
            base.GetHurt(sr);
            if (animationToPlay != null) {
                anim.Play(animationToPlay);
            }
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
