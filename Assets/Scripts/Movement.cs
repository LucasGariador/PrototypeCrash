using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement Controls")]
    [SerializeField]
    MovementController movControls;

    [Header("Dash Controls")]
    [SerializeField]
    float dashForce = 5f;
    private bool canDash = true;
    [SerializeField]
    float dashCoolDown = 3f;

    [Header("Character Movement")]
    [SerializeField]
    float speed;
    [SerializeField]
    float smoothTurnTime = 0.1f;

    private float turnSmoothVelocity;

    [HideInInspector]
    public bool activeMovement = true; //Use to deactivate Player Movement


    void Start()
    {

        movControls.Initialize();
        movControls.Dash += DashForward;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (activeMovement)
        {
            MoveCharacter();
        }
        if(dashCoolDown <= 0)
        {
            canDash = true;
        }
        dashCoolDown -= Time.deltaTime;

        if (rb.velocity.magnitude > 6f && canDash)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 6f);
        }

        if (activeMovement == false)
            Invoke(nameof(ReActivateMovement), 1f);
    }

    protected virtual void DashForward()
    {
        if (canDash)
        {
            canDash = false;
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
            dashCoolDown = 3f;
        }
    }

    private void ReActivateMovement()
    {
        activeMovement = true;
    }
    //Move and rotation of the player, using RigidBody
    private void MoveCharacter()
    {
        if (movControls.MoveDirection.magnitude >= 0.1f)
        {
            float rotAngle = Mathf.Atan2(movControls.MoveDirection.x, movControls.MoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotAngle, ref turnSmoothVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, rotAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * speed * Time.deltaTime, ForceMode.Force);

        }
    }
    //can be improved, if a player collides with other, push him and deactivates his movement for 1 second. Line 60
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && dashCoolDown > 1f)
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            dir.y = 0f;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * dashForce * 2, ForceMode.VelocityChange);
            collision.gameObject.GetComponent<Movement>().activeMovement = false;

            rb.velocity = rb.velocity/2;
        }
    }

}

public abstract class MovementController : MonoBehaviour
{
    public event System.Action Dash;

    public Vector3 MoveDirection;

    public abstract void Initialize();
    protected virtual void OnDash()
    {
        Dash?.Invoke();
    }
}