using UnityEngine;
using System.Collections;
using InControl;

public class CustomCamera : MonoBehaviour
{
    public GameObject player;
    public float aimSpeed = 10;
    public float rotationSpeed = 100;
    [Tooltip("Position of the camera when not in aim mode.")]
    public Vector3 startPosition;
    [Tooltip("Position of the camera when you are in aim mode.")]
    public Vector3 aimPosition;
    Animator anim;

    float hori2;
    Vector3 myPosition;

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
        
        switch (cameraView)
        {
            case CameraView.ThirdPerson:
                if (inputDevice.LeftTrigger.IsPressed)
                {
                    anim.SetBool("IsAiming", true);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
                }
                else
                {
                    anim.SetBool("IsAiming", false);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, aimSpeed * Time.deltaTime); ;
                }
                break;

            case CameraView.FirstPerson:
                break;
        }
    }
}
