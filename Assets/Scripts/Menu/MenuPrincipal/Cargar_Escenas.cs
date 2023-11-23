using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cargar_Escenas : MonoBehaviour
{

    public string NombreEscena;
    public void SceneLoader()
    {
        SceneManager.LoadScene(NombreEscena);
    }
}
