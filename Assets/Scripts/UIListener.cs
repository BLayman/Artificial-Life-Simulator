using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListener : MonoBehaviour
{

    public void setVisibleResource(string resource)
    {
        gameObject.GetComponent<ThreadManager>().setVisibleResource(resource);
    }
}
