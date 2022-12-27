using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    [Header("Horizontal Movement")]
    public float normaMoveSpeed = 4f;
    public float unhealthMoveSpeed = 2f;
    public float referenceMoveSpeed;
    public Vector2 direction;
    public bool rightDirection=true;


    [Header("Vertical Movement")]
    public bool onGround=false;
    public float groundLength = 0.981f; 
    public Vector3 colliderOffset;

    public float referenceJumpSpeed;

    public float normalJumpSpeed = 10f;
    public float unhealthJumpSpeed = 7f;
    public float jumpDelay = 0.25f;
    public float jumpTimer;


    [Header("Physics")]
    public float referenceMaxSpeed;
    public float normalMaxSpeed = 10f;
    public float unhealthMaxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Current Movement Values")]
    public float currentMoveSpeed;
    public float currentMaxSpeed;
    public float currentJumpSpeed;

    [Header("Items Boost")]
    public Dictionary<string,ItemsClass> itemsBoostDict = new Dictionary<string,ItemsClass>(){
        {"ItemBoostTest", new ItemsClass("ItemBoostTest",4,0.2f,0.2f,0.2f)},
        {"BebidasEnergéticas", new ItemsClass("BebidasEnergéticas",6,0.5f,0.5f,0.5f)},
        {"Cafe", new ItemsClass("Cafe",5,0.2f,0.2f,0.2f)},
        {"Exercicio", new ItemsClass("Exercicio",5,0.2f,0.1f,0.1f)},
        {"FastFood", new ItemsClass("FastFood",3,-0.2f,-0.2f,-0.2f) },

    };


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Hello, world!");
        
        //Current Movement Values
        currentJumpSpeed=normalJumpSpeed;
        currentMoveSpeed=normaMoveSpeed;
        currentMaxSpeed=normalMaxSpeed;

        // Reference Values post Effect
        referenceJumpSpeed=normalJumpSpeed;
        referenceMoveSpeed=normaMoveSpeed;
        referenceMaxSpeed=normalMaxSpeed;

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

        // Check State of Health and Change Reference Speeds
        if (this.gameObject.GetComponent<PlayerHealthEnergy>().isHealthy())
        {
            referenceJumpSpeed = normalJumpSpeed;
            referenceMoveSpeed = normaMoveSpeed;
            referenceMaxSpeed = normalMaxSpeed;

        }
        else
        {
            referenceJumpSpeed = unhealthJumpSpeed;
            referenceMoveSpeed = unhealthMoveSpeed;
            referenceMaxSpeed = unhealthMaxSpeed;
        }

        foreach (string tag in itemsBoostDict.Keys)
        {
            if(itemsBoostDict[tag].EffectApplied){
                itemsBoostDict[tag].EffectTimer+=Time.deltaTime;
                if(itemsBoostDict[tag].EffectTimer > itemsBoostDict[tag].EffectDuration){
                    Debug.Log("Item " + itemsBoostDict[tag].TagName + "Effect "+ tag + " End!!");
                    itemsBoostDict[tag].EffectApplied=false;
                    itemsBoostDict[tag].EffectTimer=0;
                    
                    
                    // Max Speed
                    if(((currentMaxSpeed-currentMaxSpeed*itemsBoostDict[tag].EffectMaxSpeed)<referenceMaxSpeed)){
                        currentMaxSpeed=referenceMaxSpeed;
                    }
                    else{
                        currentMaxSpeed-=currentMaxSpeed*itemsBoostDict[tag].EffectMaxSpeed;
                    }

                    // Move Speed
                    if((currentMoveSpeed-currentMoveSpeed*itemsBoostDict[tag].EffectMoveSpeed)<referenceMoveSpeed){
                        currentMoveSpeed=referenceMoveSpeed;
                    }
                    else{
                        currentMoveSpeed-=currentMoveSpeed*itemsBoostDict[tag].EffectMoveSpeed;
                    }

                    // Jump Speed
                    if((currentJumpSpeed-currentJumpSpeed*itemsBoostDict[tag].EffectJumpSpeed)<referenceJumpSpeed){
                        currentJumpSpeed=referenceJumpSpeed;
                    }
                    else{
                        currentJumpSpeed-=currentJumpSpeed*itemsBoostDict[tag].EffectJumpSpeed;
                    }
                    
                }
            }
        }

        // Verificar se a Speeds são atualizadas dependendo da vida e se não estiverem sub efeito
        if(!someEffectApplied() & (currentMoveSpeed!=referenceMoveSpeed || currentMaxSpeed != referenceMaxSpeed || currentJumpSpeed != referenceJumpSpeed))
        {
            currentMoveSpeed = referenceMoveSpeed;
            currentJumpSpeed = referenceJumpSpeed;
            currentMaxSpeed = referenceMaxSpeed;
        }


        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        detectPlayerHorizontalMovement(direction.x);
        
        moveCharacter(direction.x);
        
        if(jumpTimer > Time.time && onGround){
            Jump();
            this.gameObject.GetComponent<PlayerHealthEnergy>().reduceHealthEnergy("Jump");
        }

        updatePhysics();

    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(itemsBoostDict.ContainsKey(other.gameObject.tag)){

            // Restart Timer
            if (itemsBoostDict[other.gameObject.tag].EffectApplied)
            {
                itemsBoostDict[other.gameObject.tag].EffectTimer = 0;
            }

            itemsBoostDict[other.gameObject.tag].EffectApplied=true;

            currentMaxSpeed+=currentMaxSpeed*itemsBoostDict[other.gameObject.tag].EffectMaxSpeed;
            currentMoveSpeed+=currentMoveSpeed*itemsBoostDict[other.gameObject.tag].EffectMoveSpeed;
            currentJumpSpeed+=currentJumpSpeed*itemsBoostDict[other.gameObject.tag].EffectJumpSpeed;
            Debug.Log("Colisão com item " + other.gameObject.tag + "Effect Activated!!");
            Destroy(other.gameObject);

        }
        
    }

    private bool someEffectApplied()
    {
        foreach (string tag in itemsBoostDict.Keys)
        {
            if (itemsBoostDict[tag].EffectApplied)
            {
                return true;
            }
        }

        return false;
    }

    public void detectPlayerHorizontalMovement(float horizontalDirection){
        //TODO: Parece estar a funcionar
        if (horizontalDirection != 0){
            if(rb.velocity.x==currentMaxSpeed){
                this.gameObject.GetComponent<PlayerHealthEnergy>().reduceHealthEnergy("Run");
                Debug.Log("Estou me a mover correr");
            }
            else{
                this.gameObject.GetComponent<PlayerHealthEnergy>().reduceHealthEnergy("Normal");
            }
        }
    }

    public void moveCharacter(float horizontalDirection){
        
        rb.AddForce(Vector2.right * horizontalDirection * currentMoveSpeed);

        if((horizontalDirection > 0 && !rightDirection) || (horizontalDirection < 0 && rightDirection)){
            Flip();
        }

        if(Mathf.Abs(rb.velocity.x) > currentMaxSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * currentMaxSpeed, rb.velocity.y);
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
        rb.AddForce(Vector2.up * currentJumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    public void Flip(){
        rightDirection = !rightDirection;
        transform.rotation = Quaternion.Euler(0, rightDirection ? 0 : 180, 0);
    }

}