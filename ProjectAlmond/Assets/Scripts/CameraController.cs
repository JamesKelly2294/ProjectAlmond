using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform title;
    public Transform overview;
    public Transform baseview;
    public Transform petriDishes;
    public Transform genimagic;
    public Transform checkomatic;
    public Transform amalgamizer;
    public Transform resources;

    // Start is called before the first frame update
    void Start()
    {

    }

    CameraFocus currentFocus;
    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.hasGameBegun())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = 1 << 11;
            int shitToIgnoreMask = 1 << 9 | 1 << 12 | 1 << 13;

            RaycastHit shitToIgnoreHit;
            Ray shitToIgnore = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool foundShitToIgnore = Physics.Raycast(shitToIgnore, out shitToIgnoreHit, Mathf.Infinity, shitToIgnoreMask);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool foundCameraFocus = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
            if (foundCameraFocus)
            {
                if(foundShitToIgnore)
                {
                    return;
                }

                CameraFocus focus = hit.transform.GetComponent<CameraFocus>();

                if(!focus)
                {
                    return;
                }

                if (focus == currentFocus)
                {
                    currentFocus = null;
                    RequestPanToAngle(baseview, 1.0f);
                } else if (focus.CameraView)
                {
                    currentFocus = focus;
                    RequestPanToAngle(focus.CameraView, 1.0f);
                }
                
            }
            else
            {
                RequestPanToAngle(baseview, 1.0f);
            }
        }
    }

    bool panning;
    bool rotating;
    public bool RequestPanToAngle(Transform angle, float transitionDuration)
    {
        Debug.Log("RequestPanToAngle");
        if (panning || rotating || (angle.position == transform.position && angle.rotation == transform.rotation))
        {
            return false;
        }

        panning = true;
        rotating = true;
        StartCoroutine(BeginPanToAngle(angle, transitionDuration));
        StartCoroutine(BeginPanToPosition(angle, transitionDuration));

        return true;
    }

    IEnumerator BeginPanToAngle(Transform angle, float transitionDuration)
    {
        float t = 0.0f;
        //Vector3 startingPos = transform.position;
        Quaternion startingRotation = transform.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.rotation = Quaternion.Slerp(startingRotation, angle.rotation, t);
            //transform.position = Vector3.Lerp(startingPos, angle.position, t);
            yield return 0;
        }

        Debug.Log("Finished panning");
        rotating = false;
    }

    IEnumerator BeginPanToPosition(Transform position, float transitionDuration)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Vector3 currentVelocity = Vector3.zero;

        Quaternion startingRotation = transform.rotation;
        while (true)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            Vector3 newPosition =
                Vector3.SmoothDamp(transform.position, position.position, ref currentVelocity, transitionDuration);

            if (Vector3.Distance(newPosition, position.position) <= 0.01f)
            {
                transform.position = position.position;
                break;
            }

            transform.position = newPosition;
            transitionDuration -= Time.deltaTime;

            yield return 0;
        }

        panning = false;
    }


}
