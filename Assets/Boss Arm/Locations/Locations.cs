using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locations : MonoBehaviour
{
    public List<GameObject> locations;

    public void Repopulate() {
        locations = new();
        foreach(Transform child in transform) {
            locations.Add(child.transform.gameObject);
        }
    }
}
