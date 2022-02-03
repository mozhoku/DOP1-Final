using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossEnemy : Enemy
{
    public GameObject player;
    private Animator anim;
    private GameObject sprite_obj;
    private Vector3 init_scale;

    public Transform[] summon_positions; 
    public GameObject Summons;

    public Slider slid;

    // Start is called before the first frame update
    protected override void Start()
    {
        sprite_obj = transform.GetChild(0).gameObject;
        base.Start();
        anim = sprite_obj.GetComponent<Animator>();
        init_scale = transform.localScale;
        StartCoroutine(ScytheAttackCoroutine());
        StartCoroutine(Summoning());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float xdirect = (player.transform.position - this.transform.position).x;
        if (xdirect < 0) {
            transform.localScale = new Vector3(-init_scale.x, init_scale.x,init_scale.x);
        } else {
            transform.localScale = init_scale;
        }
        slid.value = CalculateHealth();
        
               
    }

    float CalculateHealth() {
        return health / max_health;
    }

    IEnumerator Summoning() {
        while (true) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack_scythe")) {yield return new WaitForSeconds(0.5f);continue;}
            int RandomNumber = Random.Range(0, 101);
            if (Vector3.Distance(player.transform.position, transform.position) >= 5.0f && RandomNumber > 70.0f) {
                anim.SetTrigger("summonspirit");
                SummonSpirits();
                yield return new WaitForSeconds(10.0f);
            } else {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void SummonSpirits() {
        foreach (Transform pos in summon_positions) {
            Instantiate(Summons, pos.position, Quaternion.identity);
        }
    }

    IEnumerator ScytheAttackCoroutine() {
        while (true) {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (Vector3.Distance(player.transform.position, transform.position) < 5.0f && !state.IsName("summonspirit") && !state.IsName("attack_scythe")) {
                anim.SetTrigger("attack_scythe");  
                GameObject transformray = sprite_obj.transform.GetChild(0).gameObject;
                Collider2D[] collided = Physics2D.OverlapCircleAll(transformray.transform.position, 1.0f, LayerMask.GetMask("Player"));
                // anim.ResetTrigger("attack_scythe");
                if (collided.Length == 0) {yield return new WaitForSeconds(0.01f);continue;}
                collided[0].gameObject.GetComponent<PlayerController>().PlayerGetHurt(10.0f);
                anim.ResetTrigger("attack_scythe");

                yield return new WaitForSeconds(5.0f);
                continue;
            } else {
                anim.ResetTrigger("attack_scythe");
                yield return new WaitForSeconds(0.1f);
            }
        }
        
    }
}
