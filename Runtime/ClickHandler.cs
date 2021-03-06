using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InteractionBehaviour {
    public class ClickHandler : MonoBehaviour
    {
        public Camera MainCamera { get; internal set; }

        public LayerMask LayerMask { get; set; }

        [SerializeField] private float maxRaycastDistance = 20f;
        [SerializeField] private float clickRate = 0.5f;
        [SerializeField] private float clickDuration = 0.5f;

        private float nextClick = 0;
        private float mouseDownTime;
        
        void Update()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

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
            var cam = MainCamera ?? Camera.main;
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, LayerMask)) {
                var interactionBehaviours = hit.collider.gameObject.GetComponents<IInteractionBehaviour>();
                if (interactionBehaviours == null || interactionBehaviours.Length == 0) return;

                foreach (var ib in interactionBehaviours) {
                    ib.HandleInteraction();
                }
            }
        }

    }
}
