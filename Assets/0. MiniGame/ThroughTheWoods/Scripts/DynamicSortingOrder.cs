using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        // Điều chỉnh Order in Layer dựa trên vị trí Y
        // Nhân vật ở dưới (Y nhỏ hơn) sẽ có Order in Layer cao hơn (hiển thị phía trước)
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f);
    }
}
