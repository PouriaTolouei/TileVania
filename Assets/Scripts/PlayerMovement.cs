using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isJumping;
    bool isAlive;


    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] float deathKick = 18f;
    [SerializeField] GameObject arrow;

   
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        isAlive = true;
    }

    void Update()
    {
        if (isAlive) {
            Run();
            FlipSprite();
            ClimbLadder();
            CheckJumping();
            Die();
        }
    }

    void OnJump(InputValue value) {
        if (isAlive && value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
            isJumping = true;
        }
    }

    void OnMove(InputValue value) {
        if (isAlive) {
            moveInput = value.Get<Vector2>();
        }        
    }

    void OnAttack(InputValue value){
        if (isAlive && value.isPressed) {
            myAnimator.SetTrigger("isShooting");
            Instantiate(arrow, transform.GetChild(0).position, transform.rotation);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;
        bool playerHasHorizontalMovement = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalMovement);
    }

     void FlipSprite()
    {
        bool playerHasHorizontalMovement = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalMovement) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder() 
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) && !isJumping) {
            myRigidbody.gravityScale = 0;
            Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
            myRigidbody.linearVelocity = climbVelocity;
            bool playerHasVerticalMovement = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("isClimbing", playerHasVerticalMovement);
        } else {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void CheckJumping()
    {
         // Once faling, jumping is done
        if (myRigidbody.linearVelocity.y < 0) {
            isJumping = false;
        }
    }

    void Die() 
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocityY += deathKick;
            StartCoroutine(FindFirstObjectByType<GameSession>().ProcessPlayerDeath());
        }
    }
}
