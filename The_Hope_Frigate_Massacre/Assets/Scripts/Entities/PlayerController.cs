
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private float maniability;
    [SerializeField] private float rotationSpeed, walkMultiplier, runMultiplier;
    public LayerMask whatIsGround;
   
    [Header("Camera Values")]
    [SerializeField] private float MouseSensitivity;
    [SerializeField] private float CamLerpRate;
    [SerializeField] float CamXAngle;
    [SerializeField] float CamYAngle;
    [SerializeField] private float ResetDelay;
    [SerializeField] Vector3 CamXDir;
    [SerializeField] Vector3 CamYDir;

    [Header("Player References")]
    [SerializeField] private DiversuitRagdoll playerRagdoll;
    [SerializeField] private Transform pelvis;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform main;

    float XInput, ZInput, MouseX, MouseY;
    private bool running;
    Vector3 forward, right, Dir;

    public DiversuitRagdoll PlayerRagdoll { get { return playerRagdoll; } }

    private void Start()
    {
        playerRagdoll.Hit += () => Hit();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        InitializeMoveDir();

        Inputs();

        NoPhysicsRotation();
    }

    private void FixedUpdate()
    {
        MovementManagement();
    }

    public void Hit()
    {
    }

    private void MovementManagement()
    {
        var speed = rb.velocity.magnitude;
        rb.velocity = Vector3.Lerp(rb.velocity.normalized, Dir, maniability) * speed;
    }

    private void Inputs()
    {
        ZInput = Input.GetAxis("Vertical");
        XInput = Input.GetAxis("Horizontal");

        MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.unscaledDeltaTime;
        MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.unscaledDeltaTime;

        float multiplier = walkMultiplier;

        if (Input.GetMouseButton(0))
        {
            if (running)
                running = false;

            Dir = Vector3.Normalize(forward);

            animator.SetBool("Walk", Mathf.Abs(ZInput)> 0.1f);
            animator.SetFloat("WalkMultiplier", ZInput * multiplier);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && ! running)
            {
                running = true;
            }

            if (running)
            {
                multiplier = runMultiplier;
            }

            Dir = Vector3.Normalize(forward * ZInput + right * XInput);

            animator.SetBool("Walk", Dir.magnitude > 0.1f);
            animator.SetFloat("WalkMultiplier", 1 * multiplier);
        }

        animator.SetBool("Aim", Input.GetMouseButton(0));

        if (Dir.magnitude < 0.3f)
        {
            running = false;
        }
    }

    private void NoPhysicsRotation()
    {
        if (Dir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(Dir.z, Dir.x) * Mathf.Rad2Deg;

            Quaternion targetQuaternion = Quaternion.Euler(pelvis.transform.rotation.eulerAngles.x, -targetAngle + 90f, pelvis.transform.rotation.eulerAngles.z);

            pelvis.transform.rotation = Quaternion.RotateTowards(pelvis.transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);
        }
    }

    private void InitializeMoveDir()
    {
        forward = main.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }
}