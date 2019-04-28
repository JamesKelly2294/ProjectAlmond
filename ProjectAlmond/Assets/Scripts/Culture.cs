using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culture : MonoBehaviour
{
    CultureGenome genome;
    CultureRenderer cultureRenderer;
    CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        cultureRenderer = GetComponent<CultureRenderer>();
        System.Random rand = new System.Random();

        List<Allele> alleles = new List<Allele>(CultureGenome.Length);

        for(int i = 0; i < alleles.Capacity; i++)
        {
            alleles.Add(new Allele(rand.Next(0, Allele.AlleleStrength)));
        }

        genome = new CultureGenome(alleles.ToArray());
        cultureRenderer.Initialize(genome);

        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 mouseDownPosition;
    Quaternion mouseDownRotation;
    Transform anchorPoint;
    void OnMouseDown()
    {
        mouseDownPosition = transform.position;
        mouseDownRotation = transform.rotation;

        cameraController.RequestPanToAngle(cameraController.baseview, 1.0f);
    }

    void OnMouseDrag()
    {
        float distanceFromScreen = Camera.main.WorldToScreenPoint(transform.position).z;

        if(anchorPoint != null)
        {

            transform.position = anchorPoint.position;
            transform.rotation = anchorPoint.rotation;
        } else
        {
            transform.rotation = mouseDownRotation;
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromScreen));
        }

        RaycastHit hit;
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 25.0f, layerMask))
        {
            anchorPoint = hit.transform;
        } else
        {
            anchorPoint = null;
        }
    }

    void OnMouseUp()
    {
        if(anchorPoint)
        {
            transform.position = anchorPoint.position;
            transform.rotation = anchorPoint.rotation;

            anchorPoint.GetComponent<CultureAnchorPoint>().Attach();
        } else
        {
            transform.position = mouseDownPosition;
            transform.rotation = mouseDownRotation;
        }
    }
}
