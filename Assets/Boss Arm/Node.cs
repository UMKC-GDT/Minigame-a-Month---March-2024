using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Node : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public List<GameObject> adjacentNodes;
    public float radius = 5f; // Radius of the circle cast
    public LayerMask layerMask; // Layer mask to filter out unwanted hits

    public void FindAdjacentNodes()
    {
        adjacentNodes = new();
        if(boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        // Circle cast to see all nearby nodes
        RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, radius, Vector2.right);
        //Debug.Log(hits.Length);
        foreach (var hit in hits)
        {
            // Check if the hit has the same tag and is not this gameObject
            if (hit.collider != null && hit.transform.gameObject.tag == gameObject.tag && hit.transform.gameObject != gameObject)
            {
                // Check for line of sight
                Vector2 directionToTarget = hit.transform.position - transform.position;
                RaycastHit2D[] sightTest = Physics2D.RaycastAll(transform.position, directionToTarget, radius, layerMask);
                foreach(var hit2 in sightTest) {
                    if(hit2.transform.CompareTag("Walls"))
                    {
                        // Debug.Log("Hit a wall");
                        break;
                    }

                    if (hit2.collider != null && hit2.collider.gameObject == hit.transform.gameObject)
                    {
                        // Add to adjacent nodes if it shares a tag and is in line of sight
                        adjacentNodes.Add(hit.transform.gameObject);
                    }
                }
            }
        }
    }
}
