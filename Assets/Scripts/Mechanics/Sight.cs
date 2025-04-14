using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sight : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] float duration = 2f;
    [SerializeField] Transform objectToLock;
    [SerializeField] MonoBehaviour movementScript;
    float lerpTime = 0f;
    float targetSaturation = -100;
    float normalSaturation = 0f;
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
            lerpTime = 0f;
            movementScript.enabled = false;
            colorAdjust.saturation.value = -100f;
        }else{
            movementScript.enabled = true;
            xrayActive = false;
            lerpTime = 0f;
            colorAdjust.saturation.value = 0f;
        }
        saturation = colorAdjust.saturation.value;
        Debug.Log("Saturasi = "+ saturation);
    }
}
