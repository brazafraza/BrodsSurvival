using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WindowHandler windowHandler;

    private CharacterController cc;
    private CameraLook cam;
    [Space]
    [Space]
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float jumpForce = 5.5f;
    [Space]
    [SerializeField] private float crouchTransitionSpeed = 5f;



    [SerializeField] private float gravity = -7f;

    private float gravityAcceleration;
    private float yVelocity;

    [HideInInspector] public bool crouching;
    [HideInInspector] public bool walking;
    [HideInInspector] public bool running;

    [Header("Footsteps")]
    private AudioSource audioS;


    private float currentCrouchLength;
    private float currentWalkLenth;
    private float currentRunLength;

    public float runStepLength;
    public float walkStepLentgh;
    public float crouchStepLength;

    void Start()
    {
        windowHandler = GetComponent<WindowHandler>();
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<CameraLook>();
        audioS = GetComponent<AudioSource>();

        gravityAcceleration = gravity * gravity;
        gravityAcceleration *= Time.deltaTime;
    }

    private void Update()
    {



        if (crouching)
        {
            if (currentCrouchLength < crouchStepLength)
            {
                currentCrouchLength += Time.deltaTime;
            }
            else
            {
                currentCrouchLength = 0;

                audioS.PlayOneShot(GetFootstep());
            }
        }

        else if (walking)
        {
            if (currentWalkLenth < walkStepLentgh)
            {
                currentWalkLenth += Time.deltaTime;
            }
            else
            {
                currentWalkLenth = 0;

                audioS.PlayOneShot(GetFootstep());
            }
        }

        else if (running) //crouching
        {
            if (currentWalkLenth < runStepLength)
            {
                currentWalkLenth += Time.deltaTime;
            }
            else
            {
                currentWalkLenth = 0;

                audioS.PlayOneShot(GetFootstep());
            }
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            moveDir.z += 1;
        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            moveDir.z -= 1;
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            moveDir.x += 1;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            moveDir.x -= 1;



        //running
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            moveDir *= runSpeed;

            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, 2, 0), crouchTransitionSpeed * Time.deltaTime);
            cc.height = Mathf.Lerp(cc.height, 2, crouchTransitionSpeed * Time.deltaTime);
            cc.center = Vector3.Lerp(cc.center, new Vector3(0, 1, 0), crouchTransitionSpeed * Time.deltaTime);

            walking = false;
            crouching = false;
            running = true;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift)) //crouching
        {

            moveDir *= crouchSpeed;

            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, 1, 0), crouchTransitionSpeed * Time.deltaTime);
            cc.height = Mathf.Lerp(cc.height, 1.2f, crouchTransitionSpeed * Time.deltaTime);
            cc.center = Vector3.Lerp(cc.center, new Vector3(0, 0.59f, 0), crouchTransitionSpeed * Time.deltaTime);

            walking = false;
            crouching = true;
            running = false;
        }
        else //walking
        {
            moveDir *= walkSpeed;

            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, 2, 0), crouchTransitionSpeed * Time.deltaTime);
            cc.height = Mathf.Lerp(cc.height, 2, crouchTransitionSpeed * Time.deltaTime);
            cc.center = Vector3.Lerp(cc.center, new Vector3(0, 1, 0), crouchTransitionSpeed * Time.deltaTime);

            walking = true;
            crouching = false;
            running = false;
        }

        if (moveDir == Vector3.zero)
        {
            walking = false;
            crouching = false;
            running = false;
        }


        if (cc.isGrounded)
        {
            yVelocity = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                yVelocity = jumpForce;
            }
        }
        else
            yVelocity -= gravityAcceleration;

        moveDir.y = yVelocity;

        moveDir = transform.TransformDirection(moveDir);
        moveDir *= Time.deltaTime;

        cc.Move(moveDir);

    }

    public AudioClip GetFootstep()
    {
        RaycastHit hit;

        //fix this bug later

        if (Physics.SphereCast(cc.center, 0.2f, Vector3.down, out hit, cc.bounds.extents.y + 0.3f))
        {


            Surface surface = hit.transform.GetComponent<Surface>();

            if (surface != null)
            {
                int i = Random.Range(0, surface.surface.footsteps.Length);

                return surface.surface.footsteps[i];
            }
            else
                return null;
        }
                
        else
        {
            Debug.LogWarning("SphereCast didn't hit collider.");
            return null;
        }
          
    } 



   
}
