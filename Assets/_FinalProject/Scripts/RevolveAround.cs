using UnityEngine;

public class RevolveAround : MonoBehaviour
{
    public GameObject sun;
    public float revolution;
    private bool isSun = false;
    private float step;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (sun == null || sun.CompareTag("Sun") == false) {                // sun gameObject was added and has appropriate tag
            sun = GameObject.FindGameObjectWithTag("Sun");                      // assign appropriate sun to variable
        }

        if (sun == null) {                                                  // verify sun was assigned
            Debug.LogError("Sun object not found in scene");                    // log error
            return;                                                             // stop running
        }

        if (gameObject.CompareTag("Sun")){                                  // if the current gameObject the sun
            isSun = true;                                                       // toggle isSun to true
            Debug.Log("object " + gameObject.name + " is the sun");             // debug log
        }

        Debug.Log("Sun object found and assigned to " + gameObject.name);   // verify sun to planet log
    }

    // Update is called once per frame
    void Update()
    {
        step = Time.deltaTime;                                              // calculate step

        if (!isSun) {                                                       // if object is not the sun
            PlanetaryRevolution();                                              // update object revolution
        }
    }

    // used to revolve around the sun
    void PlanetaryRevolution() {
        transform.RotateAround(sun.transform.position, Vector3.up, revolution * step);
    }
}
