using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_script : MonoBehaviour
{
    [SerializeField]
    public float fallMultiplier = 2.5f;

    [SerializeField]
    public float lowJumpMultiplier = 2f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // if player is falling 
        if (rb.velocity.y < 0)
        {
            //apply fall multiplier
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // if player is not falling and not holding space (jumpkey)
        else if (rb.velocity.y > 0 && ! Input.GetKey("space"))
        {
            //apply slower jump multiplier so that when the user holds space the player jumps a little higher
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


}
