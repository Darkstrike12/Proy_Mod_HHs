using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform SpawnPosition;
    [SerializeField] GameObject WeaponPrefab;
    [SerializeField] Camera SceneCamera;
    [SerializeField] float LaunchSpeed = 1f;
    [SerializeField] GameObject MouseIndicator;

    //Internal Variables
    GameObject CurrentWeapon;
    WeaponState weaponState;

    // Start is called before the first frame update
    void Start()
    {
        weaponState = WeaponState.Choosing;
        CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //switch (weaponState)
        //{
        //    case WeaponState.Choosing:
        //        CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition);
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            weaponState = WeaponState.Standby;
        //            print("Weapon Chosen");
        //        }
        //        break;

        //    case WeaponState.Standby:
        //        FaceToMouse();
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            LaunchWeapon();
        //            weaponState = WeaponState.Shoot;
        //        }
        //        break;

        //    case WeaponState.Shoot:
        //        weaponState = WeaponState.Choosing;
        //        break;
        //}
        FaceToMouse();
        if (Input.GetMouseButtonDown(0))
        {
            LaunchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition);
        }
    }

    enum WeaponState
    {
        Choosing,
        Standby,
        Shoot,
    }

    private void FaceToMouse()
    {
        Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition) - CurrentWeapon.transform.position;
        float Angle = MathF.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion Rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        CurrentWeapon.transform.rotation = Rotation;
    }

    private void FaceToMousePointer()
    {
        CurrentWeapon.transform.right = MouseIndicator.transform.position - CurrentWeapon.transform.position;
    }

    private void LaunchWeapon()
    {
        Rigidbody2D WeaponRB = CurrentWeapon.GetComponent<Rigidbody2D>();
        Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        WeaponRB.velocity = new Vector2(WeaponRB.velocity.x + LaunchSpeed, WeaponRB.velocity.y + LaunchSpeed) * WeaponRB.transform.right;
    }
}
