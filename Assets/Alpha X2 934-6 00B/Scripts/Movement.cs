using UnityEngine;
using System.Collections;

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

    Animator anim;
    Rigidbody rb;

	void Start ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        originalGroundCheckDistance = groundCheckDistance;
        m_speed = speed;
	}
	
	void Update ()
    {
        hori = Input.GetAxis("Horizontal");
        hori2 = Input.GetAxis("Horizontal2");
        vert = Input.GetAxis("Vertical");

        if (hori != 0 || vert != 0)
            anim.SetBool("IsMoving", true);
        else
            anim.SetBool("IsMoving", false);

        CheckGroundedStatus();

        if (isGrounded)
        {
            anim.SetFloat("Horizontal", hori);
            anim.SetFloat("Vertical", vert);
            anim.SetFloat("Horizontal2", hori2);
        }

        if (Input.GetButtonDown("Fire1") && isGrounded)
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

        transform.Translate(new Vector3(hori * Time.deltaTime * m_speed, 0f, vert * Time.deltaTime * m_speed));
        transform.Rotate(new Vector3(0, hori2 * rotationSpeed * Time.deltaTime, 0));   
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