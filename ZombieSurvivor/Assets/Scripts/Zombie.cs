using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class Zombie : MonoBehaviour, ILivingThings
{
    public ZombieSO zombieSO;

    public Woman target;
    public NavMeshAgent agent;
    public Animator animator;

    public Transform hitPoint;
    public ParticleSystem blood;

    public AudioSource audioSource;

    public AudioClip damage;
    public AudioClip die;

    public float attackDelay = 1f;
    private float lastAttackTime = 0;

    private float maxHealth;
    private float health;
    private float attack;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        Renderer meshRenderer = GetComponentInChildren<Renderer>();
        meshRenderer.material.color = zombieSO.skinColor;

        maxHealth = zombieSO.maxHealth;
        health = maxHealth;
        agent.speed = zombieSO.speed;
        attack = zombieSO.attack;

        GameObject woman = GameObject.FindWithTag("Player");
        if (woman != null)
        {
            target = woman.GetComponent<Woman>();
        }
    }

    void Update()
    {
        if (isDead) return;

        if (target != null)
        {
            if (!target.isDead && !isAttacking)
            {
                Chase();
            }
            else if (target.isDead)
            {
                Idle();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("충돌");

        if (Time.time - lastAttackTime >= attackDelay)
        {
            if (other.CompareTag("Player"))
            {
                Attack(other.gameObject);
                lastAttackTime = Time.time;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isAttacking = false;
    }

    void Attack(GameObject target)
    {
        Debug.Log("공격");
        isAttacking = true;
        Idle();
        target.GetComponent<Woman>().TakeDamage(this.attack);

    }

    void Chase()
    {
        animator.SetBool("Chase", true);
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
    }
    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("Chase", false);
    }

    public void Die()
    {
        audioSource.PlayOneShot(die);
        StartCoroutine(PostDie());
        GameManager.Instance.zombieAmount -= 1;
        GameManager.Instance.score++;
    }

    public void TakeDamage(float attack)
    {
        audioSource.PlayOneShot(damage);
        health -= attack;
        float ratio = this.health / this.maxHealth;
        if (health <= 0)
        {
            Die();
        }
    }
    public void damageEffect(RaycastHit hit)
    {
        hitPoint.rotation = Quaternion.LookRotation(hit.normal);
        blood.Play();
    }
    IEnumerator PostDie()
    {
        isDead = true;
        agent.enabled = false;
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
