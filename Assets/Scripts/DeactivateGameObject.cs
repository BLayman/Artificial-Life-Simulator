using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateGameObject : MonoBehaviour
{
    public GameObject toDeactivate;


    public void deactivateObject()
    {
        toDeactivate.SetActive(false);
    }
}
