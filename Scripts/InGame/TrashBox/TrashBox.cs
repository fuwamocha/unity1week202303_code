using UnityEngine;
namespace Unity1week202303.InGame.TrashBox
{
    public class TrashBox : MonoBehaviour
    {
        internal bool CanTrash { get; private set; }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Item.Item item)) CanTrash = true;
        }

        internal void Trash()
        {
            if (!CanTrash) return;
            CanTrash = false;
        }
    }
}
