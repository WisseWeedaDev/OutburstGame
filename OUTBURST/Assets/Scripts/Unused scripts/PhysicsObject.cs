using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    protected bool isGrounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferlist = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;


    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }
    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

        isGrounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 move = Vector2.up* deltaPosition.y;

        Movement(move, true);

    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
           int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferlist.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferlist.Add(hitBuffer[i]);
            }
            
            for(int i = 0; i < hitBufferlist.Count; i++)
            {
                Vector2 currentNormal = hitBufferlist [i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
            }

        }

        rb2d.position = rb2d.position + move;
    }
}
