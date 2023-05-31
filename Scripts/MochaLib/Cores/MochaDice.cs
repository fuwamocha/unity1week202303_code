using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MochaLib.Cores
{
    [Serializable]
    public struct MochaDice
    {
        [SerializeField] private int diceCount;
        [SerializeField] private int sidedValue;

        public MochaDice(int count, int sided)
        {
            diceCount  = count;
            sidedValue = sided;
        }

        public int Rolled
        {
            get
            {
                var value = 0;

                for (var i = 0; i < diceCount; i++) value += Random.Range(1, sidedValue + 1);

                return value;
            }
        }
    }
}