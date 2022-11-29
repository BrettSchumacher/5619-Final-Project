using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextShower : MonoBehaviour
{
    public Teleport tp1;
    public Teleport tp2;
    public TMP_Text text;

    bool tele1 = false;
    bool tele2 = false;
    
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        tp1.onTeleport += () => { 
            tele1 = true;
            if (tele2) EnableText();
        };
        tp2.onTeleport += () => { 
            tele2 = true;
            if (tele1) EnableText();
        };
    }

    void EnableText()
    {
        text.enabled = true;
    }
}
