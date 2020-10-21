using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_script : MonoBehaviour
{
    Object bulletRef;

    [SerializeField]
    GameObject Bullet_Location;

    [SerializeField]
    GameObject Gunsprite;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRef = Resources.Load("Player_Bullet");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //play shoot animation, implement animation when ready
            GameObject Player_Bullet = (GameObject)Instantiate(bulletRef);
            //start position of bullet is the position of gameobject called "Bullet_Location", this is located at the end of gun sprite
            Player_Bullet.transform.position = new Vector3(Bullet_Location.transform.position.x, Bullet_Location.transform.position.y, 0);
            



            //Wisse advies: maak de collider een trigger want een rigidbody laat ze botsen, Wisse big brain
        }
    }
}
