using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private float normalJump=14f;

    private float normalVelocity=7f;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Hello, world!");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump")){
            rb.velocity = new Vector3(0,normalVelocity,0);
        }
        
    }

}