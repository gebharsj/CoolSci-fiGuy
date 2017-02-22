using UnityEngine;
using System.Collections;
using InControl;

public class CustomCamera : MonoBehaviour
{
    public GameObject player;
    public float aimSpeed = 10;
    public float rotationSpeed = 100;

    [Tooltip("Position of the camera when not in aim mode.")]
    public Vector3 firstStartPosition;
    [Tooltip("Position of the camera when you are in aim mode.")]
    public Vector3 firstAimPosition;

    [Tooltip("Position of the camera when not in aim mode.")]
    public Vector3 thirdStartPosition;
    [Tooltip("Position of the camera when you are in aim mode.")]
    public Vector3 thirdAimPosition;

    Animator anim;

    float hori2;
    Vector3 myPosition;
    [HideInInspector]
    public bool isAiming;

    public enum CameraView
    {
        FirstPerson,
        ThirdPerson,
    }

    public CameraView cameraView = CameraView.ThirdPerson;
    InputDevice inputDevice;
    
	void Start ()
    {
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
	}
	void Update ()
    {
        inputDevice = InputManager.ActiveDevice;
        hori2 = inputDevice.RightStickX;
        
        if(inputDevice.DPadUp.WasPressed)
        {
            SwitchCameraView();
        }
        switch (cameraView)
        {
            case CameraView.ThirdPerson:
                if (inputDevice.LeftTrigger.IsPressed)
                {
                    isAiming = true;
                    anim.SetBool("IsAiming", true);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, thirdAimPosition, aimSpeed * Time.deltaTime);
                }
                else
                {
                    isAiming = false;
                    anim.SetBool("IsAiming", false);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, thirdStartPosition, aimSpeed * Time.deltaTime); ;
                }
                break;

            case CameraView.FirstPerson:
                if (inputDevice.LeftTrigger.IsPressed)
                {
                    isAiming = true;
                    anim.SetBool("IsAiming", true);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, firstAimPosition, aimSpeed * Time.deltaTime);
                }
                else
                {
                    isAiming = false;
                    anim.SetBool("IsAiming", false);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, firstStartPosition, aimSpeed * Time.deltaTime); ;
                }
                break;
        }
    }

    public void SwitchCameraView()
    {
        if(cameraView == CameraView.ThirdPerson)
            cameraView = CameraView.FirstPerson;
        else
            cameraView = CameraView.ThirdPerson;
    }
}
