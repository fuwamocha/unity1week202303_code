using System;
using TMPro;
using UniRx;
using UnityEngine;
namespace Unity1week202303.Score
{
    public class TimeManager : MonoBehaviour
    {
        private static readonly Subject<Unit> _onClear = new();
        [SerializeField] private TextMeshProUGUI timeText;

        private float _elapsedTime;
        private bool _isCounting;
        internal static float ClearTime { get; private set; }
        internal static IObservable<Unit> OnClear => _onClear;
        private void Update()
        {
            if (!_isCounting) return;
            _elapsedTime += Time.deltaTime;

            timeText.text = _elapsedTime.ToString("f1") + "秒";
        }

        public void StartCount()
        {
            _elapsedTime = 0;
            _isCounting = true;
        }

        public void StopCount()
        {
            _isCounting = false;
            ClearTime = _elapsedTime;
            _onClear.OnNext(Unit.Default);
        }
    }
}
