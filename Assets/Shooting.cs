using UnityEngine;
using System.Collections;
using InControl;

public class Shooting : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletHole;

    public float shootingRange = 50f;

    InputDevice inputDevice;
    Animator anim;
    Movement movement;
    CustomCamera customCamera;
    bool shooting;
    RaycastHit hit;

    void Start()
    {
        anim = GameObject.Find("Player").GetComponent<Animator>();
        movement = GameObject.Find("Player").GetComponent<Movement>();
        customCamera = Camera.main.gameObject.GetComponent<CustomCamera>();
        muzzleFlash.SetActive(false);
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
            {
                if(!shooting)
                {
                    shooting = true;
                    anim.SetBool("IsShooting", true);
                    StartCoroutine(MuzzleFlash());
                    StartCoroutine(Shoot());
                    shooting = false;
                }
            }
        }
        else
        {
            anim.SetBool("IsShooting", false);
        }
	}

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator Shoot()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, shootingRange))
        {
            if(hit.transform.tag == "Environment")
            {
                Instantiate(bulletHole, hit.point, hit.transform.rotation);
            }
        }
        yield return new WaitForSeconds(.5f);
    }
    
}
