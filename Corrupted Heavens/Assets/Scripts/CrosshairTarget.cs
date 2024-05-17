using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{

    public GameObject target;
    public GameObject crossHair;
    public bool isHit;
    public Transform position;

    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.gameObject == target)
            {
                isHit = true;
            }else
            {
                isHit = false;
            }

        }else
        {
            isHit = false;
        }

    }
}
