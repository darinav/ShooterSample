using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Aiming : MonoBehaviour {

    public GameObject Bullet;
    public Transform FiringPoint;
    public float BulletSpeed = 250f;
    public float RotationSpeed = 15f;
    public int TotalAmmoCount = 30;
    public int CurrentAmmoCount = 30;
    public Slider AmmoSlider;

    public static Aiming Instance;

    private Vector3 shootToRayHit;
    private float delay = 1f;

    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        AmmoSlider.value = CurrentAmmoCount;
        Ray camRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * 0.55f, Camera.main.pixelHeight * 0.55f));
        RaycastHit camRayHit;

        if (Physics.Raycast(camRay, out camRayHit, 150f))
        {
            shootToRayHit = camRayHit.point;
        }
        else
        {
            shootToRayHit = camRay.origin + camRay.direction * 150f;
        }
        
        TP_Controller.Instance.UpdateWeaponState(CurrentAmmoCount);

        delay -= 10f * Time.deltaTime;
        if (Input.GetMouseButton(0) && 
            delay <= 0 && 
            TP_Animator.Instance.WeaponState == TP_Animator.AimState &&
            CurrentAmmoCount > 0)
        {
            fireBullets();
            delay = 1f;
        }
    }

    void fireBullets()
    {
        //Vector3 randomTrajectoryOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        GameObject shotGO = (GameObject)Instantiate(Bullet, FiringPoint.position, FiringPoint.rotation);
        Vector3 deltaPos = shootToRayHit - shotGO.transform.position;
        shotGO.GetComponent<Rigidbody>().velocity = deltaPos.normalized * BulletSpeed;
        CurrentAmmoCount--;
    }

    public void ReloadWeapon()
    {
        CurrentAmmoCount = TotalAmmoCount;
    }
}
