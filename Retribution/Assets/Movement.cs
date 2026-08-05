using JetBrains.Annotations;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        sr = GetComponent<SpriteRenderer>();    }
    IEnumerator PlayerAnimation()
    {
        isAnimating = true;
        int walkingAnimationFrame = 0;
        int dashingAnimationFrame = 0;
        while (rb.linearVelocityX > 0)
        {
            sr.sprite = rightWalkingAnimation[walkingAnimationFrame];
            walkingAnimationFrame++;
            if (walkingAnimationFrame > 4) walkingAnimationFrame = 0;
            yield return new WaitForSeconds(0.1f);
        }

        while (rb.linearVelocityX < 0)
        {
            sr.sprite = leftWalkingAnimation[walkingAnimationFrame];
            walkingAnimationFrame++;
            if (walkingAnimationFrame > 4) walkingAnimationFrame = 0;
            yield return new WaitForSeconds(0.1f);
        }

        if (rb.linearVelocityX > -0.5 || rb.linearVelocityX < 0.5 || playerOnGround)
        {
            if (facingDirection == "right")
            {
                sr.sprite = rightWalkingAnimation[walkingAnimationFrame];
            }
            if (facingDirection == "left")
            {
                sr.sprite = leftWalkingAnimation[walkingAnimationFrame];
            }
        }


        while (rb.linearVelocityX > 13)
        {
            sr.sprite = rightDashAnimation[dashingAnimationFrame];
            dashingAnimationFrame++;
            if (dashingAnimationFrame > 1) dashingAnimationFrame = 0;
            Debug.Log("somethignidk");
            yield return new WaitForSeconds(0.1f);
        }
        
        while (rb.linearVelocityX < -13)
        {
            sr.sprite = leftDashAnimation[dashingAnimationFrame];
            dashingAnimationFrame++;
            if (dashingAnimationFrame > 1) dashingAnimationFrame = 0;
            Debug.Log("somethignidk");
            yield return new WaitForSeconds(0.1f);
        }

        isAnimating = false;
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
                rb.linearVelocityY = 0.3923998f;
                dashTimer--;
            }
        }

        if (facingDirection == "left")
        {
            while (dashTimer != 0)
            {
                rb.linearVelocityX = -50;
                rb.linearVelocityY = 0.4f;
                dashTimer--;
            }
        }
        isDashing = false;
        yield break;
    }       
    void FixedUpdate()
    {
        playerOnGround = (playerCollider.IsTouching(groundCollider));
        float moveX = 0f;
       
        
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
        Debug.Log(facingDirection);
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
        Debug.Log(playerOnGround);
        Debug.Log("is dashing" + isDashing);
    }
}

 