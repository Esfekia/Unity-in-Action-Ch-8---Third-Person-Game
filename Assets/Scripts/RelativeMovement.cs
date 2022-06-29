using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    // this script needs a reference to the object to move relative to
    [SerializeField] Transform target;

    // we need to store collusion data between functions
    private ControllerColliderHit contact;

    public float moveSpeed = 6.0f;
    public float rotSpeed = 15.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -20.0f;
    public float minFall = -1.5f;

    private float vertSpeed;

    private CharacterController charController;
    private void Start()
    {
        charController = GetComponent<CharacterController>();

        // initialize the vertical speed to the minimum falling speed at the start of the existing function
        vertSpeed = minFall;
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

            // LookRotation() value is used indirectly as the target direction to rotate toward
            Quaternion direction = Quaternion.LookRotation(movement);
            
            // Quaternion.Lerp() method smoothly changes (interpolates) between the current and target rotations.
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
                        
        }
        bool hitGround = false;
        RaycastHit hit;

        // check if player is falling
        if (vertSpeed <0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // determine distance to check against (extended slightly beyond the bottom of the capsule
            float check = (charController.height + charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }
        
        // instead of using isGrounded, check the raycasting result

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
            }
        }        
        else
        {
            // if not on the ground, apply gravity until terminal velocity is reached
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }

        // if raycasting did not detect ground, but the capsule is touching the ground
        if (charController.isGrounded)
        {
            if (Vector3.Dot(movement, contact.normal) < 0)
            {
                // respond slightly differently depending on whether the character is facing the contact point
                movement = contact.normal * moveSpeed;
            }
            else
            {
                movement += contact.normal * moveSpeed;
            }
        }
        
        movement.y = vertSpeed;
        

        // multiply movement by deltaTime to make it frame-rate independent
        movement *= Time.deltaTime;
        charController.Move(movement);
    }
    // store the collision data in the callback when a collision is detected
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
    }
}
