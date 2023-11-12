using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnCave : MonoBehaviour
{
    public GameObject headCave;
    public GameObject DoorCave;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            headCave.SetActive(false);
            DoorCave.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            headCave.SetActive(true);
            DoorCave.SetActive(false);
        }
    }
}
