using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;
using TrueSync;

public class PlayerMovement : TrueSyncBehaviour
{
    public float speed;
    public float rotationSpeed;
    public bool canSprint;
    public Image staminaBar;

    public float jumpForce;

    public float minVerticalClamp = -65;
    public float maxVerticalClamp = 65;

    public bool clampVerticalRotation = true;

    float horizontal;
    float horizontal2;
    float vertical;
    float vertical2;

    float currentSpeed;
    bool sprinting;
    bool exhausted;

    TSTransform myCamera;

    Controls controls;
    string saveData;

    TSQuaternion cameraTargetRot;
    TSQuaternion myRotation;

    TSRigidBody rb;
    bool isGrounded;

    float groundCheckDistance = 0.2f;
    float originalGroundCheckDistance;

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    public override void  OnSyncedStart()
    {
        Cursor.visible = false;
        myCamera = transform.FindChild("Camera").GetComponent<TSTransform>();
        cameraTargetRot = myCamera.rotation;
        myRotation = tsTransform.rotation;
        currentSpeed = speed;
        rb = GetComponent<TSRigidBody>();
        originalGroundCheckDistance = groundCheckDistance;
    }

    public override void OnSyncedInput()
    {
        FP horizontal = controls.Move.X;
        FP horizontal2 = controls.Look.X;
        FP vertical = controls.Move.Y;
        FP vertical2 = controls.Look.Y;
        bool jumping = controls.Jump;

        TrueSyncInput.SetFP(0, horizontal);
        TrueSyncInput.SetFP(1, vertical);
        TrueSyncInput.SetFP(2, horizontal2);
        TrueSyncInput.SetFP(3, vertical2);
        TrueSyncInput.SetBool(4, jumping);
    }
    public override void OnSyncedUpdate()
    {
        FP horizontal = TrueSyncInput.GetFP(0);
        FP horizontal2 = TrueSyncInput.GetFP(2);
        FP vertical = TrueSyncInput.GetFP(1);
        FP vertical2 = TrueSyncInput.GetFP(3);
        bool jumping = TrueSyncInput.GetBool(4);

        CheckGroundedStatus();

        if (jumping && isGrounded)
        {
            rb.velocity = new TSVector(rb.velocity.x, jumpForce, rb.velocity.z);
            CheckGroundedStatus();
        }

        if (canSprint && controls.Sprint.IsPressed)
        {
            if(!sprinting && !exhausted)
            {
                sprinting = true;
                currentSpeed = speed * 2;
            }

            if(!exhausted)
            staminaBar.fillAmount -= (Time.deltaTime * .5f);

            if(staminaBar.fillAmount <= 0)
            {
                exhausted = true;
                currentSpeed = speed;
            }
        }
        else
        {
            if(exhausted)
            {
                if(staminaBar.fillAmount >= 1)
                    exhausted = false;
            }

            if(canSprint)
            {
                sprinting = false;
                currentSpeed = speed;

                staminaBar.fillAmount += Time.deltaTime;
            }
        }
        
        cameraTargetRot *= TSQuaternion.Euler(-vertical2 * rotationSpeed * Time.deltaTime, 0f, 0f);
        myRotation *= TSQuaternion.Euler(0f, horizontal2 * rotationSpeed * Time.deltaTime, 0f);

        if (clampVerticalRotation)
           cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot.ToQuaternion());

        myCamera.rotation = TSQuaternion.Slerp(myCamera.rotation, cameraTargetRot, 1);

        tsTransform.Translate(new TSVector(horizontal * TrueSyncManager.DeltaTime * currentSpeed, 0f, vertical * TrueSyncManager.DeltaTime * currentSpeed));
        tsTransform.rotation = TSQuaternion.Slerp(tsTransform.rotation, myRotation, 1);
        // transform.Rotate(new Vector3(0, horizontal2 * rotationSpeed * Time.deltaTime, 0));
    }

    void SaveBindings()
    {
        saveData = controls.Save();
        PlayerPrefs.SetString("Bindings", saveData);
    }


    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveData = PlayerPrefs.GetString("Bindings");
            controls.Load(saveData);
        }
    }

    TSQuaternion ClampRotationAroundXAxis(Quaternion q)
    { 
        q.x /= q.w; 
        q.y /= q.w; 
        q.z /= q.w; 
        q.w = 1.0f; 
 
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x); 
        angleX = Mathf.Clamp(angleX, minVerticalClamp, maxVerticalClamp); 
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad* angleX); 
        return q.ToTSQuaternion();
    }
    void CheckGroundedStatus()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(new Vector3(transform.position.x, transform.localPosition.y * .1f, transform.position.z), Vector3.down);
        Debug.DrawLine(ray.origin, new Vector3(transform.position.x, 0, transform.position.z), Color.red);
        if (Physics.Raycast(ray.origin, Vector3.down, out hitInfo, .1f))
        {
            if ((hitInfo.collider.tag == "Ground" || hitInfo.collider.tag == "Environment") && hitInfo.distance <= groundCheckDistance)
            {
                isGrounded = true;
                print("IsGround = " + isGrounded);
            }
        }
        else
        {
            isGrounded = false;
            print("IsGround = " + isGrounded);
        }
    }
}
