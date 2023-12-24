using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MinimapDrag : MonoBehaviour
{
    public static MinimapDrag instance;
    public Camera cameraPos;

    Vector3 Origin;
    Vector3 Difference;
    Vector3 ResetCamera;


    bool isDrag;
    private void Awake()
    {
        instance = this;
        isDrag = false;
    }

    private void Start()
    {
        ResetCamera = cameraPos.transform.position;
    }

    void FixedUpdate()
    {
        if (isDrag)
        {
            Difference = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 tem = Origin - Difference;
            tem.z = -25;

            cameraPos.transform.position = tem;
        }
    }

    Vector3 temp;
    private void OnMouseDown()
    { 
        if (ControlMiniMap.instance.isMiniMapOn)
        {
            Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log(Difference);

            //Debug.Log(Vector2.Distance(Difference, cameraPos.transform.GetChild(0).position));

            isDrag = true;
        }
        
    }
    private void OnMouseUp()
    {
        isDrag = false;
    }
    // Start is called before the first frame update
}
