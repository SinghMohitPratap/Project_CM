using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRadioButton : MonoBehaviour
{

    public eDifficulty diffValue;

    // Get data from the playerprefs and set the difficulty level initially that was set before...
    void Start()
    {
        if (diffValue == GameManager.Instance.currentDifficulty)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else 
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    
}
