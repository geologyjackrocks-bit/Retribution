using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Hierarchy;
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
    public Sprite[] walkingAnimation;
    private bool isAnimating = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        sr = GetComponent<SpriteRenderer>();
    }
    IEnumerator PlayerAnimation()
    {
        isAnimating = true;
        int animationFrame = 0;
       while (rb.linearVelocityX > 0)
        {
            sr.sprite = walkingAnimation[animationFrame];
            animationFrame++;
            Debug.Log(animationFrame);
            if (animationFrame > 4) animationFrame = 0;
            {
                Debug.Log("moving");
                
                
            }
            yield return new WaitForSeconds(0.1f);
        }
      isAnimating = false;
    }
    void FixedUpdate()
    {
        
        float moveX = 0f;
        float moveY = 0f;
        bool playerOnGround = (playerCollider.IsTouching(groundCollider));
        
        if (playerOnGround)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                rb.linearVelocityY += 5f;
            }
        }

        if (Keyboard.current.sKey.isPressed) moveY -= 1f;
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
        rb.AddForceX(moveX * speed);
        Debug.Log(facingDirection);
        if (moveX != 0f && !isAnimating)
        {
            StartCoroutine(PlayerAnimation());
        }
        if (!playerOnGround)
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (facingDirection == "left")
                {
                    rb.AddForceX(-500f);
                    Debug.Log("dash left");
                }
                if (facingDirection == "right") 
                {
                    rb.AddForceX(500f);
                    Debug.Log("dash right");
                }
            }
        }

    }
}

 