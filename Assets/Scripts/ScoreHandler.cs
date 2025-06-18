using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour,IReset
{
    public TMP_Text matchText;
    public TMP_Text turnText;
    int matchvalue;
    int turnvalue;

    private void Start()
    {
        matchvalue = 0;
        turnvalue = 0;  
    }
    public void ChangeMacthTextValue(int value) 
    {
        matchvalue += value;
        matchText.text = matchvalue.ToString();
    }
    public  void ChangeturntextValue(int value) 
    {
        turnvalue += value;
        turnText.text = turnvalue.ToString();
    }
    private void OnEnable()
    {
        GameManager.GameReset += ResetValues;
    }
    private void OnDisable()
    {
        matchvalue = 0;
        turnvalue = 0;
        GameManager.GameReset -= ResetValues;
    }

    

   public void ResetValues()
    {
     
        matchvalue = 0;
        turnvalue = 0;
        matchText.text = matchvalue.ToString();
        turnText.text = turnvalue.ToString();
    }
}

