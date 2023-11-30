using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    public string NombreEscena;
    public void SceneLoader()
    {
        SceneManager.LoadScene(NombreEscena);
    }
}
