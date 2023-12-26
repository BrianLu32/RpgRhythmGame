using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    readonly KeyCode sprintKey = KeyCode.LeftShift;
    public float speed = 12f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -19.62f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // Debug.Log("Horizontal: " + horizontal + "; Vertical: " + vertical);

        // Animation Controller
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontal));
        animator.SetFloat("VerticalSpeed", Mathf.Abs(vertical));

        // Check if player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log(isGrounded);
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            animator.SetBool("IsJumping", false);
        }
        if(Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        float sprintMultiplier = Input.GetKey(sprintKey) ? 1.5f : 1f;

        Vector3 horizontalMovement = transform.right * horizontal;

        // Keeps the player oriented towards the camera while being able to move forward based on camera orientation
        Vector3 forwardDirection = new Vector3(0f, 0f, vertical).normalized;
        float targetAngle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        if(forwardDirection.magnitude >= 0.1) {
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(speed * sprintMultiplier * Time.deltaTime * moveDirection.normalized);
            
        }
        controller.Move(speed * sprintMultiplier * Time.deltaTime * horizontalMovement);
    }
}
