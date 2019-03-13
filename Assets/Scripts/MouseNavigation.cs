using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseNavigation : MonoBehaviour
{
    int framesSinceClick;
    Vector3 dragOrigin;
    Camera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // zoom out
        if (scroll > 0f)
        {
            cam.orthographicSize /= 1.2f;
        }
        // zoom in
        else if (scroll < 0f)
        {
            cam.orthographicSize *= 1.2f;
        }

        // click and drag
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            framesSinceClick = 0;
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            framesSinceClick++;
            float scalar = .0001f;
            Vector3 diff = dragOrigin - Input.mousePosition;
            //Debug.Log(diff);
            Vector3 move = new Vector3(diff.x * scalar * cam.orthographicSize, diff.y * scalar * cam.orthographicSize, 0);

            cam.transform.Translate(move);
        }

    }
}
