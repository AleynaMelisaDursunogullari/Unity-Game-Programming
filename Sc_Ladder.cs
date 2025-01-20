using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Ladder : MonoBehaviour
{
    public float vertical;
    private float speed=4f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;

    

    // Update is called once per frame
    void Update()
    {
        vertical= Input.GetAxis("Vertical");

        if(isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing=true;
        }
    }

    private void FixedUpdate()
    {
        if(isClimbing)
        {
            rb.gravityScale=0f;
            rb.velocity=new Vector2(rb.velocity.x,vertical*speed);
        }
        else
        {
            rb.gravityScale=1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("ladder"))
        {
           isLadder=true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
         if(col.CompareTag("ladder"))
         {
           isLadder=false;
           isClimbing=false;
         }
    }
}
