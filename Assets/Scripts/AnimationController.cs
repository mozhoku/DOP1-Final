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
        if (curr_anim == pc.current_anim_state) {return;}
        animator.Play(pc.current_anim_state);
        curr_anim = pc.current_anim_state;
    }
}
