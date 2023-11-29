using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionesMenuPerfiles : MonoBehaviour
{
    public UIBlock Root = null;

    public List<ProfileCollection> ProfileCollections = null;
    public ListView Perfiles = null;

    // Start is called before the first frame update
    void Start()
    {
        Perfiles.AddDataBinder<ProfileCollection, VisualPerfil>(BindPerfil);
        Perfiles.SetDataSource(ProfileCollections);
    }

    private void BindPerfil(Data.OnBind<ProfileCollection> evt, VisualPerfil target, int index)
    {
        target.NombrePerfil.Text = evt.UserData.Category;
    }

}
