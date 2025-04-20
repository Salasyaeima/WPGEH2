using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sight : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] Transform objectToLock;
    [SerializeField] MonoBehaviour movementScript;
    bool isHolding = false;
    Vector3 lockedPos;
    ColorAdjustments colorAdjust;
    public bool xrayActive = false;

    void Start()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.saturation.value = 0f;
            Debug.Log("Color Curves ditemukan!");
        }
        else
        {
            Debug.LogError("Color Curves tidak ditemukan!");
        }
    }

    void Update()
    {
        
        float saturation = 0f;
        if(Input.GetKey(KeyCode.Tab)){
            xrayActive = true;
            movementScript.enabled = false;
            colorAdjust.saturation.value = -100f;
        }else{
            movementScript.enabled = true;
            xrayActive = false;
            colorAdjust.saturation.value = 0f;
        }
        saturation = colorAdjust.saturation.value;
        Debug.Log("Saturasi = "+ saturation);
    }
}
