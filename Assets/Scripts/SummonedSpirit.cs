using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedSpirit : MonoBehaviour
{
    // Start is called before the first frame update
    float init_time;
    float currtime;
    Rigidbody2D rb2d;
    Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();
        init_time = Time.time;
        currtime = init_time;
    }

    // Update is called once per frame
    void Update()
    {
        currtime += Time.deltaTime;
        if (currtime-init_time > 2.0f && currtime-init_time < 4.0f) {
            Homing();
        }
        if (currtime-init_time > 15.0f) {
            Destroy(this.gameObject);
        }
    }
    void Homing() {
        Vector3 dir = (Vector2)target.position - rb2d.position;
        dir.Normalize();
        rb2d.velocity = dir* 5.0f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().PlayerGetHurt(5.0f);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(this.gameObject);
    }
}
