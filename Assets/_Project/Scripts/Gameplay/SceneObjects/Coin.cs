using TriplanoTest.Persistence;
using TriplanoTest.Data;
using UnityEngine;

namespace TriplanoTest.Gameplay
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private CoinData data;
        [SerializeField] private ActivationState activationState;
        [Space]
        [Tooltip("You can see properties in inspector by debug like \"_UnlitColor\"")]
        [SerializeField] private string colorProperty = "_Color";
        [SerializeField] private new Renderer renderer;

        private void Start()
        {
            renderer.material.SetColor(colorProperty, data.Color);
        }

        private void OnTriggerEnter(Collider other)
        {
            ICollector collector = other.attachedRigidbody.GetComponent<ICollector>();
            collector.AddCoin(data.Value);
            activationState.SetState(false);
            gameObject.SetActive(false);
        }
    }
}