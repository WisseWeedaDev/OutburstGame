using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    Vector3 target, mousePos, refVel, ShakeOffset;

    float CameraDistance = 3.5f;
    float SmoothTime = 0.2f, zStart;

    [SerializeField]                    //4 limits for all the sides, creates a box for the camera to move in, also drawn in gizmos
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottonLimit;
    [SerializeField]
    float topLimit;

    void Start()
    {
        target = player.position;
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPosition();

        //Clamps the location of the camera between values and ignores the z coordinate, this cause the camera to never show the end of the levels
        transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottonLimit, topLimit),
                transform.position.z
            );
    }

    Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
        ret *= 2;
        ret -= Vector2.one; //set (0,0) of mouse to middle of screen
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized; //helps smooth near edges of screen
        }
        return ret;
    }

    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * CameraDistance;
        Vector3 ret = player.position + mouseOffset;
        ret.z = zStart;
        return ret;
    }
    void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, SmoothTime); //smoothly move towards the target
        transform.position = tempPos; //update the position
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottonLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, bottonLimit), new Vector2(leftLimit, bottonLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottonLimit), new Vector2(leftLimit, topLimit));
    }
}
