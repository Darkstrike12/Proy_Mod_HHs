using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1 : MonoBehaviour
{
    public int PublicInt;
    [SerializeField] protected int ProtectedInt;
    [SerializeField] private int suma;

    protected virtual void Start()
    {
        Debug.Log($"Public Int {PublicInt} and protected {ProtectedInt}", gameObject);

    }

    protected virtual void Update()
    {
        suma = ProtectedInt + PublicInt;
    }
}
