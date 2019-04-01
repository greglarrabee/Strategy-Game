using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    //20 units per second seems to be a good movement speed
    float MoveSpeed = 20.0f;
    //5 degrees per second seems to be a good rotation speed
    float RotateSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //WASD keys each move the camera in the expected direction, forward, left, back, right respectively
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed, Space.World);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed, Space.World);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed, Space.World);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed, Space.World);
        }

        if (Input.GetKey("up"))
        {
            transform.Translate(Vector3.up * Time.deltaTime * MoveSpeed, Space.World);
        }
        if (Input.GetKey("down"))
        {
            if(this.transform.position.y > 40)
            {
                transform.Translate(Vector3.down * Time.deltaTime * MoveSpeed, Space.World);
            }
        }

        //q key rotates left, relative to the world not the camera opject's rotation
        if (Input.GetKey("q"))
        {
            transform.Rotate(0, -RotateSpeed,0, Space.World);
        }
        //e key rotates right, relative to the world not the camera opject's rotation
        if (Input.GetKey("e"))
        {
            transform.Rotate(0, RotateSpeed,0, Space.World);
        }

    }
}
