using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class Woman : MonoBehaviour, ILivingThings
{
    public Rigidbody rb;
    public Animator animator;
    public Uzi uzi;
    public Slider hpBar;
    public AudioSource audioSource;

    public AudioClip damage;
    public AudioClip die;

    public float speed = 5;
    public float turnSpeed = 100;
    public float[] IKWeights = new float[4];

    public float maxHealth = 100f;
    public float health = 100f;

    public float extraAmmo = 210;
    public float currentMaxAmmo = 30;
    public float currentAmmo = 30;


    float delayTime = 0f;
    float shotDelay = 0.075f;

    public bool isDead = false;

    private void Awake()
    {
        hpBar.value = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        float ratio = this.health / this.maxHealth;
        hpBar.value = Mathf.Clamp01(ratio);

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var dir = new Vector3(h, 0, v);

        var movement = transform.position + (dir.z * transform.forward * speed * Time.deltaTime);
        if (movement != Vector3.zero)
        {
            rb.MovePosition(movement);
            animator.SetFloat("Move", v);
        }
        var turn = Quaternion.Euler(0, h * turnSpeed * Time.deltaTime, 0);
        rb.MoveRotation(rb.rotation * turn);

        if (Input.GetMouseButton(0))
        {
            if (currentAmmo > 0)
            {

                if (Time.time - delayTime > shotDelay)
                {
                    Shoot();
                    currentAmmo--;
                    delayTime = Time.time;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Reload");
            StartCoroutine(Reload());

            uzi.Reload();
        }

        GameManager.Instance.bulletText.text = currentAmmo.ToString() + " / " + extraAmmo.ToString();

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isDead) return;

        if (uzi != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeights[0]);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeights[1]);
            animator.SetIKPosition
                (AvatarIKGoal.RightHand, uzi.rightHandle.position);
            animator.SetIKRotation
                (AvatarIKGoal.RightHand, uzi.rightHandle.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeights[2]);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKWeights[3]);
            animator.SetIKPosition
                (AvatarIKGoal.LeftHand, uzi.leftHandle.position);
            animator.SetIKRotation
                (AvatarIKGoal.LeftHand, uzi.leftHandle.rotation);
        }
        else
        {
            Debug.Log("우지가 널임");
        }
    }

    void Shoot()
    {
        uzi.Fire();
        delayTime = Time.time;
    }
    IEnumerator Reload()
    {
        extraAmmo += currentAmmo;
        currentAmmo = 0;
        yield return new WaitForSeconds(0.8f);

        float ammoToLoad = Mathf.Min(extraAmmo, currentMaxAmmo);
        currentAmmo = ammoToLoad;
        extraAmmo -= ammoToLoad;
    }

    public void Die()
    {
        isDead = true;
        audioSource.PlayOneShot(die);
        animator.SetTrigger("Die");
        uzi.gameObject.SetActive(false);
        StartCoroutine(GameManager.Instance.GameOver());
    }

    public void TakeDamage(float attack)
    {
        audioSource.PlayOneShot(damage);
        health -= attack;
        float ratio = this.health / this.maxHealth;
        hpBar.value = Mathf.Clamp01(ratio);
        if (isDead) return;
        if (health <= 0)
        {
            Die();
        }
    }
}
