using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sight : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] Transform objectToLock;
    [SerializeField] StarterAssetsInputs starterAssetInput;
    Vector3 lockedPos;
    ColorAdjustments colorAdjust;
    public bool xrayActive = false;

    void Start()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.saturation.value = 0f;
        }
        else
        {
            Debug.LogError("Color Curves tidak ditemukan!");
        }
    }

    void Update()
    {

        float saturation = 0f;
        if (Input.GetKey(KeyCode.Tab))
        {
            xrayActive = true;
            starterAssetInput.move = new Vector2(0, 0);
            colorAdjust.saturation.value = -100f;
        }
        else
        {
            xrayActive = false;
            colorAdjust.saturation.value = 0f;
        }
        saturation = colorAdjust.saturation.value;
    }
}
