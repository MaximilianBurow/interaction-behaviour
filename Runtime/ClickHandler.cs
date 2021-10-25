using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InteractionBehaviour {
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float maxRaycastDistance = 20f;
        [SerializeField] private float clickRate = 0.5f;
        [SerializeField] private float clickDuration = 0.5f;

        private float nextClick = 0;
        private float mouseDownTime;

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0)) 
            {
                mouseDownTime = Time.time;
            }
            
            if (Input.GetMouseButtonUp(0) && Time.time > nextClick)
            {
                var timeSinceMouseDown = Time.time - mouseDownTime;
                if (timeSinceMouseDown > clickDuration) return;
                
                nextClick = Time.time + clickRate;
                HandleClick();
            }
        }

        private void HandleClick() {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance)) {
                var interactionBehaviours = hit.collider.gameObject.GetComponents<IInteractionBehaviour>();
                if (interactionBehaviours == null || interactionBehaviours.Length == 0) return;

                foreach (var ib in interactionBehaviours) {
                    ib.HandleInteraction();
                }
            }
        }

    }
}
