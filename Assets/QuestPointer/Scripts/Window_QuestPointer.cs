using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_QuestPointer : MonoBehaviour
{
    [SerializeField] Camera uiCamera;
    [SerializeField] Sprite arrowSprite;
    [SerializeField] Sprite crossSprite;
    [SerializeField] Transform target;

    Vector3 targetPosition;
    RectTransform pointerRectTransform;
    Image pointerImage;
    bool isActive = false;
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();

        if (canvas == null)
        {
            Debug.LogError("Canvas tidak ditemukan. Pastikan QuestPointer berada di dalam Canvas.");
        }
        if (pointerRectTransform == null || pointerImage == null)
        {
            Debug.LogError("GameObject 'Pointer' tidak ditemukan atau tidak memiliki RectTransform/Image.");
        }
        if (uiCamera == null)
        {
            uiCamera = Camera.main;
            Debug.LogWarning("uiCamera tidak diassign, menggunakan Camera.main.");
        }
    }

    void Start()
    {
        if (target != null)
        {
            Show(target);
        }
        else
        {
            Debug.LogWarning("Target tidak diassign di Inspector, penunjuk tidak akan aktif.");
        }
    }

    void Update()
    {
        if (target != null && canvas != null)
        {
            targetPosition = target.position;

            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            bool isBehindCamera = targetPositionScreenPoint.z < 0;

            if (isBehindCamera)
            {
                pointerImage.enabled = false;
                return;
            }

            pointerImage.enabled = true;

            float borderSize = 100f;
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize ||
                               targetPositionScreenPoint.x >= Screen.width - borderSize ||
                               targetPositionScreenPoint.y <= borderSize ||
                               targetPositionScreenPoint.y >= Screen.height - borderSize;

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 targetDir = new Vector2(
                targetPositionScreenPoint.x - screenCenter.x,
                targetPositionScreenPoint.y - screenCenter.y
            ).normalized;


            Vector3 pointerPosition;
            if (isOffScreen)
            {
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

                pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

                pointerImage.sprite = arrowSprite;

                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

                pointerPosition = cappedTargetScreenPosition;
            }
            else
            {
                pointerImage.sprite = crossSprite;

                Vector3 offsetTargetPosition = target.position + new Vector3(0, 0.8f, 0);
                pointerPosition = Camera.main.WorldToScreenPoint(offsetTargetPosition);
                pointerRectTransform.localEulerAngles = Vector3.zero;
            }

            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                pointerPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : uiCamera,
                out canvasPos
            );

            pointerRectTransform.anchoredPosition = canvasPos;
        }
        else if (target == null && pointerImage.enabled)
        {
            pointerImage.enabled = false;
            Debug.LogWarning("Target menjadi null selama permainan, menyembunyikan penunjuk.");
        }
    }

    public void Show(Transform newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
            targetPosition = newTarget.position;
            isActive = true;
            gameObject.SetActive(true);
            pointerImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("Show dipanggil dengan target null, penunjuk tidak diaktifkan.");
        }
    }

    public void Hide()
    {
        isActive = false;
        pointerImage.enabled = false;
    }
}