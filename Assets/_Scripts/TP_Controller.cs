using System.Collections;
using UnityEngine;

public class TP_Controller : MonoBehaviour 
{
    public static TP_Controller Instance;

    private CapsuleCollider playerCC;
    private float distanceToGround;

	void Awake() 
    {
        playerCC = GetComponent<CapsuleCollider>();
        Instance = this;
        distanceToGround = playerCC.bounds.extents.y;
	}
	
    void FixedUpdate()
    {
        HandleInput();
        TP_Motor.Instance.UpdateMotor();
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, distanceToGround + .3f);
    }

    void HandleInput()
    {
        float deadZone = 0.1f;
        //TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
        Vector3 MoveVector = new Vector3(0, 0, 0);

        if (Input.GetAxis("Vertical") > deadZone || Input.GetAxis("Vertical") < -deadZone)
        {
            MoveVector += new Vector3(0, 0, Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone)
        {
            MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        }

        if (Input.GetButton("Jump") && IsGrounded())
        {
            TP_Motor.Instance.canJump = true;
        }

        UpdateMovementState(MoveVector);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            UpdateMovementState(KeyCode.LeftControl);
            MoveVector = Vector3.zero;
        }       

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (TP_Animator.Instance.MovementState != TP_Animator.JumpState &&
               TP_Animator.Instance.WeaponState != TP_Animator.ReloadState)
            {
                TP_Animator.Instance.WeaponState = TP_Animator.AimState;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            TP_Animator.Instance.WeaponState = TP_Animator.NewState;
        }

        ApplySpeedAndDirection(MoveVector);
    }
    
    public void UpdateWeaponState(int currentAmmoCount)
    {
        if (currentAmmoCount <= 0 && TP_Animator.Instance.WeaponState != TP_Animator.ReloadState)
        {
            TP_Animator.Instance.WeaponState = TP_Animator.NewState;
            if (Input.GetKeyDown(KeyCode.R))
            {
                TP_Animator.Instance.WeaponState = TP_Animator.ReloadState;
            }
        }
    }

    public void CompleteWeaponReload()
    {
        Aiming.Instance.ReloadWeapon();
        TP_Animator.Instance.WeaponState = TP_Animator.NewState;
    }

    void UpdateMovementState(KeyCode keyCode)
    {
        if (keyCode == KeyCode.LeftControl)
        {
            TP_Animator.Instance.MovementState = TP_Animator.CrouchState;            
        }
    }

    void UpdateMovementState(Vector3 MoveVectorFromController)
    {
        int movementState = TP_Animator.Instance.MovementState;
        if (MoveVectorFromController.magnitude <= 0)
        {           
            movementState = TP_Animator.IdleState;
        }

        if (rigidbody.velocity.y > 4f && !IsGrounded())
        {
            movementState = TP_Animator.JumpState;
        }
        else if (rigidbody.velocity.y < -4f && !IsGrounded())
        {
            movementState = TP_Animator.FallState;
        }
        
        if (IsGrounded() && MoveVectorFromController.magnitude > 0 && movementState != TP_Animator.LocoState)
        {
            movementState = TP_Animator.LocoState;
        }
        TP_Animator.Instance.MoveVectorAnimator = MoveVectorFromController;
    
        if (TP_Animator.Instance.MovementState != movementState)
        {
            TP_Animator.Instance.MovementState = movementState;
        }
            
    }

   public void ApplySpeedAndDirection(Vector3 MoveVector)
   {
       float speed = 0f;
       if (MoveVector.z > 0)
       {
           speed = TP_Motor.ForwardSpeed;
       }
       else if (MoveVector.z < 0)
       {
           speed = TP_Motor.BackwardSpeed;
       }
       else
       {
           if ((MoveVector.x > 0) || (MoveVector.x < 0))
           {
               speed = TP_Motor.StrafeSpeed;
           }
       }
       TP_Motor.Instance.Speed = speed;
       TP_Motor.Instance.MoveVector = MoveVector;
   }
}
