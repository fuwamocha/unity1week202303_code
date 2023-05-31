using UnityEngine;

namespace Unity1week202303.InGame.Item
{
    public class NextItemLoader : MonoBehaviour
    {
        [SerializeField] private Item currentItem;
        [SerializeField] private Item nextItem;

        private void Awake()
        {
            SetItemType(currentItem);
            SetItemType(nextItem);
        }

        private void OnEnable()
        {
            currentItem.IsConsumedItem += UpdateIndex;
        }

        private void OnDisable()
        {
            currentItem.IsConsumedItem -= UpdateIndex;
        }

        private static void SetItemType(Item item)
        {
            item.ItemType = Random.Range(0, 3) switch
            {
                0 => ItemType.Item1,
                1 => ItemType.Item2,
                2 => ItemType.Item3,
                _ => item.ItemType
            };
        }

        private void UpdateIndex(bool isConsumed)
        {
            if (!isConsumed) return;
            currentItem.ItemType = nextItem.ItemType;
            SetItemType(nextItem);
        }
    }
}