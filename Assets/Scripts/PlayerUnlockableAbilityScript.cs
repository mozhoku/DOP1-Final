using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlockableAbilityScript : MonoBehaviour
{

    public bool isDashUnlocked;
    private const string PLAYER_DASH = "player_dash";
    private const string PLAYER_DASH_END = "player_dash_end";
    public float dash_duration = 1.5f;
    public float dash_cooldown = 2.0f;
    private float cooldownsave_fordash;
    private float durationsave_fordash;
    


    private PlayerController pc;
    private Rigidbody2D rb2d;
    enum AbilityState {
        ready,
        active,
        on_cooldown
    };
    private AbilityState state;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        state = AbilityState.ready;
        cooldownsave_fordash = dash_cooldown;
        durationsave_fordash = dash_duration;
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
    }

    void Dash() {
        switch (state) {
            case AbilityState.ready:
                if (Input.GetKeyDown(KeyCode.LeftShift) && isDashUnlocked) {
                    rb2d.gravityScale = 0;
                    rb2d.velocity = Vector2.zero;
                    pc.lockPlayer = true;
                    pc.current_anim_state = PLAYER_DASH;
                    state = AbilityState.active;
                }
                break;
            case AbilityState.active:
                transform.Translate(new Vector2(0.02f,0f), 0);
                pc.current_anim_state = PLAYER_DASH;
                dash_duration -= Time.deltaTime;                
                if (dash_duration < 0) {
                    state = AbilityState.on_cooldown;
                    dash_duration = durationsave_fordash;
                    pc.lockPlayer = false;
                    rb2d.gravityScale = 5;
                    pc.current_anim_state = PLAYER_DASH_END;
                }
                break;
            case AbilityState.on_cooldown:
                dash_cooldown -= Time.deltaTime;
                if (dash_cooldown < 0) {
                    dash_cooldown = cooldownsave_fordash;
                    state = AbilityState.ready;
                }
                break;
        }
    }
}
