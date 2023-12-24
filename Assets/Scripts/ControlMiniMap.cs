using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMiniMap : MonoBehaviour
{
    public static ControlMiniMap instance;

    public bool isMiniMapOn;

    private void Awake()
    {
        instance = this;
        
        isMiniMapOn = false;
    }
}
