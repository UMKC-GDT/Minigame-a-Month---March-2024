using UnityEngine.SceneManagement;
using UnityEngine;

public class ExcapeController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
