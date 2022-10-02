using System.Collections;
using UnityEngine;
using Library.Logging;
using GoapUnityAi;

namespace MainGame
{
    public class MoveTester : MonoBehaviour
    {
        public float maxSizeMultiplier = 1.1f;
        public float timeToReachMaxSize = 0.1f;

        public LayerMask layersWithSelectableObjects;

        public GoapAgent selectedAgent;
        public Transform clickedOnTransform;

        private WaitForEndOfFrame waitForEndOfFrame;

        void Start()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
        }

        void Update()
        {
            clickedOnTransform = GetClickedOnAgent();
            var layer = GetLayerOfClickedObject();

            if (IsLayerPartOfLayerMask(layer, layersWithSelectableObjects))
            {
                DebugLogger.LogDebug($"Clicked on selectable object layer {LayerMask.LayerToName(layer)}");
                OnClickingSelectableLayer();
            }

            // get selected object from sensor and set action to execute moveToAction
            // Smart objects can have goals and get the goal in future testing
            // selectedTransform?.
            ExecuteMoveToActionOnAgent();
        }

        private void ExecuteMoveToActionOnAgent()
        {
            if (selectedAgent == null)
            {
                return;
            }

            var moveToAction = selectedAgent.GetAction<WorkInFieldAction>();
            selectedAgent.SetActionToExecute(moveToAction);
        }

        private Transform GetClickedOnAgent()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return null;
            }
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var anGoapAgentWasClicked = Physics.Raycast(ray, out hit, 100.0f, layersWithSelectableObjects);

            if (!anGoapAgentWasClicked)
            {
                return null;
            }

            clickedOnTransform = hit.transform;
            // pointOfClicking = hit.point;

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

        private void OnClickingSelectableLayer()
        {
            if (clickedOnTransform == null)
            {
                return;
            }

            selectedAgent = clickedOnTransform.GetComponent<GoapAgent>();

            if (selectedAgent != null)
            {
                StartCoroutine(ScaleSelected());
            }
        }

        private IEnumerator ScaleSelected()
        {
            var selectedTransform = selectedAgent.transform;
            var startingLocalScale = selectedTransform.localScale;
            var maxSize = selectedTransform.localScale * maxSizeMultiplier;

            var waitTimeInSeconds = timeToReachMaxSize / 10;
            var waitTime = new WaitForSeconds(waitTimeInSeconds);
            var deltaScale = (maxSize - startingLocalScale) / 10;

            var startTime = Time.time;
            while (selectedTransform.localScale.x < maxSize.x)
            {
                selectedTransform.localScale += deltaScale;
                yield return waitTime;
            }
            while (selectedTransform.localScale.x > startingLocalScale.x)
            {
                selectedTransform.localScale -= deltaScale;
                yield return waitTime;
            }
            selectedTransform.localScale = startingLocalScale;
        }
    }
}
