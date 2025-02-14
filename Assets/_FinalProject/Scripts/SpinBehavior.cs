using UnityEngine;

public class SpinBehavior : MonoBehaviour
{
    public GameObject sun;
    public float rotation;
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
        PlanetaryRotation();                                                // update object rotating on an axis
    }

    // used to rotate the planetary object
    void PlanetaryRotation() {
        transform.Rotate(0f, rotation * step, 0f);
    }
}
