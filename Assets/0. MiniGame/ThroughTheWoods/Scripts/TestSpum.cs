using UnityEngine;

public class TestSpum : MonoBehaviour
{
    public PlayerObj playerObj;
    public PlayerState playerState;
    public int indexAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 posMove = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerObj.SetMovePos(posMove);
        }
    }
}
