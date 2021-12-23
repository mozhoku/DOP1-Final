using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region ANIM_DECLERATIONS
    private const string PLAYER_IDLE = "player_idle";
    private const string PLAYER_RUN = "player_run";
    private const string PLAYER_JUMP = "player_jump";
    #endregion
    
    public string current_anim_state;
    [SerializeField]
    private Transform grounded_check_transform; 
    [SerializeField]
    private float player_walk_speed = 2.0f;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private bool isGrounded;
    [SerializeField]
    private float jump_strength = 10.0f;
    void Start() {
        rb2d = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();   
    }

    void PlayerMovement() {
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
        if (!isGrounded) {
            current_anim_state = PLAYER_JUMP;
        }
    }

    bool CheckIfGrounded() {
        Collider2D[] amount = Physics2D.OverlapCircleAll(grounded_check_transform.position, 0.25f);
        if (amount.Length > 1){
            return true;
        } else return false;
    }
}
