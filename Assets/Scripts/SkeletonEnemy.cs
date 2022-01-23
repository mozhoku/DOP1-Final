using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    private const string ATTACK_THRUST = "skeleton-attack-thrust";
    private const string IDLE = "skeleton-idle";
    private const string WALK = "skeleton-walk";
    private const string HURT = "skeleton-hit";

    [Header("Enemy-wise Options")]
    public Animator animator;
    public Transform raycastpos;
    public LayerMask player;
    public Transform attack_pos;
    private AnimatorStateInfo info;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackThrust());
    }

    IEnumerator AttackThrust() {
        while (true){
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)(raycastpos.position-transform.position).normalized, 
                                                3.0f, player);
            if (hit.transform != null) {
                dont_move = true;
                rb2d.velocity = Vector2.zero;
                animator.Play(ATTACK_THRUST);
                yield return new WaitForSeconds(2.5f);
            } else {
                yield return new WaitForSeconds(0.1f);            
            }

        }
    }

    protected override void Health_Damage(string animationToPlay = null, Animator anim = null)
    {
        if (info.IsName(ATTACK_THRUST)) {base.Health_Damage();}
        base.Health_Damage(HURT, animator);

    }


    private GameObject newly_damaged_attack;

    void DetectPlayerAndHurt(float damage) {           
        Collider2D[] collisions = Physics2D.OverlapBoxAll(attack_pos.position, new Vector2(1.0f, 0.16f), attack_pos.eulerAngles.z, player);
        foreach (Collider2D coll in collisions) {
            if (coll.gameObject == newly_damaged_attack) {} else {
            newly_damaged_attack = coll.gameObject;
            newly_damaged_attack.GetComponent<PlayerController>().PlayerGetHurt(damage);}
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        // base.Update();
        // base.Health_Damage();
        info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(ATTACK_THRUST)) {
            base.Health_Damage();
        } else {
            base.Update();
        }
        if (!info.IsName(ATTACK_THRUST) && !info.IsName(HURT)) {
            newly_damaged_attack = null;
            if (Mathf.Abs(rb2d.velocity.x) > 0) {
                if (info.IsName(WALK)) return;
                animator.Play(WALK);
            } else {
                if (info.IsName(IDLE)) return;
                animator.Play(IDLE);
            }
        } else if (info.IsName(ATTACK_THRUST)) {
            DetectPlayerAndHurt(10.0f);
        }
        
    }
}
