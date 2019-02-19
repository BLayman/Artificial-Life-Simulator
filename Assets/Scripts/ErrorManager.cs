using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    public GameObject errorObj;

    private void Awake()
    {
        HelperValidator.errorObj = errorObj;
    }

}
