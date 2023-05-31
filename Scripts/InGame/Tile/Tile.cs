using System;
using Unity1week202303.InGame.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202303.InGame.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Image image;

        private TileType _tileType;

        internal TileType TileType
        {
            get => _tileType;
            set
            {
                _tileType = value;
                SetColor(_tileType);
            }
        }

        internal bool TryBreak(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Item1 => Break(TileType.Breakable1),
                ItemType.Item2 => Break(TileType.Breakable2),
                ItemType.Item3 => Break(TileType.Breakable3),
                _              => throw new ArgumentOutOfRangeException()
            };
        }

        private bool Break(TileType tileType)
        {
            if (TileType != tileType) return false;
            TileType = TileType.None;
            return true;
        }

        private void SetColor(TileType tile)
        {
            switch (tile)
            {
                case TileType.None:
                case TileType.Goal:
                case TileType.Visited:
                    image.color = Color.clear;
                    break;
                case TileType.Unbreakable:
                    image.color = new Color(0, 0, 0, 0.5f);
                    break;
                case TileType.Breakable1:
                    image.color = Color.blue;
                    break;
                case TileType.Breakable2:
                    image.color = Color.red;
                    break;
                case TileType.Breakable3:
                    image.color = Color.yellow;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(tile), tile, null);
            }
        }
    }
}