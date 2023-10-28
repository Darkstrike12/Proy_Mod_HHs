using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] Base_Weapon weaponPrefab;
    [SerializeField] GameObject weaponAreaDisplay;
    [SerializeField] Camera sceneCamera;
    [SerializeField] MousePosition2D mousePosition;

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

    public void SelectWeapon()
    {
        Destroy(CurrentWeapon);
        CurrentWeapon = Instantiate(weaponPrefab, spawnPosition);
        IsWeaponSelected = true;
        weaponAreaDisplay.SetActive(true);
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
        weaponAreaDisplay.SetActive(false);
        if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost)
        {
            if (mousePosition.IsMousePointerOverGameGrid())
            {
                IsWeaponSelected = false;
                mousePosition.SetSelectedTile();
                CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
                GameManager.Instance.UpdateCurrentRecyclePoints(-CurrentWeapon.WeaponDataSO.BaseUseCost);
            }
        }

        //Rigidbody2D WeaponRB = CurrentWeapon.GetComponent<Rigidbody2D>();
        //Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        //WeaponRB.velocity = new Vector2(WeaponRB.velocity.x + LaunchSpeed, WeaponRB.velocity.y + LaunchSpeed) * WeaponRB.transform.right;
    }
}
