using System;
using TMPro;
using UnityEngine;

namespace TriplanoTest.Player
{
    [Serializable]
    public sealed class PlayerUI : PlayerControllerComponent
    {
        [SerializeField] private GameObject grabIcon;
        [SerializeField] private TMP_Text coinCounter;

        private int totalGold;

        internal void Update()
        {
            bool canGrab = Controller.Physics.CanInteract();
            grabIcon.SetActive(canGrab);
        }

        internal void AddCoin(int amount)
        {
            totalGold += amount;
            coinCounter.text = totalGold.ToString();
        }
    }
}