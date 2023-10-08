using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform SpawnPosition;
    [SerializeField] GameObject WeaponPrefab;
    [SerializeField] Camera SceneCamera;
    [SerializeField] MousePosition2D MousePosition;

    [Header("Behaviour Varaibles")]
    [SerializeField] float LaunchSpeed = 1f;

    //Internal Variables
    GameObject CurrentWeapon;
    bool IsWeaponSelected;

    [Header("Events")]
    public UnityEvent OnWeaponSelected;
    public UnityEvent OnWeaponUsed;

    // Start is called before the first frame update
    void Start()
    {
        //CurrentWeapon = Instantiate(WeaponPrefab, SpawnPosition);
        IsWeaponSelected = false;
    }

    // Update is called once per frame
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
        IsWeaponSelected = false;
        MousePosition.SetSelectedTile();
        Rigidbody2D WeaponRB = CurrentWeapon.GetComponent<Rigidbody2D>();
        //CurrentWeapon.GetComponent<Base_Weapon>().SetHitPoint(MousePosition.GetMousePointerPosition());
        //Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        WeaponRB.velocity = new Vector2(WeaponRB.velocity.x + LaunchSpeed, WeaponRB.velocity.y + LaunchSpeed) * WeaponRB.transform.right;
    }
}
