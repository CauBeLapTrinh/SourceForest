using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Forest
{

public class PlayerControl : MonoBehaviour
{
    PlayerAttack playerAttack;
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    ItemCollect itemCollect;

    public Transform weaponRotate;

    

    // Start is called before the first frame update
    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        itemCollect = GetComponent<ItemCollect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}
