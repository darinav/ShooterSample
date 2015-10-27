using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour 
{
    public static TP_Animator Instance;

    public static int IdleState = Animator.StringToHash("Base Layer.Idle");
    public static int LocoState = Animator.StringToHash("Base Layer.Locomotion");
    public static int JumpState = Animator.StringToHash("Base Layer.Jump");
    public static int FallState = Animator.StringToHash("Base Layer.RunLand");
    public static int CrouchState = Animator.StringToHash("Base Layer.Crouch");

    public static int AimState = Animator.StringToHash("Aiming.Aiming");
    public static int ReloadState = Animator.StringToHash("Aiming.Reloading");
    public static int NewState = Animator.StringToHash("Aiming.New State");

    private bool reloadAllowed;

    private int _State;
    public int MovementState
    {
        get
        {
            return _State;
        }
        set
        {
            _State = value;                
            //if(_State == IdleState)
            //{
            //    Debug.Log("Idle");
            //}
            //else if (_State == LocoState)
            //{
            //    Debug.Log("Loco");
            //}
            //else if (_State == JumpState)
            //{
            //    Debug.Log("Jump");
            //}
            //else if (_State == CrouchState)
            //{
            //    Debug.Log("Crouch");
            //}
            //else if (_State == AimState)
            //{
            //    Debug.Log("Aim");
            //}
            //else if (_State == ReloadState)
            //{
            //    Debug.Log("ReloadQQQ");
            //}
            //else if (_State == FallState)
            //{
            //    Debug.Log("Fall");
            //}
            //else
            //{
            //    Debug.Log("Unknown");
            //}

        }
    }

    public int WeaponState;
    public Vector3 MoveVectorAnimator;    

    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    private AnimatorStateInfo currentWeaponState;

	void Awake() 
    {
        Instance = this;
	}

    void Start()
    {
        anim = GetComponent<Animator>();
    }
	
	void Update() 
    {
        ProcessMovementState(MovementState);
        ProcessWeaponState(WeaponState);

        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

        CheckReloadState(anim.GetNextAnimatorStateInfo(1));
        currentWeaponState = anim.GetCurrentAnimatorStateInfo(1);
	}

    void ProcessMovementState(int state)
    {
        float v = MoveVectorAnimator.z;
        float h = MoveVectorAnimator.x;
        anim.SetFloat("Speed", v);
        anim.SetFloat("Direction", h);

        if (state == LocoState)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }

        if (state == JumpState)
        {
            anim.SetBool("Jump", true);
            anim.SetFloat("VerticalVelocity", GetComponent<Rigidbody>().velocity.y);
        }

        if (state == FallState || state == LocoState || state == IdleState)
        {
            anim.SetBool("Jump", false);
        }

        if (state == CrouchState)
        {
            anim.SetBool("Crouch", true);
        }
        else
        {
            anim.SetBool("Crouch", false);
        }

        ScopeControl.Instance.state = MovementState;
    }

    void ProcessWeaponState(int state)
    {   
        if (state == AimState)
        {
            anim.SetBool("Aim", true);
        }
        else
        {
            anim.SetBool("Aim", false);
        }

        if (state == ReloadState)
        {
            anim.SetBool("Reload", true);
            reloadAllowed = true;
            anim.SetBool("Aim", false);
        }        
        else
        {
            anim.SetBool("Reload", false);
        }
    }

    public void CheckReloadState(AnimatorStateInfo info)
    {
        if (reloadAllowed && currentWeaponState.IsName("Reloading") && info.IsName("New State"))
        {
            TP_Controller.Instance.CompleteWeaponReload();
            reloadAllowed = false;
        }
    }


}
