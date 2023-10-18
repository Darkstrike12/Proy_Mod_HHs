using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T2 : T1
{
    protected override void Start()
    {
        Debug.Log($"Protected Int {ProtectedInt} and public Int {PublicInt} and Private", gameObject);
    }
}
