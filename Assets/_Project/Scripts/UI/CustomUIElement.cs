using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TriplanoTest.UIElements
{
    public class CustomUIElement : UIBehaviour, ICancelHandler, IPointerEnterHandler
    {
        [SerializeField] private UnityEvent onCancel;

        public void OnCancel(BaseEventData eventData) => onCancel.Invoke();

        public void OnPointerEnter(PointerEventData eventData) => EventSystem.current.SetSelectedGameObject(gameObject);
    }
}