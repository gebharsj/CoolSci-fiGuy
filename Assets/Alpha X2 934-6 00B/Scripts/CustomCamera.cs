using UnityEngine;
using System.Collections;
using InControl;
using TrueSync;

public class CustomCamera : TrueSyncBehaviour
{
    GameObject player;
    public float aimSpeed = 10;
    public float rotationSpeed = 100;

    //[Tooltip("Position of the camera when not in aim mode.")]
    //public Vector3 firstStartPosition;
    //[Tooltip("Position of the camera when you are in aim mode.")]
    //public Vector3 firstAimPosition;

    [Tooltip("Position of the camera when not in aim mode.")]
    public Vector3 thirdStartPosition;
    [Tooltip("Position of the camera when you are in aim mode.")]
    public Vector3 thirdAimPosition;

    Animator anim;

    float hori2;
    Vector3 myPosition;
    [HideInInspector]
    public bool isAiming;

    //public enum CameraView
    //{
    //    FirstPerson,
    //    ThirdPerson,
    //}

    //public CameraView cameraView = CameraView.ThirdPerson;
    InputDevice inputDevice;
    
	public override void  OnSyncedStart ()
    {
        player = transform.parent.gameObject;
        anim = player.GetComponent<Animator>();
	}

    public override void OnSyncedInput()
    {
        inputDevice = InputManager.ActiveDevice;

        bool aim = inputDevice.LeftTrigger.IsPressed;
        TrueSyncInput.SetBool(3, aim);
    }
    public override void  OnSyncedUpdate ()
    {
        FP hori2 = TrueSyncInput.GetFP(1);
        bool aim = TrueSyncInput.GetBool(3);
        //if(inputDevice.DPadUp.WasPressed)
        //{
        //    SwitchCameraView();
        //}
        //switch (cameraView)
        //{
        //    case CameraView.ThirdPerson:
        if (aim)
                {
                    isAiming = true;
                    anim.SetBool("IsAiming", true);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, thirdAimPosition, aimSpeed * Time.deltaTime);
                }
                else
                {
                    isAiming = false;
                    anim.SetFloat("Vertical2", 0);
                    anim.SetBool("IsAiming", false);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, thirdStartPosition, aimSpeed * Time.deltaTime); ;
                }

       
        //    transform.Rotate(new Vector3(-vert2 * (rotationSpeed * 2) * Time.deltaTime, 0, 0));

        anim.SetFloat("Vertical2", ((float)hori2));
        //break;

        //case CameraView.FirstPerson:
        //    if (inputDevice.LeftTrigger.IsPressed)
        //    {
        //        isAiming = true;
        //        anim.SetBool("IsAiming", true);
        //        transform.localPosition = Vector3.Lerp(transform.localPosition, firstAimPosition, aimSpeed * Time.deltaTime);
        //    }
        //    else
        //    {
        //        isAiming = false;
        //        anim.SetBool("IsAiming", false);
        //        transform.localPosition = Vector3.Lerp(transform.localPosition, firstStartPosition, aimSpeed * Time.deltaTime); ;
        //    }
        //    break;
    }
    }

    //public void SwitchCameraView()
    //{
    //    if(cameraView == CameraView.ThirdPerson)
    //        cameraView = CameraView.FirstPerson;
    //    else
    //        cameraView = CameraView.ThirdPerson;
    //}
//}
