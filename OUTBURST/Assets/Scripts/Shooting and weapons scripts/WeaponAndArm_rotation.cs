using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAndArm_rotation : MonoBehaviour
{
    private SpriteRenderer spriterenderer;                          //Make variable to reference sprite renderer
    public GameObject Player;

    private void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();    //Variable is linked to the component spriterenderer so that it can be referenced in this script
    }

    private void FixedUpdate()                              //Because the function uses rotation and physics, fixedupdate is used rather than update
    {
        AimWeaponAtMouse();                                 //Call the function
    }

    private void AimWeaponAtMouse()
    {
        //Creates vector3 that translates screen position of mouse to world position
        
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);         //Mouse Position in game in vector3
        Vector3 difference = MousePos - transform.position;
        //Normalizes the vector to fix some issues
        difference.Normalize();

        //calculates rotation angle and convert it to degrees rather than radians
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        float rotationArm = rotationZ + 180;        //adds 180 to the rotation to fix sprite issues
        //Rotates arm with an Quaternion.Euler because this uses 3 axes, adding 180 to the rotation because the sprite is weird and this is an offset
        transform.rotation = Quaternion.Euler(0f, 0f, rotationArm);

        //If the mousepos.x is bigger than the player.x flip sprite, if smaller flip it the other way, this ensures that the weapon is always the right side up
        if (MousePos.x > Player.transform.position.x)                   
        {
            spriterenderer.flipY = true;
        }
        if (MousePos.x < Player.transform.position.x)
        {
            spriterenderer.flipY = false;
        }


        #region former rotation
        /*
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
        */
        #endregion former rotation

        #region zooi
        /*
        // stupid fucking code does not work piece of shit 
        if (angle * Mathf.Rad2Deg > 180)
        {
            spriterenderer.flipY = false;
        }
        else if (angle * Mathf.Rad2Deg < -180)
        {
            spriterenderer.flipY = true;
        }
          
        Debug.Log(angle);
        
        pseudo code for weapon that flips the right direction when pointing right or left
        if (angle is straight up or straight down)
        {
            flip the sprite on y-axis
        }
         */
        #endregion zooi
    }
}
