using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthSystem
{

    #region ANIM_DECLERATIONS
    private const string PLAYER_IDLE = "player_idle";
    private const string PLAYER_RUN = "player_run";
    private const string PLAYER_CLIMB = "player_climb";
    private const string PLAYER_JUMP = "player_jump";
    private const string PLAYER_CLIMB_IDLE = "player_climb_idle";
    private const string PLAYER_GROUND_ATTACK_1 = "player_ground_attack1";
    private const string PLAYER_GROUND_ATTACK_2 = "player_ground_attack2";
    private const string PLAYER_FALL = "player_fall";
    #endregion
    private bool currently_climbing;
    public string current_anim_state;
    [SerializeField]
    private Transform grounded_check_transform; 
    [SerializeField]
    private Transform can_climb_check; 
    [SerializeField]
    private float player_walk_speed = 2.0f;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool currently_attacking;
    [SerializeField]
    private float jump_strength = 10.0f;
    private Vector3 before_climbing_pos;
    [SerializeField]
    private GameObject attack_anim_hitbox;
    private bool do_the_second_attack = false;
    private int clickamount = 0;
    public LayerMask enemyMask;

    void Start() {
        rb2d = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        currently_climbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currently_attacking) {
            PlayerMovement();   
            if (!CheckIfGrounded()) {
                CanClimb();
            }
        }
        if (!currently_climbing) {
            Attack();
        }
    }

    #region CLICK_DEFAULT_ATTACK
    void Attack() {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            clickamount++;
        if (clickamount > 1 && current_anim_state != PLAYER_GROUND_ATTACK_2) {
            do_the_second_attack = true;
        }
        if (clickamount >= 1 && !currently_attacking) {
            currently_attacking = true;
            current_anim_state = PLAYER_GROUND_ATTACK_1;
        }
    }

    private GameObject newly_damaged_attack1;
    void Attack1_OR() {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_anim_hitbox.transform.position, 0.25f, enemyMask);
        foreach (Collider2D coll in collisions) {
            if (coll.gameObject == newly_damaged_attack1) return;
            newly_damaged_attack1 = coll.gameObject;
            newly_damaged_attack1.GetComponent<Enemy>().GetDamaged(20.0f);
        }
    }

    private GameObject newly_damaged_attack2;

    void Attack2_OR() {
        if (do_the_second_attack) {
            current_anim_state = PLAYER_GROUND_ATTACK_2;
            Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_anim_hitbox.transform.position, 0.25f, enemyMask);
            foreach (Collider2D coll in collisions) {
                if (coll.gameObject == newly_damaged_attack2) return;
                newly_damaged_attack2 = coll.gameObject;
                newly_damaged_attack2.GetComponent<Enemy>().GetDamaged(20.0f);
            }
        }
    }
    void StopAbilityToAttack() {
        clickamount = 0;
        currently_attacking = false;
        do_the_second_attack = false;
        newly_damaged_attack1 = null;
        newly_damaged_attack2 = null;
    }

    #endregion


    //Used animation event
    //TODO:
    //When the height of the sprite you want to climb towards changes
    //Hands are in the wrong place
    //Fix it 
    #region CLIMB

    void ReturnAbilityToMove() {
        currently_climbing = false;
    }
    void LockMovementForClimbing() {
        currently_climbing = true;
        rb2d.gravityScale = 0;
        rb2d.velocity = Vector2.zero;
    }
    void StartClimbIdleAnimation() {
        current_anim_state = PLAYER_CLIMB_IDLE;
    }


    void CanClimb() {
        if (currently_climbing) return;
        Collider2D[] amount = Physics2D.OverlapCircleAll(can_climb_check.position, 0.15f);
        foreach (Collider2D bash in amount) {
            if (bash.gameObject.CompareTag("platform")) {
                float y = bash.gameObject.transform.localScale.y;
                this.transform.position = new Vector3(this.transform.position.x, 
                                                        bash.transform.position.y+(y/2)-0.3f, 
                                                        this.transform.position.z);
                current_anim_state = PLAYER_CLIMB;
                return;
            }
        }
    }
    #endregion

    //CheckIfGrounded here
    //false if not
    #region PLAYER_MOVEMENT

    void PlayerMovement() {
        if (!currently_climbing) {
            float horiz = Input.GetAxisRaw("Horizontal");
            if (horiz != 0) {
                if (horiz < 0) {sr.flipX=true;} 
                else {sr.flipX=false;}
                this.transform.Translate(new Vector3(
                    Input.GetAxis("Horizontal")*player_walk_speed*Time.deltaTime,
                    0f,
                    0f
                ));
                current_anim_state = PLAYER_RUN;
            } else {current_anim_state=PLAYER_IDLE;}

            isGrounded = CheckIfGrounded();

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
                rb2d.AddForce(new Vector2(0, jump_strength), ForceMode2D.Impulse);
            }
            if (!isGrounded && rb2d.velocity.y > 0) {
                current_anim_state = PLAYER_JUMP;
            }

            if (!isGrounded && rb2d.velocity.y < 0) {
                current_anim_state = PLAYER_FALL;
            }
        }

        if (currently_climbing && (current_anim_state == PLAYER_CLIMB_IDLE || current_anim_state == PLAYER_CLIMB)) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                rb2d.AddForce(new Vector2(0, jump_strength), ForceMode2D.Impulse);
                current_anim_state = PLAYER_JUMP;
                rb2d.gravityScale = 5;
                Invoke("ReturnAbilityToMove", 0.1f);
            }
        }
    }

    //false if not grounded
    bool CheckIfGrounded() {
        Collider2D[] amount = Physics2D.OverlapCircleAll(grounded_check_transform.position, 0.25f);
        if (amount.Length > 1){
            return true;
        } else return false;
    }

    #endregion
    

    
    //Debug
    private void OnDrawGizmos() {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(can_climb_check.position, 0.15f);
        // Gizmos.DrawSphere(attack_anim_hitbox.transform.position, 0.2f);
    }
}
