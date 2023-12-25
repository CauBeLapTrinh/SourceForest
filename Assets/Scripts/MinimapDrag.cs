using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEditor.Progress;

public class MinimapDrag : MonoBehaviour
{
    public static MinimapDrag instance;
    public Camera cameraPos;

    public float maxX;
    public float minX;
    public float maxY;
    public float minY;


    Vector3 Origin;
    Vector3 Difference;
    Vector3 lastDifference = Vector3.zero;
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

            if (tem.x > 0 && tem.y > 0)
            {
                tem.x = 1f;
                tem.y = 1f;
            }
            else if (tem.x <= 0 && tem.y <= 0)
            {
                tem.x = -1f;
                tem.y = -1f;
            }
            if (tem.x > 0)
            {
                tem.x = 1f;
            }
            else if (tem.x <= 0)
            {
                tem.x = -1f;
            }
            if (tem.y > 0)
            {
                tem.y = 1f;
            }
            else if (tem.y <= 0)
            {
                tem.y = -1f;
            }

            Vector3 pos = cameraPos.transform.position + tem;

            pos.z = -25;

            if (pos.x >= maxX && pos.y >= maxY)
            {
                pos.x = maxX;
                pos.y = maxY;
            }
            else if (pos.x <= minX && pos.y <= minY)
            {
                pos.x = minX;
                pos.y = minY;
            }
            if (pos.x >= maxX && pos.y <= minY)
            {
                pos.x = maxX;
                pos.y = minY;
            }
            else if (pos.x <= minX && pos.y >= maxY)
            {
                pos.x = minX;
                pos.y = maxY;
            }
            if (pos.x >= maxX)
            {
                pos.x = maxX;
            }
            else if (pos.x <= minX)
            {
                pos.x = minX;
            }
            else if(pos.y >= maxY)
            {
                pos.y = maxY;
            }
            else if (pos.y <= minY)
            {
                pos.y = minY;
            }

            if(Difference != lastDifference)
            {
                cameraPos.transform.position = pos;
            }

            lastDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    Vector3 temp;
    private void OnMouseDown()
    { 
        if (ControlMiniMap.instance.isMiniMapOn)
        {
            isDrag = true;

            Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
    }
    private void OnMouseUp()
    {
        isDrag = false;
    }
    // Start is called before the first frame update
}
