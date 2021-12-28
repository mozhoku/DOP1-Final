using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public PlayerController pc;
    private Animator animator;
    public string curr_anim;
    void Start()
    {
        animator = pc.GetComponent<Animator>();   
    }

    void Update()
    {
        PlayAnim();
    }

    void PlayAnim() {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //let dash end finish before going into idle animation on ground
        if (info.IsName("player_dash_end")) {
            if (info.normalizedTime < 1) {
                pc.current_anim_state = "player_dash_end";
            } else {
                pc.isHurt = false;
                pc.currently_attacking = false;
                pc.lockPlayer = false;
                pc.current_anim_state = "player_idle";
            } 
        }
        
        if (curr_anim == pc.current_anim_state) {return;}
        animator.Play(pc.current_anim_state);
        curr_anim = pc.current_anim_state;
    }
}
