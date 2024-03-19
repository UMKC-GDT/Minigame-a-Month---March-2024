using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BossArmController : MonoBehaviour {
    public LineRenderer lineRenderer;
    public List<Vector2> armPositions = new();
    public Vector2 lastPosition;
    public Vector2 currentPosition;
    public float maxSegmentDistance = 1.0f;

    private void Start() {
        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
        }
        ClearLineRenderer();
    }
    public void StartAttack(GameObject player) {
        ClearLineRenderer();
    }

    private void ClearLineRenderer() {
        lineRenderer.positionCount = 0;
    }

    private IEnumerator ChaseUnderling() {
        yield return new WaitForSeconds(0.1f);
    }
}
