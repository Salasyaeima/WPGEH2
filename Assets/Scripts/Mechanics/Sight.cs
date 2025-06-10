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
            AudioManager.instance.PlayLoopingSFX(xRaySound, 0.3f);
            starterAssetInput.move = new Vector2(0, 0);
            colorAdjust.saturation.value = -100f;
        }
        else
        {
            AudioManager.instance.StopLoopingSFX(xRaySound);
            xrayActive = false;
            colorAdjust.saturation.value = 0f;
        }
        saturation = colorAdjust.saturation.value;
    }
}
