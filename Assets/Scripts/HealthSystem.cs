using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    public float health;
    public AudioSource audioSource;
    public AudioClip HurtSound;

    public void GetDamaged(float damage) {
        health -= damage;
        audioSource = GetComponent<AudioSource>();
        AudioClip beforeAudio = audioSource.clip;
        audioSource.clip = HurtSound;
        audioSource.Play();
    }

    public void GetDamaged_FLASHFUSED(float damage, SpriteRenderer sr) {
        health -= damage;
        GetHurt(sr);
    }

    protected void GetHurt(SpriteRenderer sr) {
        StartCoroutine(HurtCoroutine(sr));
    }

    IEnumerator HurtCoroutine(SpriteRenderer sr) {
        sr.color = new Color(120,0,0);
        yield return new WaitForSeconds(0.4f);
        sr.color = Color.white;
    }
}
