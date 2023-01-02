using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private LayerMask groundLayer;

    [Header("Sounds Effects")]
    [SerializeField] private AudioSource catchItemSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;


    [Header("Horizontal Movement")]
    [SerializeField] private float normaMoveSpeed = 10f;
    [SerializeField] private float unhealthMoveSpeed = 7f;
    [SerializeField] private float referenceMoveSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool rightDirection=true;


    [Header("Vertical Movement")]
    [SerializeField] private float referenceJumpSpeed;
    [SerializeField] private float normalJumpSpeed = 10f;
    [SerializeField] private float unhealthJumpSpeed = 8f;

    [SerializeField] private bool onGround=false;
    [SerializeField] private float groundLength = 0.11f;//0.981f;
    [SerializeField] private Vector3 colliderOffset;



    private float jumpDelay = 0.25f;
    private float jumpTimer;


    [Header("Physics")]
    [SerializeField] private float referenceMaxSpeed;
    [SerializeField] private float normalMaxSpeed = 8f;
    [SerializeField] private float unhealthMaxSpeed = 6f;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float fallMultiplier = 5f;

    [Header("Current Movement Values")]
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float currentMaxSpeed;
    [SerializeField] private float currentJumpSpeed;

    [Header("Player Velocity")]
    [SerializeField] private float horizontalVelocity;
    [SerializeField] private float verticalVelocity;

    [Header("Items Boost")]
    private Dictionary<string,ItemsClass> itemsBoostDict = new Dictionary<string,ItemsClass>(){
        {"ItemBoostTest", new ItemsClass("ItemBoostTest",4,0.2f,0.2f,0.2f)},
        {"BebidasEnergeticas", new ItemsClass("BebidasEnergéticas",6,0.5f,0.5f,0.5f)},
        {"Cafe", new ItemsClass("Cafe",5,0.2f,0.2f,0.2f)},
        {"Exercicio", new ItemsClass("Exercicio",5,0.2f,0.1f,0.1f)},
        {"FastFood", new ItemsClass("FastFood",3,-0.2f,-0.2f,-0.2f) },

    };


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Start");

        //Current Movement Values
        currentJumpSpeed =normalJumpSpeed;
        currentMoveSpeed=normaMoveSpeed;
        currentMaxSpeed=normalMaxSpeed;

        // Reference Values post Effect
        referenceJumpSpeed=normalJumpSpeed;
        referenceMoveSpeed=normaMoveSpeed;
        referenceMaxSpeed=normalMaxSpeed;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundLayer=LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    private void Update()
    {   
        bool wasOnGround=onGround;

        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        
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

    }

    private void FixedUpdate()
    {

        moveCharacter(direction.x);

        if (jumpTimer > Time.time && onGround)
        {
            Jump();
            jumpSoundEffect.Play();
            this.gameObject.GetComponent<PlayerHealthEnergy>().reduceHealthEnergy("Jump");
        }

        updatePhysics();

        horizontalVelocity = rb.velocity.x;
        verticalVelocity = rb.velocity.y;

    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(itemsBoostDict.ContainsKey(other.gameObject.tag)){

            catchItemSoundEffect.Play();
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
        if (horizontalDirection != 0 & Time.timeScale!=0f)
        {

            animator.SetBool("moving", true);

            if(rb.velocity.x==currentMaxSpeed){
                this.gameObject.GetComponent<PlayerHealthEnergy>().reduceHealthEnergy("Run");
                Debug.Log("Estou me a mover correr");
            }
            else{
                this.gameObject.GetComponent<PlayerHealthEnergy>().reduceHealthEnergy("Normal");
            }
        }
        else
        {
            animator.SetBool("moving", false);

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
        }
        else{
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0){
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

    }

    public void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * currentJumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }
    public void Flip(){
        rightDirection = !rightDirection;
        transform.rotation = Quaternion.Euler(0, rightDirection ? 0 : 180, 0);
    }

}