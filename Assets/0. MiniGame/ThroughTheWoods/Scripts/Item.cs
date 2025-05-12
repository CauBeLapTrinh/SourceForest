using DG.Tweening;
using UnityEngine;

namespace ThroughTheWoods
{
    public enum ItemEffectType
    {
        Heal,      // Hồi máu
        Mana,      // Hồi mana
        Exp
    }
    public class Item : MonoBehaviour
    {
        [Header("Item Settings")]
        public ItemEffectType effectType; // Loại hiệu ứng
        public int effectValue;           // Giá trị hiệu ứng

        // [Header("Visual Settings")]
        // public GameObject pickupEffect;  // Hiệu ứng khi nhặt item

        void OnDestroy()
        {
            DOTween.Kill(transform); // Hủy tất cả các tween liên quan đến item này khi nó bị hủy
        }
        private void Start()
        {
            // Hiệu ứng nảy lên khi item xuất hiện
            Vector3 startScale = transform.localScale;
            transform.localScale = Vector3.zero; // Bắt đầu với kích thước 0
            transform.DOScale(startScale, 0.5f).SetEase(Ease.OutBack); // Phóng to với hiệu ứng nảy

            // Tùy chọn: Di chuyển nhẹ lên/xuống
            transform.DOMoveY(transform.position.y + 0.5f, 0.3f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Kiểm tra nếu nhân vật đi vào vùng của item
            if (collision.CompareTag("Player"))
            {
                ApplyEffect(collision.gameObject); // Áp dụng hiệu ứng
                Instantiate(Controller.instance.controlPrefabs.collectPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject); // Hủy item sau khi nhặt
            }
        }

        private void ApplyEffect(GameObject player)
        {
            if (!player.TryGetComponent<SPUM_PlayerController>(out var playerController)) return;

            switch (effectType)
            {
                case ItemEffectType.Heal:
                    playerController.Healing(effectValue);
                    break;
                case ItemEffectType.Mana:
                    playerController.RestoreMana(effectValue);
                    break;
                case ItemEffectType.Exp:
                    playerController.GainExp(effectValue);
                    break;
                default:
                    Debug.LogWarning("Unknown item effect type!");
                    break;
            }
        }
    }
}