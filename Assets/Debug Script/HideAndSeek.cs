using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class HideAndSeek : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Defining the tag, for smarter AI")]
    private string triggerTag;
    [SerializeField]
    private List<string> layerName;
    private int layerIndex;
    private Vector3 lastPosition;
    private bool isHiding;
    private AreaCheck areaCheck;
    private void Start() {
        areaCheck = GetComponent<AreaCheck>();
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
                lastPosition = areaCheck.DetectedTarget.transform.position;
                areaCheck.DetectedTarget.transform.position = this.transform.position;
                if(areaCheck.DetectedTarget.CompareTag(triggerTag))
                {
                    this.gameObject.layer = areaCheck.DetectedTarget.layer;
                }
                else
                {
                    areaCheck.DetectionLayer = LayerMask.GetMask(layerName[1]);
                    isHiding = true;
                }
            }
            else
            {
                SwitchAll(areaCheck.DetectedTarget.transform.parent.gameObject, true);
                areaCheck.DetectedTarget.transform.parent.position =lastPosition;
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

        Component[] components = newGameObject.GetComponents<Component>();
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
            if (transform.gameObject.layer == 8)
            {
                transform.gameObject.SetActive(condition);
            }
        }
    }

    private int GetIndexFromLayerName(string newLayerName)
    {
        int layerIndex = LayerMask.NameToLayer(newLayerName);
        Debug.Log(layerIndex);
        return layerIndex;
    }
}
