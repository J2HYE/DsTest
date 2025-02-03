using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompassIndicater : MonoBehaviour
{
    public static CompassIndicater Instance { get; private set; }
    [SerializeField] private RectTransform compassBar;
    [SerializeField] private RectTransform markerPrefab;
    private Transform player;
    private RectTransform[] compassMarkers;
    private List<Transform> targets;

    private List<RectTransform> activeMarkers = new List<RectTransform>();
    private float compassWidth = 1600f;

    private float maxVisibleDistance = 200f;
    private float minDistanceForScaling = 50f;
    private Color nearColor = Color.green;
    private Color farColor = Color.red;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        compassBar.transform.parent.gameObject.SetActive(true);
        compassMarkers = new RectTransform[compassBar.childCount];
        for (int i = 0; i < compassBar.childCount; i++)
        {
            compassMarkers[i] = compassBar.GetChild(i).GetComponent<RectTransform>();
        }
        targets = new List<Transform> ();
    }

    void Start()
    {
        player = GameManager.playerTransform;
        SetupMarkers();
        InitializeTargetMarkers();
    }

    void Update()
    {
        if (targets == null || targets.Count == 0) return;
        UpdateMarkers();
        UpdateTargetMarkers();
    }

    void SetupMarkers()
    {
        for (int i = 0; i < compassMarkers.Length; i++)
        {
            float positionX = (i - 6) * (compassWidth / 12);
            compassMarkers[i].anchoredPosition = new Vector2(positionX, -3.73f);
        }
    }

    void InitializeTargetMarkers()
    {
        if (targets == null || targets.Count == 0) return;

        foreach (Transform target in targets)
        {
            RectTransform newMarker = Instantiate(markerPrefab, compassBar);
            activeMarkers.Add(newMarker);
        }
    }

    void UpdateMarkers()
    {
        float playerRotationY = player.eulerAngles.y;

        for (int i = 0; i < compassMarkers.Length; i++)
        {
            float angleOffset = (i * (360f / compassMarkers.Length)) - playerRotationY;
            float normalizedOffset = angleOffset / 360f;
            float markerX = normalizedOffset * compassWidth;
            markerX = Mathf.Repeat(markerX + compassWidth / 2, compassWidth) - compassWidth / 2;
            compassMarkers[i].anchoredPosition = new Vector2(markerX, compassMarkers[i].anchoredPosition.y);
        }
    }


    void UpdateTargetMarkers()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Transform target = targets[i];
            RectTransform marker = activeMarkers[i];

            Vector3 directionToTarget = target.position - player.position;
            float distance = Vector3.Distance(target.position, player.position);
            float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            float relativeAngle = Mathf.DeltaAngle(player.eulerAngles.y, targetAngle);

            float markerX = (relativeAngle / 360f) * compassWidth;

            markerX = Mathf.Repeat(markerX + compassWidth / 2, compassWidth) - compassWidth / 2;
            marker.anchoredPosition = new Vector2(markerX, marker.anchoredPosition.y);

            TextMeshProUGUI[] distanceText = marker.GetComponentsInChildren<TextMeshProUGUI>();

            Color markerColor = Color.Lerp(nearColor, farColor, Mathf.Clamp01(distance / maxVisibleDistance));
            marker.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = markerColor;
            if (distanceText != null)
            {
                distanceText[1].color = markerColor;
                distanceText[1].text = distance.ToString("N0") + "m";
                distanceText[0].faceColor = markerColor;
                distanceText[0].text = target.name;
            }

            float scale = Mathf.Clamp(1f - (distance - minDistanceForScaling) / (maxVisibleDistance - minDistanceForScaling), 0.5f, 1f);
            marker.GetChild(0).localScale = Vector3.one * scale;

            marker.gameObject.SetActive(distance <= maxVisibleDistance);
        }
    }

    public static void AddTarget(Transform newTarget)
    {
        if (Instance.targets.Contains(newTarget)) return;
        Instance.targets.Add(newTarget);
        RectTransform newMarker = Instantiate(Instance.markerPrefab, Instance.compassBar);
        Instance.activeMarkers.Add(newMarker);
    }

    public static void RemoveTarget(Transform targetToRemove)
    {
        int index = Instance.targets.IndexOf(targetToRemove);
        if (index >= 0)
        {
            Destroy(Instance.activeMarkers[index].gameObject);
            Instance.activeMarkers.RemoveAt(index);
            Instance.targets.RemoveAt(index);
        }
    }
}
