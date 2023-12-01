using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject weaponAreaDisplay;
    [SerializeField] Camera sceneCamera;
    [SerializeField] MousePosition2D mousePosition;

    [Header("Weapons")]
    [SerializeField] List<Base_Weapon> aviableWeapons;
    [SerializeField] List<Base_Weapon> weaponPool;

    [Header("Behaviour Varaibles")]
    [SerializeField] float LaunchSpeed = 1f;

    //Internal Variables
    Base_Weapon CurrentWeapon;
    bool IsWeaponSelected;

    [Header("Events")]
    public UnityEvent OnWeaponSelected;
    public UnityEvent OnWeaponUsed;

    #region UnityFunctions

    void Start()
    {
        //CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition);
        IsWeaponSelected = false;
        weaponAreaDisplay = Instantiate(weaponAreaDisplay, transform);
        weaponAreaDisplay.SetActive(false);

        foreach (var weapon in aviableWeapons)
        {
            Base_Weapon wp = Instantiate(weapon, spawnPosition);
            wp.gameObject.SetActive(false);
            weaponPool.Add(wp);
        }
    }

    void Update()
    {
        if (IsWeaponSelected)
        {
            WeaponFaceToMouse();
            DisplayWeaponAreaIndicator();
        }
        if (Input.GetMouseButtonDown(0) && IsWeaponSelected)
        {
            OnWeaponUsed.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnWeaponSelected.Invoke();
        }
    }

    #endregion

    public void SelectWeapon(int weaponIndex)
    {
        CurrentWeapon = null;
        CurrentWeapon = weaponPool[weaponIndex];
        if(CurrentWeapon.wpState == Base_Weapon.State.Standby)
        {
            CurrentWeapon.transform.position = spawnPosition.transform.position;
            CurrentWeapon.gameObject.SetActive(true);
            IsWeaponSelected = true;
            weaponAreaDisplay.SetActive(true);
        } 
        else
        {
            CurrentWeapon = null;
        }
    }

    void WeaponFaceToMouse()
    {
        Vector2 Direction = sceneCamera.ScreenToWorldPoint(Input.mousePosition) - CurrentWeapon.transform.position;
        float Angle = MathF.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion Rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        CurrentWeapon.transform.rotation = Rotation;
    }

    void DisplayWeaponAreaIndicator()
    {
        CurrentWeapon.DisplayWeaponAffectedArea(weaponAreaDisplay);
        weaponAreaDisplay.transform.position = mousePosition.GetMousePointerPosition();
    }

    public void WeaponFaceToPointer(Transform Pointer)
    {
        CurrentWeapon.transform.right = Pointer.transform.position - CurrentWeapon.transform.position;
    }

    public void LaunchWeapon()
    {
        if(CurrentWeapon != null && CurrentWeapon.wpState == Base_Weapon.State.Standby)
        {
            if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost && mousePosition.IsMousePointerOverGameGrid())
            {
                weaponAreaDisplay.SetActive(false);
                IsWeaponSelected = false;
                mousePosition.SetSelectedTile();
                CurrentWeapon.wpState = Base_Weapon.State.Active;
                CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
                GameManager.Instance.UpdateCurrentRecyclePoints(-CurrentWeapon.WeaponDataSO.BaseUseCost);
            }
        }
        //if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost)
        //{
        //    if (mousePosition.IsMousePointerOverGameGrid())
        //    {
        //        IsWeaponSelected = false;
        //        mousePosition.SetSelectedTile();
        //        CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
        //        GameManager.Instance.UpdateCurrentRecyclePoints(-CurrentWeapon.WeaponDataSO.BaseUseCost);
        //    }
        //}

        //Rigidbody2D WeaponRB = CurrentWeapon.GetComponent<Rigidbody2D>();
        //Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        //WeaponRB.velocity = new Vector2(WeaponRB.velocity.x + LaunchSpeed, WeaponRB.velocity.y + LaunchSpeed) * WeaponRB.transform.right;
    }
}
