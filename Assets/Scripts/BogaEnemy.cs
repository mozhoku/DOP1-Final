using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogaEnemy : Enemy
{
    [Header("Enemy-wise Options")]
    public Animator animator;
    public LayerMask playerMask;
    public GameObject player;
    public GameObject RaycastPosition;
    public float speed_of_the_ram;
    public float duration_of_the_ram;
    public float CD_period;
    private Vector3 init_scale;
    AudioSource audiosource;
    public AudioClip roar;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackRam());
        init_scale = transform.localScale;
        audiosource = GetComponent<AudioSource>();
    }

    IEnumerator AttackRam() {
        yield return new WaitForSeconds(2.0f);
        while (true) {
            if (Vector3.Distance(transform.position, player.transform.position) > 10.0f) {
                yield return new WaitForSeconds(0.1f);
            } 
            RaycastHit2D hit = Physics2D.Raycast(RaycastPosition.transform.position, 
                            (RaycastPosition.transform.position-transform.position).normalized, 
                            Mathf.Infinity);
            if (hit.collider.gameObject.layer  != 8) {yield return new WaitForSeconds(0.1f);continue;}
            float where_is_player_in_the_x_direction = (player.transform.position - this.transform.position).x;
            audioSource.clip = roar;
            audiosource.Play();
            yield return new WaitForSeconds(0.25f);
            rb2d.velocity = new Vector2(where_is_player_in_the_x_direction, 0f).normalized * speed_of_the_ram;
            animator.SetTrigger("attack");
            yield return new WaitForSeconds(duration_of_the_ram);
            rb2d.velocity = Vector2.zero;
            yield return new WaitForSeconds(CD_period);
        }

    }
    protected override void Update() 
    {
        base.Update();

        float xdirect = (player.transform.position - this.transform.position).x;
        if (xdirect < 0) {
            transform.localScale = new Vector3(-init_scale.x, init_scale.x,init_scale.x);
            // sr.flipX = true;
        } else {
            // sr.flipX = false;
            transform.localScale = init_scale;
        }

    }
    public override void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);
    }
}
