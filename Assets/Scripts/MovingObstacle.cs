using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingObstacle : MonoBehaviour
{
    public UnityEvent obstacleHit;     // event to attach other functions to; functions will be called when player collides with obstacle
    public List<GameObject> wayPoints; // waypoints for the obstacle to move between
    public float obstacleSpeed = 1.0f; // the movement speed of the obstacle in units / second

    private float originalSpeed;       // the original speed
    private bool isMovingToggle;       // flag for whether or not the obstacle is currently set to move 

    private void Awake() { originalSpeed = obstacleSpeed; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // requires the player to have tag "Player"
            obstacleHit.Invoke();
    }

    private void Update()
    {
        
    }

    // method for toggling speed
    public void ToggleMovement()
    {
        isMovingToggle = !isMovingToggle;                           // toggle flag
        obstacleSpeed = (isMovingToggle) ? originalSpeed : 0.0f;    // update speed based on flag
    }
}
