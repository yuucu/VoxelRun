using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    None,
    Up,
    Left,
    Right,
}

public class Player : MonoBehaviour
{

    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_JumpPower = 12f;
    [Range(1f, 10f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;
    [SerializeField]
    float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;
    [SerializeField]
    int m_MaxJumpCount = 2;
    [SerializeField]
    float moveDistance;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    bool m_Crouching;

    bool isAliving;
    int currentLane;
    Vector3 targetPos;
    Direction targetDirection;

    [SerializeField]
    private GameObject magnet;
    [SerializeField]
    private GameObject coinUpper;

    int haveCoin = 0;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    private int magnetTime = 10;
    private int coinUpTime = 10;
    private int specialCoin = 30;
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    private AudioSource[] audios;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        audios = GetComponents<AudioSource>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_OrigGroundCheckDistance = m_GroundCheckDistance;

        currentLane = 2;
        targetDirection = Direction.None;
        isAliving = false;

        GameManager.Instance.setPlayer(this);

    }


    public void gameStart()
    {
        audios[6].Play();
        isAliving = true;
    }


    void Update()
    {
        if (isAliving)
        {
            itemTime += Time.deltaTime;

            //Left
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                targetDirection = Direction.Left;


            //Right
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                targetDirection = Direction.Right;


            if (isAliving)
                Move(Vector3.forward);
            else
                Move(Vector3.zero);

            targetDirection = Direction.None;

        }
    }



    // 投げる
    void Hit()
    {
        Vector3 pos = transform.position;
        pos.z += 0.5f;
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/bomb"), pos, Quaternion.identity);
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * 600);
    }



    int upgradeCount = 10;
    private void FixedUpdate()
    {
        if (Mathf.FloorToInt(getPos().z) > upgradeCount)
        {
            updateGameMode();
            upgradeCount += 20 + upgradeCount / 10;
        }
    }


    [SerializeField]
    private TargetFollow lightWrapper;

    public void updateGameMode()
    {
        m_MoveSpeedMultiplier += 0.06f;
        EnemyManager.Instance.upgradeEnemy();
        StageManager.Instance.upgradeStage();
        lightWrapper.upgrade();
    }


    float time = 0;


    public void Move(Vector3 move)
    {

        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();



        // 横移動
        if (targetDirection == Direction.Left)
            if (currentLane > 1)
                currentLane--;

        if (targetDirection == Direction.Right)
            if (currentLane < 3)
                currentLane++;

        // control and velocity handling is different when grounded and airborne:

        if (m_IsGrounded)
        {
            HandleGroundedMovement();
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move);

        time += Time.deltaTime;
        if (time >= 0.3f)
        {
            audios[0].Play();
            time = 0;
        }


    }



    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);

        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }



    void HandleGroundedMovement()
    {

        Hashtable hash = new Hashtable();
        hash.Add("time", 1f);

        switch (currentLane)
        {
            case 1:
                transform.position = new Vector3(-moveDistance, transform.position.y, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(0, transform.position.y, transform.position.z);

                break;
            case 3:
                transform.position = new Vector3(moveDistance, transform.position.y, transform.position.z);
                break;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isAliving)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (collision.gameObject.GetComponent<Enemy>().getIsAliving())
                {
                    audios[5].Play();
                    Debug.Log(transform.name + " collision with " + collision.transform.name);
                    if (m_IsGrounded)
                        m_Rigidbody.AddForce(new Vector3(0, 260, -200));
                    playerDeath();
                }
                else
                    Destroy(collision.gameObject);
            }

            if (collision.gameObject.tag == "Bomb")
            {
                audios[3].Play();
                Debug.Log(transform.name + " collision with " + collision.transform.name);
                m_Rigidbody.AddForce(new Vector3(0, 260, -200));
                playerDeath();
            }
        }
    }

    float itemTime = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (isAliving)
        {
            if (other.tag == "Spike")
            {
                audios[5].Play();
                if (m_IsGrounded)
                    m_Rigidbody.AddForce(new Vector3(0, 120, 0));
                playerDeath();
            }

            if (other.tag == "Water")
            {
                audios[5].Play();
                m_Rigidbody.velocity = Vector3.zero;
                Vector3 newPos = other.transform.position;
                newPos.y += 0.1f;
                transform.position = newPos;
                playerDeath();
            }
            if (other.tag == "Coin")
            {
                audios[1].Play();
                haveCoin++;
            }

            if (itemTime > 1f)
                if (other.tag == "SpecialCoin")
                {
                    itemTime = 0f;
                    audios[2].Play();
                    haveCoin += specialCoin;
                }

            if (itemTime > 1f)
                if (other.tag == "Magnet")
                {
                    itemTime = 0;
                    audios[4].Play();
                    CancelInvoke("magnetOff");
                    Vector3 pos = transform.position;
                    pos.y = 1.2f;
                    magnet.transform.position = pos;
                    magnet.SetActive(true);
                    Invoke("magnetOff", magnetTime + upgradeCount / 50);
                }

            if (itemTime > 1f)
                if (other.tag == "CoinUp")
                {
                    itemTime = 0f;
                    audios[4].Play();
                    CancelInvoke("coinOff");
                    Vector3 pos = transform.position;
                    pos.y = 1.2f;
                    magnet.transform.position = pos;
                    coinUpper.SetActive(true);
                    Invoke("coinOff", coinUpTime + upgradeCount / 50);
                }
        }
    }

    void magnetOff()
    {
        magnet.SetActive(false);
    }

    void coinOff()
    {
        coinUpper.SetActive(false);
    }

    [SerializeField]
    private GameResult gameResult;

    void playerDeath()
    {
        audios[6].Stop();
        audios[7].Play();
        m_Animator.SetTrigger("Death");
        isAliving = false;
        Invoke("createTensi", 1f);
        GameManager.Instance.gameStop();
        gameResult.show(getPos().z, haveCoin);
    }

    void createTensi()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        Instantiate(Resources.Load<GameObject>("Prefabs/tensi"), pos, Quaternion.identity);
    }



    public Vector3 getPos()
    {
        return transform.position;
    }

    public int getCoin()
    {
        return haveCoin;
    }

    public bool getIsAliving()
    {
        return isAliving;
    }
}
