using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void EscenaJuego(string nombreNivel) {
        SceneManager.LoadScene(nombreNivel);
    }
}
