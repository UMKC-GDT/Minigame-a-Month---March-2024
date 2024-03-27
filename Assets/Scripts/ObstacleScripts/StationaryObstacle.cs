using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StationaryObstacle : MonoBehaviour
{
    public UnityEvent obstacleHit; // event to attach other functions to; functions will be called when player collides with obstacle

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // requires the player to have tag "Player"
            obstacleHit.Invoke();
    }

}
