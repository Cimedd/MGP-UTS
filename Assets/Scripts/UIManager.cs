using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text Ammo;
    public GameObject panelOver;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateAmmo(int ammo)
    {
        Ammo.text = $"Ammo : {ammo}";
    }

    public void upadteHealth()
    {

    }
}
