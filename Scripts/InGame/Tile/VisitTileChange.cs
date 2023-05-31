using UnityEngine;

namespace Unity1week202303.InGame.Tile
{
    public class VisitTileChange : MonoBehaviour
    {
        [SerializeField] private Tile tile;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player.Player player)) tile.TileType = TileType.Visited;
        }
    }
}