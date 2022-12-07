using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    [Header("Horizontal Movement")]
    public float moveSpeed=4f;
    public Vector2 direction;
    public bool rightDirection=true;


    [Header("Vertical Movement")]
    public bool onGround=false;
    public float groundLength = 0.981f; 
    public Vector3 colliderOffset;

    public float jumpSpeed=10f;
    public float jumpDelay=0.25f;
    public float jumpTimer;


    [Header("Physics")]
    public float maxSpeed= 10f;
    public float linearDrag=4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    



    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Hello, world!");
        rb = GetComponent<Rigidbody2D>();
        groundLayer=LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    private void Update()
    {   
        bool wasOnGround=onGround;

        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        
        if(!wasOnGround && onGround){
            //StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }

        if(Input.GetButtonDown("Jump")){
            jumpTimer = Time.time + jumpDelay;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        moveCharacter(direction.x);
        
        if(jumpTimer > Time.time && onGround){
            Jump();
        }

        detectPlayerMovement();

        updatePhysics();
    }

    public void detectPlayerMovement(){
        
        /**
        * TODO: Verificar melhor mas parece estar a detetar corretamente
        */

        if(Mathf.Abs(rb.velocity.y)!=0){
            Debug.Log("Estou me a mover Saltar");
        }

        if(Mathf.Abs(rb.velocity.x)!=0 && rb.velocity.x < maxSpeed){
            Debug.Log("Estou me a mover andar");
        }

        if(Mathf.Abs(rb.velocity.x)!=0 && rb.velocity.x == maxSpeed){
            Debug.Log("Estou me a mover correr");
        }

    }
    public void moveCharacter(float horizontalDirection){
        
        rb.AddForce(Vector2.right * horizontalDirection * moveSpeed);

        if((horizontalDirection > 0 && !rightDirection) || (horizontalDirection < 0 && rightDirection)){
            Flip();
        }

        if(Mathf.Abs(rb.velocity.x) > maxSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        ///animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
    }

    void updatePhysics(){
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) ||  (direction.x < 0 && rb.velocity.x > 0);
        if(onGround){
            if(Mathf.Abs(direction.x) < 0.4f || changingDirection){
                rb.drag = linearDrag;
            } else{
                rb.drag = 0f;
            }
            rb.gravityScale=0;
        } else{
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0){
                rb.gravityScale = gravity * fallMultiplier;
            }else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

    }

    public void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    public void Flip(){
        rightDirection = !rightDirection;
        transform.rotation = Quaternion.Euler(0, rightDirection ? 0 : 180, 0);
    }

}