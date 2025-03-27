using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi : MonoBehaviour
{
    public Transform rightHandle;
    public Transform leftHandle;

    public Transform firePoint;

    public ParticleSystem muzzle;
    public ParticleSystem shell;
    public LineRenderer bulletTrajectory;

    public AudioSource uzi;

    public AudioClip shot;
    public AudioClip reload;

    public float attack = 20;
    public float range = 15f;


    public void Fire()
    {
        muzzle.Play();
        shell.Play();

        uzi.PlayOneShot(shot);


        StartCoroutine(FireBullet());
    }
    public void Reload()
    {
        uzi.PlayOneShot(reload);
    }

    IEnumerator FireBullet()
    {
        bulletTrajectory.SetPosition(0, firePoint.position);

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            bulletTrajectory.SetPosition(1, hit.point);
            if (hit.collider.CompareTag("Zombie"))
            {
                Zombie target = hit.collider.GetComponent<Zombie>();
                if (target != null)
                {
                    target.TakeDamage(attack);
                    target.damageEffect(hit);
                }
            }
        }
        else
        {
            bulletTrajectory.SetPosition(1, firePoint.position + firePoint.forward * range);
        }

        bulletTrajectory.enabled = true;
        yield return null;
        bulletTrajectory.enabled = false;
    }
}
