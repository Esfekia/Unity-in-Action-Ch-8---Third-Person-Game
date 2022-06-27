using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{
    // this script needs a reference to the object to move relative to
    [SerializeField] Transform target;

    private void Update()
    {
        // start with vector (0,0,0) and add movement components progressively.
        Vector3 movement = Vector3.zero;

        // remember getaxis will vary between -1 and 1!
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        //handle movement only while arrow keys are pressed
        if (horInput != 0 || vertInput != 0)
        {
            // for both sideways directions, using -1 and 1
            Vector3 right = target.right;

            // calculate the player's forward direction by using the cross product of the target's right direction
            Vector3 forward = Vector3.Cross(right, Vector3.up);

            // add together the input in each direction to get the combined movement vector
            movement = (right * horInput) + (forward * vertInput);

            // LookRotation() calculates a quaternion facing in that direction
            transform.rotation = Quaternion.LookRotation(movement);
        }

    }
}
