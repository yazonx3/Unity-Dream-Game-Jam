using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private bool exitingSlope;

    public float playerHeight;
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;
    public KeyCode crouchKey = KeyCode.LeftControl;


    public KeyCode sprintKey = KeyCode.LeftShift;
    public movementState state;
    public enum movementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private float speed;
    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;

    public Rigidbody rb;
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
        startYScale = transform.localScale.y;
    }


    private void Update()
    {
        CheckOnGround();
        VelocityLimiter();
        stateHandler();
        //Debug.Log(moveDirection);
        if (OnGround)
        {
            rb.drag = GroundDrag;
        }
        else
        {
            rb.drag = 0;

        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        
    }



    // Functions below!!!!

    private void stateHandler()
    {
        if(Input.GetKey(crouchKey))
        {
            state = movementState.crouching;
            speed = crouchSpeed;
        }

        if(OnGround && Input.GetKey(sprintKey))
        {
            state = movementState.sprinting;
            speed = sprintSpeed;
        }
        
        else if (OnGround) 
        {
            state = movementState.walking;
            speed = walkSpeed;
        }

        else
        {
            state = movementState.air;
        }
    }
    private void MovePlayer()
    {
        verticalInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = Input.GetAxisRaw("Vertical");

        moveDirection = verticalInput * lookDirection.right + lookDirection.forward * horizontalInput;


        rb.useGravity = !OnSlope();
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * speed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (OnGround)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speed  * airMultiplier, ForceMode.Force);
        }

        //Debug.Log(OnGround);
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
            //Debug.Log(hit.distance);
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
        //Debug.Log("Called Velocity Limiter");
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > speed)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }

        else //(flatVel.magnitude > speed)
        {
            Vector3 LimitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(LimitedVel.x, rb.velocity.y, LimitedVel.z);

        }
        
    }
    private void jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    private void resetJumpcd()
    {
        canJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
   


    }
 

