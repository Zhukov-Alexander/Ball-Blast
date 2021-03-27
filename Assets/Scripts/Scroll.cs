using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;
[RequireComponent(typeof(ScrollRect))]
public class Scroll : MonoBehaviour
{
    ScrollRect scrollRect;
    RectTransform scrollView;
    [SerializeField] RectTransform container;
    RectTransform activeRectTransform;
    List<RectTransform> rectTransforms;
    List<Vector2> rectTransformsContainerPositiones;
    public Action<RectTransform> OnActiveRectTransformChanged;
    public void Set(int activeRectIndex)
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollView = GetComponent<RectTransform>();
        rectTransforms = container.gameObject.GetComponentsInChildrenNonRecursive<RectTransform>();
        CalculateRectTransformsContainerPositiones();
        SetContainerPosition(activeRectIndex);
    }
    private void FixedUpdate()
    {
        List<float> distances = rectTransforms.Select(a => Mathf.Abs(a.position.x - scrollView.position.x)).ToList();
        int index = distances.IndexOf(distances.Min());
        RectTransform rectTransformChosen = rectTransforms[index];

        if (activeRectTransform == null || activeRectTransform != rectTransformChosen)
        {
            activeRectTransform = rectTransformChosen;
            OnActiveRectTransformChanged(activeRectTransform);
        }

        float distanceX = rectTransformsContainerPositiones[index].x - container.anchoredPosition.x;

        if (Mathf.Abs(distanceX) <= gameConfig.minDistance && Mathf.Abs(scrollRect.velocity.x) < gameConfig.minScrollVelocity && !Input.GetMouseButton(0))
        {
            scrollRect.velocity = Vector2.zero;
            container.anchoredPosition = rectTransformsContainerPositiones[index];
        }
        if (Mathf.Abs(distanceX) > gameConfig.minDistance && !Input.GetMouseButton(0))
        {
            float moveForceX = Mathf.Sign(distanceX) * ( 1 / Mathf.Abs(distanceX) * gameConfig.snapMoveCoef + gameConfig.constSnapMove);
            container.anchoredPosition += new Vector2(moveForceX, 0);
        }
    }
    public void CalculateRectTransformsContainerPositiones()
    {
        rectTransformsContainerPositiones = new List<Vector2>();
        float rectLength = rectTransforms[0].sizeDelta.x;
        for (int i = 0; i < rectTransforms.Count; i++)
        {
            rectTransformsContainerPositiones.Add(new Vector3(-rectLength * (i + 1.5f), container.anchoredPosition.y));
        }
    }
    public void SetContainerPosition(int rectIndex)
    {
        container.anchoredPosition = rectTransformsContainerPositiones[rectIndex];
    }
}
