using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Data
{
    [CreateAssetMenu(menuName = MenuPath.Data + "Coin", fileName = "New" + nameof(CoinData))]
    public class CoinData : ScriptableObject
    {
        [Header("Coin")]
        [SerializeField] private int value;
        [SerializeField] private Color color;

        public int Value => value;
        public Color Color => color;
    }
}