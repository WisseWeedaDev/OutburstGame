using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{
    Rigidbody2D rb2d;

    [SerializeField]
    float BulletLifeLength = 0.5f;

    /*
    [SerializeField]
    GameObject GunGameObject;
    Vector3 GunRotation = new Vector3(transform.rotation.GunGameObject.x, GunGameObject.y, GunGameObject.z);
    */

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("DestroySelf", BulletLifeLength);
    }

    void FixedUpdate()
    {
        //forces that are applied to bullet that makes it move forward
        rb2d.velocity = new Vector2(-20, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
