using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public bool canSprint;

    public Image staminaBar;

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

    Transform myCamera;

    Controls controls;
    string saveData;

    Quaternion cameraTargetRot;
    Quaternion myRotation;

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    void Start()
    {
        Cursor.visible = false;
        myCamera = transform.FindChild("Camera");
        cameraTargetRot = myCamera.localRotation;
        myRotation = transform.localRotation;
        currentSpeed = speed;
    }

    void Update()
    {
        horizontal = controls.Move.X;
        horizontal2 = controls.Look.X;
        vertical = controls.Move.Y;
        vertical2 = controls.Look.Y;

        if(canSprint && controls.Sprint.IsPressed)
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
        
        cameraTargetRot *= Quaternion.Euler(-vertical2 * rotationSpeed * Time.deltaTime, 0f, 0f);
        myRotation *= Quaternion.Euler(0f, horizontal2 * rotationSpeed * Time.deltaTime, 0f);

        if (clampVerticalRotation)
           cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        myCamera.localRotation = Quaternion.Slerp(myCamera.localRotation, cameraTargetRot, 1);

        transform.Translate(new Vector3(horizontal * Time.deltaTime * currentSpeed, 0f, vertical * Time.deltaTime * currentSpeed));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, myRotation, 1);
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

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    { 
        q.x /= q.w; 
        q.y /= q.w; 
        q.z /= q.w; 
        q.w = 1.0f; 
 
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x); 
        angleX = Mathf.Clamp(angleX, minVerticalClamp, maxVerticalClamp); 
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad* angleX); 
        return q;
    } 


}
