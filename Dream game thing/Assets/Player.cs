using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 1f;
    private float verticalInput;
    private float horizontalInput;

    public Transform lookDirection;
    public float GroundDrag = 10;

    private bool OnGround;

    public float jumpPower = 10;
    public float airMultiplier;
    public float JumpCoolDown;
    bool canJump = true;

    public Vector3 moveDirection;
    RaycastHit hit;

    void Start()
    {

    }


    private void Update()
    {
        CheckOnGround();
        VelocityLimiter();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (OnGround)
        {
            rb.drag = GroundDrag;
        }
        else
        {
            rb.drag = 0;

        }
    }



    // Functions below!!!!
    private void MovePlayer()
    {
        verticalInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = Input.GetAxisRaw("Vertical");

        moveDirection = verticalInput * lookDirection.right + lookDirection.forward * horizontalInput;

        if (OnGround)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speed  * airMultiplier, ForceMode.Force);
        }

        Debug.Log(OnGround);
        if (OnGround)
        {
            if (Input.GetKey("space") && canJump)
            {
                canJump = false;
                jump();
                Invoke(nameof(resetJumpcd), JumpCoolDown);
                }
        }
    }


    private void CheckOnGround()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.distance);
            if (hit.distance > transform.localScale.y + 0.1)
            {
                OnGround = false;
            }
            else
            {
                OnGround = true;
            }
        }
    }
    void VelocityLimiter()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 LimitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(LimitedVel.x, rb.velocity.y, LimitedVel.z);
        }
    }
    private void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    private void resetJumpcd()
    {
        canJump = true;
    }


   


        }
 

