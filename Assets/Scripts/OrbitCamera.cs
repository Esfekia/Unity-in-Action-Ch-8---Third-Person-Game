using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    // serialized reference to the object to orbit around
    [SerializeField] Transform target;

    public float rotSpeed = 0.2f;

    private float rotY;
    private Vector3 offset;

    private void Start()
    {
        rotY = transform.eulerAngles.y;

        // store the starting position offset between the camera and the target
        offset = target.position - transform.position;
    }
    private void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");
        
        // either rotate the camera slowly using arrow keys .. 
        if (!Mathf.Approximately(horInput,0))
        {
            rotY += horInput * rotSpeed;
        }

        // .. or rotate quickly with the mouse
        else
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed * 10;
        }

        // convert rotation angle to a quaternion
        Quaternion rotation = Quaternion.Euler(0, rotY, 0);

        // maintain the starting offset, shifted according to camera's rotation
        transform.position = target.position - (rotation * offset);
        
        // no matter where the camera is relative to the target, always face the target
        transform.LookAt(target);
    }
}
