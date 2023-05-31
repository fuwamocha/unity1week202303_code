using System;
using MochaLib.Audio;
using Unity1week202303.InGame.Tile;
using UnityEngine;
using UnityEngine.UI;
namespace Unity1week202303.InGame.Item
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Canvas _effectCanvas;
        [SerializeField] private TrashBox.TrashBox trashBox;
        [SerializeField] private GameObject particleObject;
        [SerializeField] private AudioClip se;
        [SerializeField] private Image itemImage;
        public Action<bool> IsConsumedItem;
        private bool _canBreak;
        private ItemMove _itemMove;
        private ItemType _itemType;
        private Tile.Tile _tile;
        public ItemType ItemType
        {
            get => _itemType;
            set
            {
                _itemType = value;
                SetColor(_itemType);
            }
        }

        private void Awake()
        {
            _itemMove = GetComponent<ItemMove>();
        }

        private void OnMouseUp()
        {
            if (trashBox.CanTrash)
            {
                trashBox.Trash();
                Consume();
                return;
            }

            if (!_canBreak) return;
            var tilePosition = _tile.transform.position;
            if (!_tile.TryBreak(ItemType)) return;
            Instantiate(particleObject, tilePosition, Quaternion.identity, _effectCanvas.transform);
            Consume();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out _tile)) return;
            if (_tile.TileType is not (TileType.Breakable1 or TileType.Breakable2 or TileType.Breakable3)) return;

            if ((ItemType != ItemType.Item1 || _tile.TileType != TileType.Breakable1) &&
                (ItemType != ItemType.Item2 || _tile.TileType != TileType.Breakable2) &&
                (ItemType != ItemType.Item3 || _tile.TileType != TileType.Breakable3))
            {
                return;
            }
            _canBreak = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_canBreak || !other.TryGetComponent(out Tile.Tile tile)) return;
            if (_tile != tile) return;
            _canBreak = false;
        }

        private void SetColor(ItemType type)
        {
            itemImage.color = type switch
            {
                ItemType.Item1 => Color.blue,
                ItemType.Item2 => Color.red,
                ItemType.Item3 => Color.yellow,
                _              => itemImage.color
            };
        }

        private void Consume()
        {
            AudioPlayer.Se.Play(se, false);
            IsConsumedItem?.Invoke(true);
            _itemMove.ResetPosition();
        }
    }
}
