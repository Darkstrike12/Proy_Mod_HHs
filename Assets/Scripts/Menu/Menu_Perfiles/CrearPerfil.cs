using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CrearPerfil : MonoBehaviour
{
    [SerializeField] public TMP_InputField txtUsario;
    public TMP_Text nombre;
    public string nombreUsuario;
    public void InstantiateCaller(GameObject prefab)
    {
        GameObject Content = GameObject.Find("Content");
        GameObject Input = GameObject.Find("InputField");
        string s;
        Instantiate(prefab, Content.transform);

    }
    public void Nombre(GameObject prefab)
    {
        nombreUsuario = txtUsario.text;
        nombre.text = nombreUsuario;

    }
}
