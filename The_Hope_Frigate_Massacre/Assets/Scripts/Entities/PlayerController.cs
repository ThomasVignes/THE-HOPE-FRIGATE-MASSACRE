
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Whumpus;

[System.Serializable]
public class OverrideAction
{
    public string Name;
    public float Duration;
}

public class PlayerController : MonoBehaviour
{
    [Header("Cheats")]
    public bool instaCut;

    [Header("Player Values")]
    public bool Praying;
    [SerializeField] private float maniability;
    [SerializeField] private float additionnalSpeed;
    [SerializeField] private float rotationSpeed, walkMultiplier, runMultiplier;
    [SerializeField] private float aimAssist, shotKnockback, limbEjection, shootCooldown;
    public LayerMask whatIsGround, whatAreEnemies;
    [SerializeField] private List<OverrideAction> overrideActions = new List<OverrideAction>();
   
    [Header("Camera Values")]
    [SerializeField] private float MouseSensitivity;
    [SerializeField] float CamXAngle;
    [SerializeField] float CamYAngle;
    [SerializeField] private float ResetDelay;
    [SerializeField] Vector3 CamXDir;
    [SerializeField] Vector3 CamYDir;

    [Header("Player References")]
    [SerializeField] private DiversuitRagdoll playerRagdoll;
    [SerializeField] private RagdollLimb rArmJoint;
    [SerializeField] private Transform pelvis;
    [SerializeField] private Rigidbody rb, lhand, rhand;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform camForward, camRight;
    [SerializeField] private GameObject aimCursor;

    private GunfireController gunfire;

    float XInput, ZInput, MouseX, MouseY;
    private float shootTimer, overrideTimer;
    private bool running, overrideAction, inAction, rArmCut;
    Vector3 forward, right, Dir;

    public DiversuitRagdoll PlayerRagdoll { get { return playerRagdoll; } }

    private void Start()
    {
        playerRagdoll.Hit += () => Hit();
        Cursor.lockState = CursorLockMode.Locked;

        gunfire = GetComponentInChildren<GunfireController>();
    }

    void Update()
    {
        InitializeMoveDir();

        Inputs();

        if (!Praying)
        {
            NoPhysicsRotation();
        }

        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;
        else if (shootTimer < 0)
            shootTimer = 0;
    }

    private void FixedUpdate()
    {
        if (!Praying)
        {
            MovementManagement();
        }
    }

    private void LateUpdate()
    {
        if (!Praying)
        {
            CamMovement();
        }
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

        if (overrideAction)
        {
            Dir = Vector3.Normalize(forward);

            overrideTimer -= Time.deltaTime;

            if (overrideTimer <= 0)
            {
                overrideTimer = 0;
                overrideAction = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F11))
                Death();

            if (Input.GetKeyDown(KeyCode.F))
            {
                RarmAction();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Praying = !Praying;
                
                animator.SetBool("Pray", Praying);
            }

            if (Input.GetMouseButton(1))
            {
                if (running)
                    running = false;

                Dir = Vector3.Normalize(forward);

                animator.SetBool("Walk", Mathf.Abs(ZInput)> 0.1f);
                animator.SetFloat("WalkMultiplier", ZInput * multiplier);

                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
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
                rb.AddForce(additionnalSpeed * Dir);

                animator.SetBool("Walk", Dir.magnitude > 0.1f);
                animator.SetFloat("WalkMultiplier", 1 * multiplier);
            }
        }

        animator.SetBool("Block", Input.GetKey(KeyCode.Space));
        animator.SetBool("Aim", Input.GetMouseButton(1));

        if (aimCursor.activeSelf != Input.GetMouseButton(1))
            aimCursor.SetActive(Input.GetMouseButton(1));

        if (Dir.magnitude < 0.3f)
        {
            running = false;
        }
    }

    private void Death()
    {
        var rb = pelvis.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddRelativeForce(Vector3.forward * 5000f);
        animator.SetTrigger("Death");
        playerRagdoll.ChangeWeight(0.02f);
        playerRagdoll.EnableForces(false);
    }

    private void RarmAction()
    {
        if (!inAction)
        {
            if (!rArmCut)
            {
                inAction = true;
                animator.SetTrigger("ThrowRArm");
            }
            else
            {
                rArmCut = false;
                rArmJoint.Reattatch();
            }
            animator.SetBool("RarmDetached", rArmCut);
        }
    }



    public void DetachArm()
    {
        inAction = false;

        rArmCut = true;
        rArmJoint.CutLimb();
        var rb = rArmJoint.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 500);
        rb.AddForce(forward.normalized * 2000);
        animator.SetBool("RarmDetached", rArmCut);
    }

    private void Shoot()
    {
        if (shootTimer == 0)
        {
            //Shoot
            Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
            float rayLength = 500f;

            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
            RaycastHit hit;
            if (Physics.SphereCast(ray, aimAssist, out hit, rayLength, whatAreEnemies))
            {
                TargetLimb target = hit.collider.gameObject.GetComponent<TargetLimb>();

                if (target != null)
                {
                    target.Hit(shotKnockback, ray.direction.normalized);
                }
                    
                if (instaCut)
                {
                    RagdollLimb limb = hit.collider.gameObject.GetComponent<RagdollLimb>();

                    if (limb != null)
                    {
                        limb.CutLimb();
                    }
                }
            }

            //Effects
            lhand.AddForce(Vector3.up * 400);
            rhand.AddForce(Vector3.up * 400);

            gunfire.FireWeapon();
            AmbianceManager.Instance.StopMusic(3f);

            shootTimer = shootCooldown;
        }
    }

    private void CamMovement()
    {
        CamYAngle -= MouseY;
        CamYAngle = Mathf.Clamp(CamYAngle, -55f, 30f);
        CamXAngle += MouseX;

        camRight.localRotation = Quaternion.Euler(CamYAngle, 0f, 0f);
        camForward.transform.localRotation = Quaternion.Euler(0f, CamXAngle, 0f);
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

    public void PlayOverrideAction(string name)
    {
        foreach (var action in overrideActions)
        {
            if (action.Name == name)
            {
                overrideTimer = action.Duration;
                animator.SetTrigger(action.Name);
                overrideAction = true;
            }
        }
    }

    private void InitializeMoveDir()
    {
        forward = camForward.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }
}
