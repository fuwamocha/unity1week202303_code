using System.Collections;
using DG.Tweening;
using UnityEngine;
namespace Unity1week202303.InGame.Item
{
    public class ItemMove : MonoBehaviour
    {
        private readonly Vector3 _originalScale = Vector3.one * 0.4f;
        private Vector3 _itemPosition;
        private Vector3 _offset;
        internal bool IsMouseDown { get; private set; }

        private void Start()
        {
            _itemPosition = transform.position;
        }

        private void OnMouseDown()
        {
            IsMouseDown = true;
            transform.DOScale(_originalScale * 1.4f, 0.1f);
            if (Camera.main != null) _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            if (Camera.main == null) return;
            var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
            transform.position = newPosition;
        }

        private void OnMouseUp()
        {
            IsMouseDown = false;
            transform.DOScale(0, 0.1f);
            StartCoroutine(ResetPosition());
        }

        internal IEnumerator ResetPosition()
        {
            yield return new WaitForSeconds(0.1f);
            transform.DOScale(_originalScale, 0.1f);
            transform.position = _itemPosition;
        }
    }
}
