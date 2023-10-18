using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform SpawnPosition;
    [SerializeField] Base_Weapon WeaponPrefab;
    [SerializeField] Camera SceneCamera;
    [SerializeField] MousePosition2D MousePosition;

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
    }

    void Update()
    {
        if (IsWeaponSelected) WeaponFaceToMouse();
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
        //CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition.position, Quaternion.identity);
        CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition);
        IsWeaponSelected = true;
    }

    void WeaponFaceToMouse()
    {
        Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition) - CurrentWeapon.transform.position;
        float Angle = MathF.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion Rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        CurrentWeapon.transform.rotation = Rotation;
    }

    public void WeaponFaceToPointer(Transform Pointer)
    {
        CurrentWeapon.transform.right = Pointer.transform.position - CurrentWeapon.transform.position;
    }

    public void LaunchWeapon()
    {
        if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost)
        {
            IsWeaponSelected = false;
            MousePosition.SetSelectedTile();
            CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
            GameManager.Instance.CurrentRecyclePoints -= CurrentWeapon.WeaponDataSO.BaseUseCost;
        }

        //Rigidbody2D WeaponRB = CurrentWeapon.GetComponent<Rigidbody2D>();
        //Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        //WeaponRB.velocity = new Vector2(WeaponRB.velocity.x + LaunchSpeed, WeaponRB.velocity.y + LaunchSpeed) * WeaponRB.transform.right;
    }
}
