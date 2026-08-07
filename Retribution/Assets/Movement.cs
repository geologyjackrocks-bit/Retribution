using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Hierarchy;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D playerCollider;
    public Collider2D groundCollider;
    string facingDirection = "nothing"; //for checking the direction of the character when needing to dash or animate sprites
    public Sprite[] rightWalkingAnimation;
    public Sprite[] leftWalkingAnimation;
    public Sprite[] rightDashAnimation;
    public Sprite[] leftDashAnimation;
    private bool isAnimating = false;
    private bool isDashing = false;
    private bool playerOnGround;
    private int walkingAnimationFrame;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        sr = GetComponent<SpriteRenderer>();    }
    IEnumerator PlayerAnimation()
    {
        isAnimating = true; // lets the rest of the script know that it is animating and not to start the coroutine again
        walkingAnimationFrame = 0;
        int dashingAnimationFrame = 0;
        while (rb.linearVelocityX != 0 && rb.linearVelocityX <= 13 && facingDirection == "right") // looking for right walk
        {
            sr.sprite = rightWalkingAnimation[walkingAnimationFrame];
            walkingAnimationFrame++;
            if (walkingAnimationFrame > 4) walkingAnimationFrame = 0;
            yield return new WaitForSeconds(0.11f);
        }

        while (rb.linearVelocityX != 0 && rb.linearVelocityX >= -13 && facingDirection == "left") // looking for left walk
        {
            sr.sprite = leftWalkingAnimation[walkingAnimationFrame];
            walkingAnimationFrame++;
            if (walkingAnimationFrame > 4) walkingAnimationFrame = 0;
            yield return new WaitForSeconds(0.11f);
        }

        while (rb.linearVelocityX > 13) // looking for right dash
        {
            sr.sprite = rightDashAnimation[dashingAnimationFrame];
            dashingAnimationFrame++;
            if (dashingAnimationFrame > 1) dashingAnimationFrame = 0;
            yield return new WaitForSeconds(0.1f);
        }
        
        while (rb.linearVelocityX < -13) // looking for left dash
        {
            sr.sprite = leftDashAnimation[dashingAnimationFrame];
            dashingAnimationFrame++;
            if (dashingAnimationFrame > 1) dashingAnimationFrame = 0;
            yield return new WaitForSeconds(0.1f);
        }

        isAnimating = false; // lets the rest of the script know that it is no longer animating and can start the coroutine again
    }
    IEnumerator Dashing()
    {
        isDashing = true;
        int dashTimer = 20;
        if (facingDirection == "right")
        {
            while (dashTimer != 0)
            {
                rb.linearVelocityX = 50;
                rb.linearVelocityY = 0.3923998f; // this number is required to be exactly that to keep the linearVelocityY exactly 0, it wont say 0 idk why it does that but it is actually 0
                dashTimer--;
            }
        }

        if (facingDirection == "left")
        {
            while (dashTimer != 0)
            {
                rb.linearVelocityX = -50;
                rb.linearVelocityY = 0.3923998f; // note on line 83 applies here as well
                dashTimer--;
            }
        }
        isDashing = false;
        yield break;
    }       
    void FixedUpdate()
    {
        playerOnGround = (playerCollider.IsTouching(groundCollider));
        float moveX = 0f; // i dont think this is actually nessicarry for the movement left to right but it doesnt work quite right if i do it differently so idk

        if ((rb.linearVelocityX > -0.5 && rb.linearVelocityX < 0.5) || playerOnGround)
        {
            if (facingDirection == "right")
            {
                sr.sprite = rightWalkingAnimation[walkingAnimationFrame];
                StopCoroutine(Dashing());
                StopCoroutine(PlayerAnimation());
            }
            if (facingDirection == "left")
            {
                sr.sprite = leftWalkingAnimation[walkingAnimationFrame];
                StopCoroutine(Dashing());
                StopCoroutine(PlayerAnimation());
            }
        }

        if (playerOnGround)
        {
            if (Keyboard.current.wKey.isPressed) rb.linearVelocityY += 5f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            moveX -= 1f;
            facingDirection = "left";
        }
        if (Keyboard.current.dKey.isPressed)
        {
            moveX += 1f;
            facingDirection = "right";
        }
        if (rb.linearVelocityX < 12 && rb.linearVelocityX > -12) rb.AddForceX(moveX * speed);
        if (playerOnGround && rb.linearVelocityX > 12) rb.linearVelocityX = 12;
        if (playerOnGround && rb.linearVelocityX < -12) rb.linearVelocityX = -12;
        Debug.Log("facingDirection="+facingDirection);
        if (moveX != 0f && !isAnimating)
        {
            StartCoroutine(PlayerAnimation());
        }
        if (!playerOnGround)
        {
            if (Keyboard.current.iKey.isPressed && !isDashing)
            {
                
                StartCoroutine(Dashing());
                
            }
        }
        Debug.Log("playerOnGround="+playerOnGround);
        Debug.Log("isDashing=" + isDashing);
    }
}

 