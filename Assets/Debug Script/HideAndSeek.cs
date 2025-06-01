using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class HideAndSeek : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Defining the tag, for smarter AI")]
    private string triggerTag;
    [SerializeField]
    private List<string> layerName;
    private GameObject noneObject;
    private Vector3 lastPosition;
    private bool isHiding = false;
    private AreaCheck areaCheck;
    private Transform targetTransform;
    private LayerMask targetLayer;
    private void Start() {
        areaCheck = GetComponent<AreaCheck>();
        targetTransform = areaCheck.DetectedTarget.transform;
        targetLayer = areaCheck.DetectedTarget.layer;
    }

    private void Update() {
        PerformHide();
    }

    private void PerformHide()
    {
        if (areaCheck.CheckClickEvent())
        {
            if (!isHiding)
            {
                SwitchAll(areaCheck.DetectedTarget, false);
                lastPosition = targetTransform.position;
                targetTransform.position = this.transform.position;
                if(areaCheck.DetectedTarget.CompareTag(triggerTag))
                {
                    this.gameObject.layer = targetLayer;
                }
                else
                {
                    areaCheck.DetectionLayer = LayerMask.GetMask(layerName[1]);
                    isHiding = true;
                }
            }
            else
            {
                SwitchAll(targetTransform.parent.gameObject, true);
                targetTransform.parent.position = lastPosition;
                areaCheck.DetectionLayer = LayerMask.GetMask(layerName[0]);
                isHiding = false;
            }
        }
    }

    private void SwitchAll(GameObject newGameObject, bool condition)
    {
        CapsuleCollider capsuleCollider = newGameObject.GetComponent<CapsuleCollider>();
        Rigidbody rb = newGameObject.GetComponent<Rigidbody>();
        if(capsuleCollider != null){capsuleCollider.enabled = condition;}
        if(rb != null){rb.useGravity = condition;}

        Component[] components = newGameObject.GetComponentsInChildren<Component>();
        foreach (Component component in components)
        {
            if (component is MonoBehaviour)
            {
                (component as MonoBehaviour).enabled = condition;
            }
        }

        Transform[] transforms = newGameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform in transforms)
        {
            if (transform.gameObject.layer == LayerMask.GetMask("None"))
            {
                noneObject = transform.gameObject;
                Debug.Log(noneObject.name);
            }
            
            noneObject.SetActive(condition);

            if (noneObject != null)
            {
                
            }
        }
    }
}
