using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float speed = 10f;
    public float jumpForce = 10f;
    float hori;
    float vert;
    float m_speed;    
    float groundCheckDistance = 0.2f;
    float originalGroundCheckDistance;

    bool isGrounded;
    bool settingLandBool;
    bool horiPressed;
    bool vertPressed;

    Animator anim;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        originalGroundCheckDistance = groundCheckDistance;
        m_speed = speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal", hori);
        anim.SetFloat("Vertical", vert);

        CheckGroundedStatus();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump_down"))
                    StartCoroutine(SetLandBool());
            }
            else
            {
                isGrounded = false;
                anim.SetFloat("Height", hitInfo.distance);
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump_down"))
                    StartCoroutine(SetLandBool());
            }
        }
        else
            isGrounded = false;
    }

    IEnumerator SetLandBool()
    {
        if(!settingLandBool)
        {
            settingLandBool = true;
            yield return new WaitForSeconds(0.25f);
            anim.SetBool("isJumping", false);
            settingLandBool = false;
        }
    }
}