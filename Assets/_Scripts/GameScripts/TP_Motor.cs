using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{
    public static TP_Motor Instance;
    public static float ForwardSpeed = 10f;
    public static float BackwardSpeed = 4f;
    public static float StrafeSpeed = 7f;
    public static float JumpHeigth = 4f;

    public float Gravity = 7f;
    public Vector3 MoveVector { get; set; }
    public float Speed { get; set; }
    public float MaxVelocityChange = 10f;

    internal bool canJump = false;

    void Awake()
    {
        Instance = this;
	}
	
	public void UpdateMotor()
    {
        SnapAllingCharacterWithCamera();
        ProcessMotion();
	}
    void ProcessMotion()
    {
        MoveVector = transform.TransformDirection(MoveVector);   
     
        if (MoveVector.magnitude > 1)
        {
            MoveVector = Vector3.Normalize(MoveVector);
        }
        
        MoveVector *= Speed;
        Vector3 currentVelocity = rigidbody.velocity;
        Vector3 velocityChange = (MoveVector - currentVelocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxVelocityChange, MaxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -MaxVelocityChange, MaxVelocityChange);
        MoveVector = new Vector3(velocityChange.x, 0, velocityChange.z);
        rigidbody.AddForce (MoveVector, ForceMode.VelocityChange);
        
        if(canJump)
        {
            rigidbody.velocity = new Vector3 (currentVelocity.x, CalculateJumpVerticalSpeed(), currentVelocity.z);
            canJump = false;            
        }

        ApplyGravity();
    }

    void ApplyGravity()
    {
        rigidbody.AddForce(new Vector3(0, -Gravity * rigidbody.mass, 0));
    }

    void SnapAllingCharacterWithCamera()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                                              Camera.main.transform.eulerAngles.y,
                                              transform.eulerAngles.z);
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * JumpHeigth * Gravity);
    }
}
