using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Game components are named to be referenced later
    Rigidbody2D rb2d;
    SpriteRenderer spriterenderer;

    //most of the variables are serialized to easily acces them in the unity editor rather than changing them in the script

    //boolean for checking if the player is grounded, true means grounded, false means not grounded
    bool isGrounded;

    //3 transforms for drawing a linecast to check if grounded, 3 is more accurate than only one
    [SerializeField]
    Transform GroundCheckMiddle;

    [SerializeField]
    Transform GroundCheckRight;

    [SerializeField]
    Transform GroundCheckLeft;

    //Float to check and control the horizontal velocity of the player
    public float horizontalVelocity;

    //Velocity acceleration to control the speed of player
    [SerializeField]
    public float VelocityAcceleration;

    //Floats for damping the player velocity on specific moments
    [SerializeField]
    [Range(0, 1)]
    float horizontalDampingWhenStarting = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    float horizontalDampingWhenStopping = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    float horizontalDampingWhenTurning = 0.5f;

    // float for how high the player jumps
    [SerializeField]
    private float jumpSpeed = 5f;

    //float for how much the jumpheight needs to be cut
    [SerializeField]
    [Range(0, 1)]
    float cutJumpHeight = 0.5f;
   

    //float for timer to check how long ago the player jumped
    float jumpPressedRemember = 0;
    [SerializeField]
    //float for the time player has to jump again while in the air, so that it registers earlier
    float jumpPressedRememberTime = 0.2f;

    //float for the time that is remembered that the player was grounded
    float groundedRemember = 0;
    [SerializeField]
    //float for the time that is remembered that the player touched ground
    float groundedRememberTime = 0.2f;

    // Start is called before the first frame update

    void Start()
    {
        //linking the components to the names
        spriterenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();                                                          
    }
    

    // most of the physics is in the fixedupdate so that the physics is not dependent on the frame rate of the game
    void FixedUpdate()
    {
        // [JUMP]
        // checks if player is grounded by drawing 3 linecasts, if one of them is true, the other options are not checked, therefore the || is used
        // if true player is grounded so isgrounded is true, if false player is not grounded so isgrounded is false
        if ((Physics2D.Linecast(transform.position, GroundCheckMiddle.position, 1 << LayerMask.NameToLayer("Ground"))) ||
            (Physics2D.Linecast(transform.position, GroundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"))) ||
            (Physics2D.Linecast(transform.position, GroundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        // allow the player to press jump again in the air, so that when the player does that the game remembers and the player jumps again this makes it feel more responsive
        // this statement checks if the user has pressed the jump button in the last 0.2 seconds, if that is the case and the player is grounded the player jumps
        groundedRemember -= Time.deltaTime;
        if (isGrounded)
        {
            groundedRemember = groundedRememberTime;
        }

        //if the player has been on ground and jumped in a certain amount of time the the player can jump
        if ((jumpPressedRemember > 0) && (groundedRemember > 0))
        {
            groundedRemember = 0;
            jumpPressedRemember = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        //Links the horizontalvelocity to rigidbody speed on x axis
        horizontalVelocity = rb2d.velocity.x;
        //Left input means -1 and right input means 1
        horizontalVelocity += Input.GetAxisRaw("Horizontal") * VelocityAcceleration;

        //Checks if there is a input on the x axis, if not plays the idle animation
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            //play idle animation booy ya
        }

        //Flips the sprite the right way
        if (horizontalVelocity > 0)
        {
            spriterenderer.flipX = true;
        }
        else if (horizontalVelocity < 0)
        {
            spriterenderer.flipX = false;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.deltaTime * 10f);
        else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(horizontalVelocity))
           horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.deltaTime * 10f);
        else
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStarting, Time.deltaTime * 10f);

        rb2d.velocity = new Vector2(horizontalVelocity, rb2d.velocity.y);
    }

    void Update()
    {
        //if space is pressed the timer resets
        jumpPressedRemember -= Time.deltaTime;
        if (Input.GetKeyDown("space"))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }
        if (Input.GetKeyUp("space"))
        {
            if (rb2d.velocity.y > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * cutJumpHeight);
            }
        }
    }
}
