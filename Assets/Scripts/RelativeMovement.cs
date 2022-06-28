using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    // this script needs a reference to the object to move relative to
    [SerializeField] Transform target;
        
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    private CharacterController charController;
    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

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

            // the facing directions are magnitude 1, so multiply with the desired speed value
            movement *= moveSpeed;

            // limit diagonal movement to the same speed as movement along an axis
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            // multiply movement by deltaTime to make it frame-rate independent
            movement *= Time.deltaTime;
            charController.Move(movement);


            // LookRotation() value is used indirectly as the target direction to rotate toward
            Quaternion direction = Quaternion.LookRotation(movement);
            
            // Quaternion.Lerp() method smoothly changes (interpolates) between the current and target rotations.
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

    }
}
