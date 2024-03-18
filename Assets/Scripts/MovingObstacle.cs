using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingObstacle : MonoBehaviour
{
    // public variables
    public UnityEvent obstacleHit;            // event to attach other functions to; functions will be called when player collides with obstacle
    public List<GameObject> waypoints;        // waypoints for the obstacle to move between; use the waypoint prefabs
    public float obstacleSpeed = 1.0f;        // the movement speed of the obstacle in units / second

    // private variables
    private float originalSpeed;              // the original speed
    private bool isMovingToggle = true;       // flag for whether or not the obstacle is currently set to move
    private GameObject currentWaypoint;       // the waypoint towards which the obstacle will move
    private Vector2 currentWaypointDirection; // the direction of the obstacle -> waypoint

    private void Awake() 
    {
        originalSpeed = obstacleSpeed;
        currentWaypoint = (waypoints.Count > 0) ? waypoints[0] : null;
        currentWaypointDirection = GetDirection(currentWaypoint);
    }
    private void OnTriggerEnter(Collider other)
    {
        // if the obstacle collides with the player
        if (other.CompareTag("Player"))
            obstacleHit.Invoke();                                     // execute all methods assigned to obstacleHit

        // if the obstacle collides with a waypoint
        else if (other.CompareTag("Waypoint"))
        {
            currentWaypoint = GetNextWaypoint(currentWaypoint);       // get the next waypoint
            currentWaypointDirection = GetDirection(currentWaypoint); // get normalized vector of obstacle -> waypoint
        }
    }

    private void Update()
    {
        MoveObstacle();
    }

    // method for getting the next waypoint
    private GameObject GetNextWaypoint(GameObject currentWaypoint) { return waypoints[(waypoints.IndexOf(currentWaypoint) + 1) % waypoints.Count]; }

    // method for getting the direction between the obstacle and the current waypoint
    private Vector2 GetDirection(GameObject waypoint)
    {
        Vector2 direction = waypoint.transform.position - this.transform.position;
        return direction.normalized;
    }

    // method for moving the obstacle towards the current waypoint
    private void MoveObstacle()
    {
        Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        currentPosition += currentWaypointDirection * Time.deltaTime;
        this.transform.position = currentPosition;
    }


    // method for toggling speed; example use case: stopping NPC's movement while they talk to the player
    public void ToggleMovement()
    {
        isMovingToggle = !isMovingToggle;                           // toggle flag
        obstacleSpeed = (isMovingToggle) ? originalSpeed : 0.0f;    // update speed based on flag
    }
}
