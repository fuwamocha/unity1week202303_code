using System;
using InGame;
using MochaLib.Cores;
using UniRx;
using Unity1week202303.InGame.Tile;
using UnityEngine;
using UnityEngine.Serialization;
namespace Unity1week202303.Player
{
    public class Player : MonoBehaviour
    {
        private const int HalfSquare = 105;

        [FormerlySerializedAs("_diceInteger")] [SerializeField]
        private MochaIntRange mochaDiceInteger;

        private readonly ReactiveProperty<TileType> _nextTile = new();
        private readonly Vector2 _resetColliderPosition = Vector2.left * 90 + Vector2.up * 90;
        private BoxCollider2D _collider2D;
        private Vector2Int _currentIndex = Vector2Int.zero;
        internal Action OnClear;

        private void Start()
        {
            _collider2D = GetComponent<BoxCollider2D>();

            var wStream = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.W));
            var aStream = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.A));
            var sStream = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.S));
            var dStream = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.D));

            wStream.Subscribe(_ =>
            {
                if (_currentIndex.y + 1 >= StageCreate.StageRow[StageController.Stage]) return;
                MoveToNextTile(Vector2.up);
            }).AddTo(this);

            aStream.Subscribe(_ =>
            {
                if (_currentIndex.x <= 0) return;
                MoveToNextTile(Vector2.left);
            }).AddTo(this);

            sStream.Subscribe(_ =>
            {
                if (_currentIndex.y <= 0) return;
                MoveToNextTile(Vector2.down);
            }).AddTo(this);

            dStream.Subscribe(_ =>
            {
                if (_currentIndex.x + 1 >= StageCreate.StageRow[StageController.Stage]) return;
                MoveToNextTile(Vector2.right);
            }).AddTo(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Tile tile)) return;
            _nextTile.SetValueAndForceNotify(tile.TileType);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Goal")) OnClear?.Invoke();
        }

        private async void MoveToNextTile(Vector2 direction)
        {
            _collider2D.offset = direction * HalfSquare * 2;
            await _nextTile;
            _collider2D.offset = _resetColliderPosition;
            if (_nextTile.Value is not (TileType.None or TileType.Visited or TileType.Goal)) return;
            _currentIndex += Vector2Int.RoundToInt(direction);
            transform.localPosition += (Vector3)direction * HalfSquare;
        }
    }
}
