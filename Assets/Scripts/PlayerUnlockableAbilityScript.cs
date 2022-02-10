using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlockableAbilityScript : MonoBehaviour
{
    [Header("Dash Ability")]
    [Space(4)]
    public bool isDashUnlocked;
    public Transform spawn_point;
    public GameObject dash_anim;
    private const string PLAYER_DASH = "player_dash";
    private const string PLAYER_DASH_END = "player_dash_end";
    public float dash_duration = 1.5f;
    public float dash_cooldown = 2.0f;
    private float cooldownsave_fordash;
    private float durationsave_fordash;
    public float dash_range = 0.05f;
    private bool can_dash_in_air;


    private PlayerController pc;
    private Rigidbody2D rb2d;
    public enum AbilityState {
        ready,
        active,
        on_cooldown
    };
    public AbilityState state_dash;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        state_dash = AbilityState.ready;
        cooldownsave_fordash = dash_cooldown;
        durationsave_fordash = dash_duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.playerDead){return;} 
        if (pc.lockPlayer && pc.isGrounded && state_dash == AbilityState.ready) return;
        if (pc.currently_climbing) return;
        if (pc.isGrounded) can_dash_in_air = true;
        Dash();
    }

    #region DASH
    void Dash() {
        switch (state_dash) {
            case AbilityState.ready:
                if (!pc.isGrounded && !can_dash_in_air) return;
                if (Input.GetKeyDown(KeyCode.LeftShift) && isDashUnlocked && !pc.currently_attacking) {
                    Physics2D.IgnoreLayerCollision(8,6, true);
                    rb2d.gravityScale = 0;
                    rb2d.velocity = Vector2.zero;
                    pc.lockPlayer = true;
                    pc.current_anim_state = PLAYER_DASH;
                    state_dash = AbilityState.active;
                    StartCoroutine(SpawnFXSprites());
                }
                break;
            case AbilityState.active:
                //invul during dash -> no collision between enemies and player
                if (Mathf.Sign(transform.localScale.x) == -1)
                    rb2d.velocity = new Vector2(-dash_range, 0f);
                else
                    rb2d.velocity = new Vector2(dash_range, 0f);
                pc.current_anim_state = PLAYER_DASH;
                dash_duration -= Time.deltaTime;                
                if (dash_duration < 0) {
                    if (!pc.isGrounded && can_dash_in_air) can_dash_in_air = false;
                    Physics2D.IgnoreLayerCollision(8,6, false);
                    rb2d.velocity = Vector2.zero;
                    state_dash = AbilityState.on_cooldown;
                    dash_duration = durationsave_fordash;
                    rb2d.gravityScale = 5;
                    pc.lockPlayer = false;
                    pc.current_anim_state = PLAYER_DASH_END;
                }
                break;
            case AbilityState.on_cooldown:
                dash_cooldown -= Time.deltaTime;
                if (dash_cooldown < 0) {
                    dash_cooldown = cooldownsave_fordash;
                    state_dash = AbilityState.ready;
                }
                break;
        }
    }

    IEnumerator SpawnFXSprites() {
        while (state_dash == AbilityState.active) {
            Vector3 randspawnp = spawn_point.position+
                                    new Vector3(Input.GetAxisRaw("Horizontal")*-0.6f, Random.Range(-0.4f, 0.0f), 0.0f);
            GameObject sp = Instantiate(dash_anim, randspawnp, 
                                        Quaternion.Euler(0.0f, 0.0f, 90.0f));
            yield return new WaitForSeconds(0.02f);
        }
    }

    #endregion

    void Throw() {

    }
}
