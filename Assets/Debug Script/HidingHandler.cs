using System.Collections.Generic;
using UnityEngine;

public class HidingHandlers : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Tag if Chased by Enemy")]
    private string triggerTag;
    [SerializeField]
    private List<string> layerName;
    private GameObject noneObject;
    private Vector3 lastPosition;
    private bool isHiding = false;
    private AreaCheck areaCheck;
    private GameObject target;

    private void Start() {
        areaCheck = GetComponent<AreaCheck>();
    }

    private void Update() {
        PerformHide();
    }

    private void PerformHide()
    {
        target = areaCheck.DetectedTarget;
        if (areaCheck.CheckClickEvent())
        {
            if (!isHiding)
            {
                SwitchAll(target, false);
                lastPosition = target.transform.position;
                target.transform.position = this.transform.position;
                if(target.CompareTag(triggerTag))
                {
                    this.gameObject.layer = LayerMask.NameToLayer(layerName[3]);
                    
                }
                isHiding = true;
                areaCheck.DetectionLayer = LayerMask.GetMask(layerName[1]);
            }
            else
            {
                SwitchAll(target.transform.parent.gameObject, true);
                target.transform.parent.position = lastPosition;
                areaCheck.DetectionLayer = LayerMask.GetMask(layerName[0]);
                isHiding = false;
            }
            Debug.Log(isHiding);
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
                Debug.Log(component.name);
            }
        }

        Transform[] transforms = newGameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform in transforms)
        {
            if (transform.gameObject.layer == LayerMask.NameToLayer(layerName[2]))
            {
                noneObject = transform.gameObject;
            }

            if (noneObject != null)
            {
                noneObject.SetActive(condition);
            }
        }
    }
}
