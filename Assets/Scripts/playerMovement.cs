using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private float dirX = 0f;

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float jumpForce = 14.0f;
    [SerializeField] private LayerMask jumpableGround;
    private enum MovementState { idle, running, falling, jumping};

    [SerializeField] private AudioSource jumpSoundEffect; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && onGround())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f) {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f) {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f) {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f) {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int) state);
    }

    private bool onGround() {
        // Are we on the ground
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}
