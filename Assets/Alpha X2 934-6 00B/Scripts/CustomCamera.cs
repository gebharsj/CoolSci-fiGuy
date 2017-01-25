using UnityEngine;
using System.Collections;
using InControl;

public class CustomCamera : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed = 100;
    public float rotationSpeed = 100;
    [Tooltip("Position of the camera when not in aim mode.")]
    public Vector3 startPosition;
    [Tooltip("Position of the camera when you are in aim mode.")]
    public Vector3 aimPosition;

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
                    myPosition = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + aimPosition.x, player.transform.position.y + aimPosition.y, player.transform.position.z + aimPosition.z), Time.deltaTime * 10);
                    transform.RotateAround(myPosition, new Vector3(0, transform.position.y, 0), hori2 * Time.deltaTime);
                }
                else
                {
                    myPosition = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + startPosition.y, player.transform.position.z + startPosition.z), Time.deltaTime * 10);
                    transform.RotateAround(myPosition, new Vector3(0, transform.position.y, 0), hori2 * Time.deltaTime);
                }
                break;

            case CameraView.FirstPerson:
                break;
        }
    }
}
