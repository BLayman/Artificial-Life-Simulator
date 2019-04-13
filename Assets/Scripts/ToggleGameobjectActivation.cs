using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameobjectActivation : MonoBehaviour
{

    public void toggleActivation()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            gameObject.GetComponent<EcoStatsPopulator>().populateFields();
        }
    }

}
