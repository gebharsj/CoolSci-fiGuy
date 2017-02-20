using UnityEngine;
using System.Collections;
using InControl;
using TrueSync;

public class Shooting : TrueSyncBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletHole;

    public float shootingRange = 50f;
    public float fireFreq = .05f;
    public float magazineSize = 60f;
    public float reloadTime = 2f;

    GameObject player;
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

    public override void  OnSyncedStart()
    {
        foreach (GameObject _player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(TrueSyncManager.LocalPlayer == owner)
            {
                player = _player;
                anim = player.GetComponent<Animator>();
                movement = player.GetComponent<Movement>();
                customCamera = Camera.main.gameObject.GetComponent<CustomCamera>();
            }
        }
        muzzleFlash.SetActive(false);
        ammo = magazineSize;
    }

    public override void OnSyncedInput()
    {
        inputDevice = InputManager.ActiveDevice;

        bool reload = inputDevice.Action2.WasPressed;
        bool shot = inputDevice.RightTrigger.IsPressed;

        TrueSyncInput.SetBool(4, reload);
        TrueSyncInput.SetBool(5, shot);
    }
    public override void OnSyncedUpdate ()
    {
        bool reload = TrueSyncInput.GetBool(4);
        bool shot = TrueSyncInput.GetBool(5);

        if(reload)
        {
            StartCoroutine(Reload());
        }
        if(shot)
        {
            //if(movement.isProne && !customCamera.isAiming)
            //{
            //    //Can not shoot while proned and not aiming.
            //}
            //else
            //{
                if(!shooting && Time.time > lastShot + fireFreq && ammo > 0 && !reloading)
                {
                    shooting = true;
                    lastShot = Time.time;
                    anim.SetBool("IsShooting", true);
                    StartCoroutine(Shoot());
                    shooting = false;
                }
          //  }
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
        if (Physics.Raycast(transform.position, transform.right, out hit, shootingRange))
        {
            print("Happens");
            if(hit.transform.tag == "Player")
            {
                print("HIT ENEMY");
                if (!hitObject)
                {
                    hitObject = true;
                    ammo--;
                    GameObject clone = TrueSyncManager.Instantiate(bulletHole, hit.point, hit.transform.rotation) as GameObject;
                    clone.transform.SetParent(hit.transform);
                    hit.transform.GetComponent<Health>().TookDamage();
                }
            }
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
