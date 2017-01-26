using UnityEngine;
using System.Collections;
using InControl;

public class Shooting : MonoBehaviour
{
    InputDevice inputDevice;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
	
	void Update ()
    {
        inputDevice = InputManager.ActiveDevice;

        if(inputDevice.RightTrigger.IsPressed)
        {
            anim.SetBool("IsShooting", true);
        }
        else
        {
            anim.SetBool("IsShooting", false);
        }
	}
}
