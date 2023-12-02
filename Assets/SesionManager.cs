using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SesionManager : MonoBehaviour
{
    public static SesionManager Instance;

    public string CurrentSesion;

    public float MasterVolume;
    public bool MusicMute;
    public bool SFXMute;

    private void Awake()
    {
        //MasterVolume = 1.0f;
        //MusicMute = true;
        //SFXMute = true;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Destroy(Instance.gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
