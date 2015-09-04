using UnityEngine;
using System.Collections;

public class ScopeControl : MonoBehaviour
{
    float smallScopeSize = 0.5f;
    float normalScopeSize = 1f;
    float bigScopeSize = 1.2f;
    
    public static ScopeControl Instance;
    public int state;

    void Start()
    {
        Instance = this;
        transform.position = new Vector2(Camera.main.pixelWidth * 0.55f, Camera.main.pixelHeight * 0.55f);        
    }
    void LateUpdate ()
    {
        resizeScope(state);
    }

    public void resizeScope(int state)
    {
        if (!Input.GetMouseButton(1))
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            //if (state == TP_Animator.IdleState)
            //{
            //    transform.localScale = Vector3.one * normalScopeSize;
            //}
            //if (state == TP_Animator.LocoState ||
            //    state == TP_Animator.JumpState)
            //{
            //    transform.localScale = Vector3.one * bigScopeSize;
            //}
            //if (state == TP_Animator.CrouchState)
            //{
            //    transform.localScale = Vector3.one * smallScopeSize;
            //}
            if (Input.GetMouseButton(1))
            {
                if (state == TP_Animator.LocoState ||
                state == TP_Animator.JumpState)
                {
                    transform.localScale = Vector3.one * bigScopeSize;
                }
                else if (state == TP_Animator.CrouchState)
                {
                    transform.localScale = Vector3.one * smallScopeSize;
                }
                else
                {
                    transform.localScale = Vector3.one * normalScopeSize;
                }
            }
        }        
    }
}
