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
        isHolding = Input.GetKey(KeyCode.H);
        float saturation = 0f;
        if(isHolding){
            lerpTime = 0f;
            movementScript.enabled = false;
            ResetSight();
        }else{
            movementScript.enabled = true;
            lerpTime = 0f;
            SightActive();
        }
        saturation = colorAdjust.saturation.value;
        Debug.Log("Saturasi = "+ saturation);
    }

    void SightActive(){
        if(lerpTime < duration){
            lerpTime += Time.deltaTime;
            colorAdjust.saturation.value = Mathf.Lerp(normalSaturation, targetSaturation, lerpTime / duration);
        }
        
    }

    void ResetSight(){
        if(lerpTime < duration){
            lerpTime += Time.deltaTime;
            colorAdjust.saturation.value = Mathf.Lerp(targetSaturation, normalSaturation, lerpTime / duration);
        }
    }
}
