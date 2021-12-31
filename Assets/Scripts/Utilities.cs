using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public SkeletonEnemy se;
    void DestroyYourself() {
        Destroy(this.gameObject);
    }

    void DisableYourself() {
        this.gameObject.SetActive(false);
    }

    void PlaySkeletonIdleAnimation() {
        this.GetComponent<Animator>().Play("skeleton-idle");
        se.dont_move = false;
    }
}
