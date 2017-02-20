using UnityEngine;
using System.Collections;
using InControl;
using TrueSync;

public class Movement : TrueSyncBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;
    public float rotationSpeed = 100f;
    float hori;
    float vert;
    float hori2;
    float m_speed;    
    float groundCheckDistance = 0.2f;
    float originalGroundCheckDistance;

    bool isGrounded;
    bool settingLandBool;
    bool horiPressed;
    bool vertPressed;
    bool isSprinting;
    bool isCrouching;
    bool isMoving;
 //   [HideInInspector]
  //  public bool isProne;

    Animator anim;
    Rigidbody rb;

    InputDevice inputDevice;
    CustomCamera customCamera;

	public override void  OnSyncedStart ()
    {
        if (TrueSyncManager.LocalPlayer == owner)
        {
            print(TrueSyncManager.LocalPlayer);
            transform.FindChild("Main Camera").gameObject.SetActive(true);
            customCamera = transform.FindChild("Main Camera").gameObject.GetComponent<CustomCamera>();
        }

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        originalGroundCheckDistance = groundCheckDistance;
        m_speed = speed;
	}

    public override void OnSyncedInput()
    {
        inputDevice = InputManager.ActiveDevice;

        float hori = inputDevice.LeftStickX;
        float hori2 = inputDevice.RightStickX;
        float vert = inputDevice.LeftStickY;
        bool sprint = inputDevice.LeftStickButton.IsPressed;
        bool crouch = inputDevice.RightStickButton.WasPressed;
        bool jump = inputDevice.Action1.WasPressed;

        TrueSyncInput.SetFP(0, hori);
        TrueSyncInput.SetFP(1, hori2);
        TrueSyncInput.SetFP(2, vert);
        TrueSyncInput.SetBool(6, sprint);
        TrueSyncInput.SetBool(7, crouch);
        TrueSyncInput.SetBool(8, jump);
    }

    public override void OnSyncedUpdate ()
    {
        FP hori = TrueSyncInput.GetFP(0);
        FP hori2 = TrueSyncInput.GetFP(1);
        FP vert = TrueSyncInput.GetFP(2);
        bool sprint = TrueSyncInput.GetBool(6);
        bool crouch = TrueSyncInput.GetBool(7);
        bool jump = TrueSyncInput.GetBool(8);

        if (hori != 0 || vert != 0)
        {
            isMoving = true;
            anim.SetBool("IsMoving", true);
        }
        else
        {
            isMoving = false;
            anim.SetBool("IsMoving", false);
        }

        CheckGroundedStatus();

        if (isGrounded)
        {
            anim.SetFloat("Horizontal", (float)hori);
            anim.SetFloat("Vertical", (float)vert);
            anim.SetFloat("Horizontal2", (float)hori2);
            
            if(sprint)
            {
                isSprinting = true;
                anim.SetBool("IsSprinting", true);
            }
            else
            {
                isSprinting = false;
                anim.SetBool("IsSprinting", false);
            }

            //if (inputDevice.RightStickButton.WasRepeated)
            //{
            //    if (!isProne)
            //    {
            //        isProne = true;
            //        anim.SetBool("IsProne", true);
            //    }
            //}
            if (crouch)
            {
                if(!isCrouching)
                {
                    isCrouching = true;
                    anim.SetBool("IsCrouching", true);
                }
                else
                {
                  //  isProne = false;
                    isCrouching = false;
                    anim.SetBool("IsProne", false);
                    anim.SetBool("IsCrouching", false);
                }
            }

            //if (isProne && isMoving)
            //{
            //    anim.SetBool("IsProneMoving", true);
            //}
            //else
            //{
            //    anim.SetBool("IsProneMoving", false);
            //}

            if (isCrouching && isMoving)
            {
                anim.SetBool("IsCrouchMoving", true);
            }
            else
            {
                anim.SetBool("IsCrouchMoving", false);
            }
        }

        if (jump && isGrounded) // && !isProne
        {
            anim.SetBool("isJumping", true);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            CheckGroundedStatus();
        }

        if (hori != 0)
            horiPressed = true;
        else
            horiPressed = false;

        if (vert != 0)
            vertPressed = true;
        else
            vertPressed = false;

        if (horiPressed && vertPressed)
            m_speed = speed / speed;
        else
            m_speed = speed;

        if (!isGrounded)
            m_speed = speed * 0.75f;
        else
            m_speed = speed;

        //if(!isProne)
        //{
            if (!isCrouching)
            {
                if (!isSprinting)
                {
                    //Walking / Running
                    transform.Translate(new Vector3((float)hori * Time.deltaTime * m_speed, 0f, (float)vert * Time.deltaTime * m_speed));
                    transform.Rotate(new Vector3(0, (float)hori2 * rotationSpeed * Time.deltaTime, 0));
                }
                else
                {
                //Sprinting
                    transform.Translate(new Vector3((float)hori * Time.deltaTime * m_speed * 1.5f, 0f, (float)vert * Time.deltaTime * m_speed * 1.5f));
                    transform.Rotate(new Vector3(0, (float)hori2 * rotationSpeed * 1.5f * Time.deltaTime, 0));
                }
            }
            else
            {
            //Crouching
                transform.Translate(new Vector3((float)hori * Time.deltaTime * m_speed * .5f, 0f, (float)vert * Time.deltaTime * m_speed * .5f));
                transform.Rotate(new Vector3(0, (float)hori2 * rotationSpeed * .5f * Time.deltaTime, 0));
            }
        //}
        //else
        //{
        //    if(customCamera.isAiming)
        //    {
        //        //You can't walk while aiming in prone.
        //        anim.SetFloat("Horizontal", 0);
        //        anim.SetFloat("Vertical", 0);
        //    }
        //    else
        //    {
        //        //Proning
        //        transform.Translate(new Vector3(hori * Time.deltaTime * m_speed * .25f, 0f, vert * Time.deltaTime * m_speed * .25f));
        //        transform.Rotate(new Vector3(0, hori2 * rotationSpeed * .25f * Time.deltaTime, 0));
        //    }
        //}
    }

    void CheckGroundedStatus()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.2f) + (Vector3.down * groundCheckDistance));
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo))
        {
            if ((hitInfo.collider.tag == "Ground" || hitInfo.collider.tag == "Environment") && hitInfo.distance <= groundCheckDistance)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
                anim.SetBool("isJumping", false);
                anim.SetFloat("Height", hitInfo.distance);
            }
        }
    }
}