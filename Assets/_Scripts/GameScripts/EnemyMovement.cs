using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float EnemyVisionLength = 50f;
    public float EnemyMoveSpeed = 2f;
    public float FieldOFViewAngle = 120f;
    public int Health = 2;
    public static EnemyMovement Instance;

    private float deltaMoveOffset = 5f;
    private float playerCollisionMoveOffset = 1.25f;
    private Animator anim;
    private Vector3 enemyEyePosition;
    private Vector3 lastMoveDirection;
    private Vector3 lastPlayerPosition;
    private GameObject player;
    private CapsuleCollider enemyCollider;
    private TP_Health playerHealth;

    private Vector3 lookDirection;
    private Vector3 moveDirection;
    private RaycastHit hitInfo;
    private int layerMask = 1;

    void Awake()
    {
        player = GameObject.Find("TP_Character");
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<CapsuleCollider>();
        playerHealth = player.GetComponent<TP_Health>();
        Instance = this;
    }

    void Update()
    {
        if (playerHealth.CurrentPlayerHealth <= 0)
        {
            anim.SetBool("Move", false);
            anim.SetBool("PlayerDead", true);
            enabled = false;
        }

        enemyEyePosition = enemyCollider.bounds.center;
        lookDirection = player.transform.position - enemyEyePosition;
        lookDirection.y = 0;
        moveDirection = Vector3.zero;
        float angle = Vector3.Angle(lookDirection, transform.forward);

        if (Physics.Raycast(enemyEyePosition, lookDirection, out hitInfo, EnemyVisionLength, layerMask))
        {
            if (hitInfo.distance >= collider.bounds.extents.z * 1.5f)
            {
                if (hitInfo.collider.gameObject == player && angle <= FieldOFViewAngle * 0.5f)
                {
                    moveDirection = lookDirection;
                    moveDirection.y = 0;
                    moveDirection = moveDirection.normalized;
                    lastMoveDirection = moveDirection;
                    lastPlayerPosition = player.transform.position;
                    transform.Translate(moveDirection * EnemyMoveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    if (Vector3.Distance(lastPlayerPosition, transform.position) > deltaMoveOffset)
                    {
                        transform.Translate(lastMoveDirection * EnemyMoveSpeed * Time.deltaTime, Space.World);
                    }
                    else
                    {
                        anim.SetBool("Move", false);
                    }
                }
            }
        }

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            anim.SetBool("Move", true);
        }
    }
}