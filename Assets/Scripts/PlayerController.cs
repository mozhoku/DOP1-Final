using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthSystem
{

    #region ANIM_DECLERATIONS
    private const string PLAYER_DEATH = "player_death";
    private const string PLAYER_HURT = "player_hurt";
    private const string PLAYER_IDLE = "player_idle";
    private const string PLAYER_RUN = "player_run";
    private const string PLAYER_CLIMB = "player_climb";
    private const string PLAYER_JUMP = "player_jump";
    private const string PLAYER_CLIMB_IDLE = "player_climb_idle";
    private const string PLAYER_GROUND_ATTACK_1 = "player_ground_attack1";
    private const string PLAYER_GROUND_ATTACK_2 = "player_ground_attack2";
    private const string PLAYER_FALL = "player_fall";
    #endregion
    public float max_health;
    private bool currently_climbing;
    public bool lockPlayer;
    public string current_anim_state;
    [SerializeField] private LevelLoader levelLoad;
    [SerializeField]
    private Transform grounded_check_transform; 
    [SerializeField]
    private Transform can_climb_check_HIT; 
    [SerializeField]
    private Transform can_climb_check_MISS; 
    [SerializeField]
    private Transform can_climb_check_TRANSFORMPOS; 
    [SerializeField]
    private float player_walk_speed = 2.0f;
    private Rigidbody2D rb2d;
    public bool isGrounded;
    public bool currently_attacking;
    [SerializeField]
    private float jump_strength = 10.0f;
    private Vector3 before_climbing_pos;
    [SerializeField]
    public float attack_radius = 0.25f;
    public GameObject attack_anim_hitbox;
    private bool do_the_second_attack = false;
    private int clickamount = 0;
    public LayerMask enemyMask;
    private BoxCollider2D bc2d;
    private Vector3 init_scale;
    private SpriteRenderer sr;
    public bool isHurt;
    public Transform target_pivot;
    public GameObject dust_particle_jump_directional;
    public GameObject dust_particle_jump_nondirectional;
    public bool playerDead;
    private bool isInvincible;
    private PlayerUnlockableAbilityScript unlockable_abilites;
    private bool canDoubleJump;
    [SerializeField] LayerMask platformMask;

    public int EnableDamageBoost = 0;
    public AudioClip jumpSoundClip;
    public AudioClip runSoundClip;


    void Start() {
        rb2d = this.GetComponent<Rigidbody2D>();
        bc2d = this.GetComponent<BoxCollider2D>();
        sr = this.GetComponent<SpriteRenderer>();
        unlockable_abilites = GetComponent<PlayerUnlockableAbilityScript>();
        init_scale = this.transform.localScale;
        currently_climbing = false;
        lockPlayer = false;
        playerDead = false;
        max_health = health;
    }

    void Update()
    {
        if (isGrounded && !audioSource.isPlaying && Mathf.Abs(rb2d.velocity.x) > 0) {
            audioSource.clip = runSoundClip;
            audioSource.Play();
        } if (rb2d.velocity.x == 0 && audioSource.clip == runSoundClip) {
            audioSource.Stop();
        }
        if (health > max_health) {
            health = max_health;
        }
        if (health <= 0) {
            if (current_anim_state == PLAYER_DEATH) return;
            OnDeath();
            return;
        }
        if (!currently_attacking && !isHurt && !lockPlayer) {
            PlayerMovement();   
            if (!CheckIfGrounded()) {
                CanClimb();

            }
        }
        if (!currently_climbing && !isHurt && !lockPlayer) {
            Attack();
        }
    }

    public void LockPlayerDialogue() {
        lockPlayer = true;
        rb2d.velocity = Vector2.zero;
        current_anim_state = PLAYER_IDLE;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("asansor_trigger")) {
            lockPlayer = true;
            current_anim_state = PLAYER_IDLE;
            rb2d.velocity = Vector2.zero;
            GameObject ele_go = FindClosestGos("asansor");
            StartCoroutine(GoUpElevator(ele_go));
        }
    }

    IEnumerator GoUpElevator(GameObject ele_go) {
        while (true) {
            Vector3 nextpos = Vector3.Lerp(ele_go.transform.position, new Vector3(ele_go.transform.position.x, 0.0f, 0.0f), 0.02f);
            if (nextpos.y == 0.0f || nextpos.y > -0.1f) {
                ele_go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                break;
            }
            ele_go.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 300.0f, 0.0f)*Time.fixedDeltaTime;
            rb2d.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);
        }
        lockPlayer = false;
    }

    private GameObject FindClosestGos(string tag)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude; //no minuses
            if (curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void OnDeath() {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        current_anim_state = PLAYER_DEATH; 
        playerDead = true;
        StartCoroutine(levelLoad.OnDeath());
    }

    public void PlayerGetHurt(float damage) {
        if (unlockable_abilites.state_dash == PlayerUnlockableAbilityScript.AbilityState.active) {
            return;
        } else{
        isHurt = true;
        current_anim_state = PLAYER_HURT;
        base.GetDamaged(damage);
        base.GetHurt(sr);
        rb2d.gravityScale = 5;
        isInvincible = true;
        Invoke("ResetInvincible", 2.0f); }
    }

    #region ONCOLLISION
    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.layer == 6 && !isInvincible) {
            // isHurt = true;
            // current_anim_state = PLAYER_HURT;
            PlayerGetHurt(20.0f);
            foreach (ContactPoint2D contact in other.contacts)
            {
                Vector2 direction = new Vector2(this.transform.position.x, this.transform.position.y) - contact.point;
                rb2d.AddForce(direction*4.0f, ForceMode2D.Impulse);
            }
            // base.GetDamaged(20.0f);
            // base.GetHurt(sr);
            // rb2d.gravityScale = 5;
            // isInvincible = true;
            // Invoke("ResetInvincible", 2.0f);
        }
    }
    #endregion


    #region RESETS

    void ResetInvincible() {
        isInvincible = false;
    }

    void RestartIsHurt() {
        isHurt = false;
        //This is to fix animation stuck problem
        //Dont touch this
        currently_attacking = false;
        clickamount = 0;
    }
    #endregion

    #region CLICK_DEFAULT_ATTACK
    //This is to fix the animation skipping that happens when you attack consequently
    //Reset is in the resetting function, at the end
    private string before_attack_anim;
    void Attack() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isHurt)
            clickamount++;
        if (clickamount > 1 && current_anim_state != PLAYER_GROUND_ATTACK_2) {
            do_the_second_attack = true;
        }
        if (clickamount >= 1 && !currently_attacking) {
            rb2d.velocity = Vector2.zero;
            before_attack_anim = current_anim_state;
            currently_attacking = true;
            current_anim_state = PLAYER_GROUND_ATTACK_1;
        } else {
        }
    }

    private GameObject newly_damaged_attack1;
    void Attack1_OR() {
        // if (!isGrounded) {
            // rb2d.velocity = Vector2.zero;
            // rb2d.gravityScale = 0;
        // }
        Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_anim_hitbox.transform.position, attack_radius, enemyMask);
        foreach (Collider2D coll in collisions) {
            if (coll.gameObject == newly_damaged_attack1) {} else{
            newly_damaged_attack1 = coll.gameObject;
            newly_damaged_attack1.GetComponent<Enemy>().GetDamaged(20.0f+EnableDamageBoost*10.0f);}
        }
    }

    private GameObject newly_damaged_attack2;

    void Attack2_OR() {
        if (do_the_second_attack) {
            current_anim_state = PLAYER_GROUND_ATTACK_2;
            Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_anim_hitbox.transform.position, attack_radius, enemyMask);
            foreach (Collider2D coll in collisions) {
                if (coll.gameObject == newly_damaged_attack2) {} else{
                newly_damaged_attack2 = coll.gameObject;
                newly_damaged_attack2.GetComponent<Enemy>().GetDamaged(20.0f+EnableDamageBoost*10.0f);}
            }
        }
    }
    void StopAbilityToAttack() {
        current_anim_state = before_attack_anim;
        clickamount = 0;
        rb2d.gravityScale = 5;
        rb2d.velocity = new Vector2(0, -5.0f);
        currently_attacking = false;
        do_the_second_attack = false;
        newly_damaged_attack1 = null;
        newly_damaged_attack2 = null;
    }

    #endregion

    #region CLIMB

    void ReturnAbilityToMove() {
        currently_climbing = false;
    }

    void LockMovementForClimbing() {
        currently_climbing = true;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;
        current_anim_state = PLAYER_CLIMB;
    }

    void StartClimbIdleAnimation() {
        current_anim_state = PLAYER_CLIMB_IDLE;
    }


    void CanClimb() {
        if (currently_climbing) return;
        #region old_climb
        // Collider2D[] amount = Physics2D.OverlapCircleAll(can_climb_check.position, 0.15f);
        // foreach (Collider2D bash in amount) {
        //     // if (bash.gameObject.CompareTag("platform")) {
        //     // 7 is the platform layer index
        //     if (bash.gameObject.layer == 7) {
        //         float y = bash.gameObject.transform.localScale.y;
        //         this.transform.position = new Vector3(this.transform.position.x, 
        //                                                 bash.transform.position.y+(y/2)-0.3f, 
        //                                                 this.transform.position.z);
        //         current_anim_state = PLAYER_CLIMB;
        //         return;
        //     }
        // }
        #endregion
        // Debug.DrawRay((Vector2)can_climb_check_HIT.position, new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized, Color.red, 0.1f);

        //Did the player hit a wall?
        Collider2D[] amount = Physics2D.OverlapCircleAll(can_climb_check_HIT.position, 0.4f, platformMask);
        if (amount.Length == 0) return;
        //transform the player according to this, be sure that it hits a wall
        RaycastHit2D hit = Physics2D.Raycast((Vector2)can_climb_check_TRANSFORMPOS.position, 
        new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized, 0.5f, platformMask);
        //this has to return false, or it means theres a wall
        //and we dont want to climb that
        bool hitMISS = Physics2D.Raycast((Vector2)can_climb_check_MISS.position, new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized, 0.8f, platformMask);
        //seems to return TRUE at first, hits the pos (0,0,0). so to be sure add magnitude check
        if (!hitMISS && hit.point.magnitude != 0) {
            transform.position = new Vector3(hit.point.x, transform.position.y, 0);
            current_anim_state = PLAYER_CLIMB;
            return;
        }
    }
    #endregion

    //CheckIfGrounded here
    //false if not
    #region PLAYER_MOVEMENT
    public static void ScaleAround(Transform target, Transform pivot, Vector3 scale) {
        Transform pivotParent = pivot.parent;
        Vector3 pivotPos = pivot.position;
        pivot.parent = target;        
        target.localScale = scale;
        target.position += pivotPos - pivot.position;
        pivot.parent = pivotParent;
    }

    float time_before_switch = 0.15f;
    void PlayerMovement() {
        if (!currently_climbing) {
            float horiz = Input.GetAxisRaw("Horizontal");
            if (horiz != 0) {
                if (horiz < 0 && -init_scale.x != transform.localScale.x) {
                    ScaleAround(this.transform, target_pivot, new Vector3(-4.4223f, 4.4223f,4.4223f));
                    // this.transform.localScale = new Vector3(-init_scale.x, init_scale.y,init_scale.z);
                }
                else if (horiz > 0 && init_scale.x != transform.localScale.x) {
                    ScaleAround(this.transform, target_pivot, new Vector3(4.4223f, 4.4223f,4.4223f));

                    // this.transform.localScale = new Vector3(init_scale.x, init_scale.y,init_scale.z);
                }

                // this.transform.Translate(new Vector3(
                //     Input.GetAxis("Horizontal")*player_walk_speed*Time.deltaTime,
                //     0f,
                //     0f
                // ));
                rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*player_walk_speed*Time.fixedDeltaTime, rb2d.velocity.y);
                if (current_anim_state == PLAYER_FALL && !isGrounded) {} else {
                    current_anim_state = PLAYER_RUN;
                }
            } else {
                //Bootleg fix for animation skipping
                time_before_switch -= Time.deltaTime;
                if(time_before_switch < 0) {current_anim_state=PLAYER_IDLE;time_before_switch=0.15f;rb2d.velocity=new Vector2(0, rb2d.velocity.y);}
            }

            isGrounded = CheckIfGrounded();
            if (isGrounded) canDoubleJump = true;
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
                audioSource.clip = jumpSoundClip;
                audioSource.Play();
                JumpFunction(jump_strength, horiz);
            }
            if (canDoubleJump && !isGrounded && Input.GetKeyDown(KeyCode.Space)) {
                audioSource.clip = jumpSoundClip;
                audioSource.Play();
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                JumpFunction(jump_strength/1.3f, horiz);
                canDoubleJump = false;
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
                audioSource.clip = jumpSoundClip;
                audioSource.Play();
                rb2d.AddForce(new Vector2(0, jump_strength), ForceMode2D.Impulse);
                current_anim_state = PLAYER_JUMP;
                rb2d.gravityScale = 5;
                Invoke("ReturnAbilityToMove", 0.1f);
            }
        }
    }

    void JumpFunction(float strength, float horiz) {
        if (horiz > 0) {Play_Animation_Effect_Smoke_Directional(horiz, new Vector3(-0.55f, 0.30f, 0.0f)); }
        else if (horiz < 0) {Play_Animation_Effect_Smoke_Directional(horiz, new Vector3(0.55f, 0.30f, 0.0f)); }
        else if (horiz == 0) {Play_Animation_Effect_Smoke_Nondirectional();}
        rb2d.AddForce(new Vector2(0, strength), ForceMode2D.Impulse);
    }

    //false if not grounded
    bool CheckIfGrounded() {
        Collider2D[] amount = Physics2D.OverlapCircleAll(grounded_check_transform.position, 0.25f);
        if (amount.Length > 1){
            return true;
        } else return false;
    }

    #endregion
    
    #region effects

    void Play_Animation_Effect_Smoke_Directional(float horizontal, Vector3 offset) {
        GameObject returned_obj = Instantiate(dust_particle_jump_directional,
                        grounded_check_transform.position+offset, Quaternion.identity);
        returned_obj.transform.localScale = new Vector3(2.0f*horizontal,2.0f,2.0f);
        returned_obj.GetComponent<Animator>().Play("smoke_directional_feet", 0, 0f);
    }

    void Play_Animation_Effect_Smoke_Nondirectional() {
        GameObject returned_obj = Instantiate(dust_particle_jump_nondirectional,
                        grounded_check_transform.position+new Vector3(0.0f, 0.30f, 0.0f), Quaternion.identity);
        returned_obj.transform.localScale = new Vector3(4.0f,4.0f,4.0f);
        returned_obj.GetComponent<Animator>().Play("smoke_nondirectional_feet", 0, 0f);
    }

    #endregion
    //Debug
    private void OnDrawGizmos() {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(can_climb_check.position, 0.15f);
        // Gizmos.DrawSphere(attack_anim_hitbox.transform.position, 0.2f);
    }
}
