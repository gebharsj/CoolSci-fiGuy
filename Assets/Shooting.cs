using UnityEngine;
using System.Collections;
using InControl;

public class Shooting : MonoBehaviour
{
    InputDevice inputDevice;
    Animator anim;
    Movement movement;
    CustomCamera customCamera;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        customCamera = transform.FindChild("Main Camera").gameObject.GetComponent<CustomCamera>();
    }
	
	void Update ()
    {
        inputDevice = InputManager.ActiveDevice;

        if(inputDevice.RightTrigger.IsPressed)
        {
            if(movement.isProne && !customCamera.isAiming)
            {
                //Can not shoot while proned and not aiming.
            }
            else
            anim.SetBool("IsShooting", true);
        }
        else
        {
            anim.SetBool("IsShooting", false);
        }
	}
}
