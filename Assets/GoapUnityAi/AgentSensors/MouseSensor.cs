using UnityEngine;
using Library.Logging;

namespace GoapUnityAi
{
    public class MouseSensor : MonoBehaviour
    {
        public LayerMask layersWithSmartObjects;
        public Transform SelectedTransform { get; private set; }

        private Transform clickedOnTransform;
        private Vector3 pointOfClicking;

        private GoapAgent agent;

        private void Awake()
        {
            agent = GetComponent<GoapAgent>();
        }

        void Update()
        {
            clickedOnTransform = GetClickedOnObject();
            var layer = GetLayerOfClickedObject();

            if (IsLayerPartOfLayerMask(layer, layersWithSmartObjects))
            {
                DebugLogger.LogDebug($"Clicked on smart objects layer {LayerMask.LayerToName(layer)}");
                OnClickingSmartObject();
            }
        }

        private Transform GetClickedOnObject()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return null;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var aSmartObjectWasClicked = Physics.Raycast(ray, out hit, 100.0f, layersWithSmartObjects);

            if (!aSmartObjectWasClicked)
            {
                return null;
            }

            clickedOnTransform = hit.transform;
            pointOfClicking = hit.point;
            return hit.transform;
        }

        private bool IsLayerPartOfLayerMask(LayerMask layer, LayerMask includeLayers)
        {
            return ((1 << layer) & includeLayers) != 0;
        }

        private LayerMask GetLayerOfClickedObject()
        {
            if (clickedOnTransform == null)
            {
                return LayerMask.NameToLayer("Default");
            }
            return clickedOnTransform.gameObject.layer;
        }

        private void OnClickingSmartObject()
        {
            DebugLogger.LogDebug($"Clicked on the smart object {clickedOnTransform.gameObject.name}");
            SelectedTransform = clickedOnTransform;
            // agent.SetTargetPosition(pointOfClicking);
        }
    }
}
