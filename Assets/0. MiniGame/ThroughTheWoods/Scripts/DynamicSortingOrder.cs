using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Lấy component SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Điều chỉnh Order in Layer dựa trên vị trí Y
        // Nhân vật ở dưới (Y nhỏ hơn) sẽ có Order in Layer cao hơn (hiển thị phía trước)
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
    }
}
