using UnityEngine;

public class XRay : MonoBehaviour
{
    [SerializeField] LayerMask defaultLayer;
    [SerializeField] LayerMask xRayLayer;
    [SerializeField] private Sight sight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if(sight.xrayActive == true)
        {
            int layerNum = (int)Mathf.Log(xRayLayer.value, 2);
            gameObject.layer = layerNum;
            if(transform.childCount > 0)
                SetLayerAllChildren(transform, layerNum);
        }
        else
        {
            int layerNum = (int)Mathf.Log(defaultLayer.value, 2);
            gameObject.layer = layerNum;
            if(transform.childCount > 0)
                SetLayerAllChildren(transform, layerNum);
        }
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach(var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
