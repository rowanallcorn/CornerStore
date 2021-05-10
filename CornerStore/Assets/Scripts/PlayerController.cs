using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Configuration Variables
    /// </summary>
    //Max Slope; 
    [Range(5f, 60f)]
    public float slopeLimit = 60f;
    //Move speed; Meters/Second
    public float moveSpeed = 2f;
    //Turn speed; Degrees/Second
    public float turnSpeed = 300;
    //Can player jump
    public bool allowJump = false;
    //Jump speed; Meters/Second
    public float jumpSpeed = 4f;

    /// <summary>
    /// Public Accessors
    /// </summary>
    //Not grounded if jumping, falling, or too steep slope
    public bool IsGrounded { get; private set; }
    //Controls forward movement, -1 is backwards and 1 is forward
    public float ForwardInput { get; set; }
    //Controls turning movement -1 is right and 1 is left
    public float TurnInput { get; set; }
    //Controls jump
    public bool JumpInput { get; set; }

    /// <summary>
    /// Private Variables
    /// </summary>
    new private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    /// <summary>
    /// Functions
    /// </summary>
    //Awake is called when the script instance is being loaded
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    //FixedUpdate is called every .02 seconds along with unity physics updates
    private void FixedUpdate()
    {
        CheckGrounded();
        ProcessActions();
    }

    //Checks if player is grounded and updates IsGrounded
    private void CheckGrounded()
    {
        IsGrounded = false;
        float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        Vector3 capsuleBottom = transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            float normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit)
            {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    IsGrounded = true;
            }
        }
    }

    //Turns inputs into movement
    private void ProcessActions()
    {
        //Turning
        if (TurnInput != 0f)
        {
            float angle = Mathf.Clamp(TurnInput, -1f, 1f) * turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }

        //Movement
        Vector3 move = transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + move);

        //Jump
        if (JumpInput && allowJump && IsGrounded)
        {
            rigidbody.AddForce(transform.up * jumpSpeed, ForceMode.VelocityChange);
        }
    }
}
