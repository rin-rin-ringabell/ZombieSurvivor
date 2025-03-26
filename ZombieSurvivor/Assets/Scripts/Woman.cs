using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Woman : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;

    public float speed = 5;
    public float turnSpeed = 100;


    void Update()
    {
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
    }
}
