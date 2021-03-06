﻿using UnityEngine;
using System.Collections;
using InControl;

public class Shooting : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletHole;

    public float shootingRange = 50f;
    public float fireFreq = .05f;
    public float magazineSize = 60f;
    public float reloadTime = 2f;

    float ammo;
    float lastShot;

    bool shooting;
    bool hitObject;
    bool reloading;

    InputDevice inputDevice;
    Animator anim;
    Movement movement;
    CustomCamera customCamera;
    RaycastHit hit;

    void Start()
    {
        anim = GameObject.Find("Player").GetComponent<Animator>();
        movement = GameObject.Find("Player").GetComponent<Movement>();
        customCamera = Camera.main.gameObject.GetComponent<CustomCamera>();
        muzzleFlash.SetActive(false);
        ammo = magazineSize;
    }
	
	void Update ()
    {
        inputDevice = InputManager.ActiveDevice;

        if(inputDevice.Action3.WasPressed)
        {
            StartCoroutine(Reload());
        }
        if(inputDevice.RightTrigger.IsPressed)
        {
            if(movement.isProne && !customCamera.isAiming)
            {
                //Can not shoot while proned and not aiming.
            }
            else
            {
                if(!shooting && Time.time > lastShot + fireFreq && ammo > 0 && !reloading)
                {
                    shooting = true;
                    lastShot = Time.time;
                    anim.SetBool("IsShooting", true);
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

    IEnumerator Shoot()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.01f);
        muzzleFlash.SetActive(false);
        if (Physics.Raycast(transform.position, transform.forward, out hit, shootingRange))
        {
            if(hit.transform.tag == "Environment")
            {
                if(!hitObject)
                {
                    hitObject = true;
                    ammo--;
                    Instantiate(bulletHole, hit.point, hit.transform.rotation);
                }
            }
        }
        hitObject = false;

        if(ammo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        reloading = true;
        anim.SetBool("IsReloading", true);
        ammo = magazineSize;
        yield return new WaitForSeconds(reloadTime);
        anim.SetBool("IsReloading", false);
        reloading = false;
    }
}
