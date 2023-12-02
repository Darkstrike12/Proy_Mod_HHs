using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    public string NombreEscena;

    public void SceneLoader()
    {
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.StopBGM();
        //    AudioManager.Instance.StopAmbienceSound();
        //}
        SceneManager.LoadScene(NombreEscena);
    }

    //public void LoadLevel(Scene Level)
    //{
    //    SceneManager.LoadScene(Level.name);
    //}
}
