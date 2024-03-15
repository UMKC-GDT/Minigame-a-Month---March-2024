using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Strike : MonoBehaviour
{
    public bool isActive;
    public Image cross;

    // Start is called before the first frame update
    void Start()
    {
        cross.enabled = false;
        isActive = false;
    }

    public void activate()
    {
        cross.enabled = true;
        isActive = true;
    }

    public void deactivate()
    {
        cross.enabled = false;
        isActive = false;
    }
}
