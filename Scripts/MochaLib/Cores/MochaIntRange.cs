using System;
using UnityEngine;

namespace MochaLib.Cores
{
    [Serializable]
    public struct MochaIntRange : IRange<int>
    {
        [SerializeField] private int minValue;
        [SerializeField] private int maxValue;

        public MochaIntRange(int min, int max)
        {
            minValue = min;
            maxValue = Mathf.Max(min, max);
        }

        public int Min
        {
            get => minValue;
            set => minValue = Mathf.Min(value, maxValue);
        }

        public int Max
        {
            get => maxValue;
            set => maxValue = Mathf.Max(value, minValue);
        }

        public int Mid => minValue + (maxValue - minValue) / 2;
        public int Random => minValue < maxValue ? UnityEngine.Random.Range(minValue, maxValue + 1) : minValue;
    }
}
