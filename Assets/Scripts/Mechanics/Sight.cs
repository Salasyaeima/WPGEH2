using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sight : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] Transform objectToLock;
    [SerializeField] StarterAssetsInputs starterAssetInput;
    [SerializeField] string xRaySound = "focusloop";
    Vector3 lockedPos;
    ColorAdjustments colorAdjust;
    public bool xrayActive = false;
    bool isXRaySFXPlaying = false;

    void Start()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.saturation.value = 30f;
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
            if (!isXRaySFXPlaying)
            {
                AudioManager.instance.PlayLoopingSFX(xRaySound, 0.3f);
                isXRaySFXPlaying = true;
            }
            starterAssetInput.move = new Vector2(0, 0);
            colorAdjust.saturation.value = -100f;
        }
        else
        {
            if (isXRaySFXPlaying)
            {
                AudioManager.instance.StopLoopingSFX(xRaySound);
                isXRaySFXPlaying = false;
            }
            xrayActive = false;
            colorAdjust.saturation.value = 30f;
        }
        saturation = colorAdjust.saturation.value;
    }
}
