using UnityEngine;
using System.Collections;
using InControl;

public class Movement : MonoBehaviour
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
    [HideInInspector]
    public bool isProne;

    Animator anim;
    Rigidbody rb;

    InputDevice inputDevice;
    CustomCamera customCamera;

	void Start ()
    {
        customCamera = transform.FindChild("Main Camera").gameObject.GetComponent<CustomCamera>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        originalGroundCheckDistance = groundCheckDistance;
        m_speed = speed;
	}
	
	void Update ()
    {
        inputDevice = InputManager.ActiveDevice;

        hori = inputDevice.LeftStickX;
        hori2 = inputDevice.RightStickX;
        vert = inputDevice.LeftStickY;

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
            anim.SetFloat("Horizontal", hori);
            anim.SetFloat("Vertical", vert);
            anim.SetFloat("Horizontal2", hori2);
            
            if(inputDevice.LeftStickButton.IsPressed)
            {
                isSprinting = true;
                anim.SetBool("IsSprinting", true);
            }
            else
            {
                isSprinting = false;
                anim.SetBool("IsSprinting", false);
            }

            if (inputDevice.RightStickButton.WasRepeated)
            {
                if (!isProne)
                {
                    isProne = true;
                    anim.SetBool("IsProne", true);
                }
            }
            if (inputDevice.RightStickButton.WasPressed)
            {
                if(!isCrouching)
                {
                    isCrouching = true;
                    anim.SetBool("IsCrouching", true);
                }
                else
                {
                    isProne = false;
                    isCrouching = false;
                    anim.SetBool("IsProne", false);
                    anim.SetBool("IsCrouching", false);
                }
            }

            if (isProne && isMoving)
            {
                anim.SetBool("IsProneMoving", true);
            }
            else
            {
                anim.SetBool("IsProneMoving", false);
            }

            if (isCrouching && isMoving)
            {
                anim.SetBool("IsCrouchMoving", true);
            }
            else
            {
                anim.SetBool("IsCrouchMoving", false);
            }
        }

        if (inputDevice.Action1.WasPressed && isGrounded && !isProne)
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

        if(!isProne)
        {
            if (!isCrouching)
            {
                if (!isSprinting)
                {
                    //Walking / Running
                    transform.Translate(new Vector3(hori * Time.deltaTime * m_speed, 0f, vert * Time.deltaTime * m_speed));
                    transform.Rotate(new Vector3(0, hori2 * rotationSpeed * Time.deltaTime, 0));
                }
                else
                {
                    //Sprinting
                    transform.Translate(new Vector3(hori * Time.deltaTime * m_speed * 1.5f, 0f, vert * Time.deltaTime * m_speed * 1.5f));
                    transform.Rotate(new Vector3(0, hori2 * rotationSpeed * 1.5f * Time.deltaTime, 0));
                }
            }
            else
            {
                //Crouching
                transform.Translate(new Vector3(hori * Time.deltaTime * m_speed * .5f, 0f, vert * Time.deltaTime * m_speed * .5f));
                transform.Rotate(new Vector3(0, hori2 * rotationSpeed * .5f * Time.deltaTime, 0));
            }
        }
        else
        {
            if(customCamera.isAiming)
            {
                //You can't walk while aiming in prone.
                anim.SetFloat("Horizontal", 0);
                anim.SetFloat("Vertical", 0);
            }
            else
            {
                //Proning
                transform.Translate(new Vector3(hori * Time.deltaTime * m_speed * .25f, 0f, vert * Time.deltaTime * m_speed * .25f));
                transform.Rotate(new Vector3(0, hori2 * rotationSpeed * .25f * Time.deltaTime, 0));
            }
        }
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