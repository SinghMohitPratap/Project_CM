using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{

    public GameObject menuPanel;
    public Transform gearIcon;
    bool isClicked = false;
    bool isRunning = false;
    public Sprite volumeOff;
    public Sprite volumeOn;
    public GameObject volumeImage;
    public void OnclickGearIcon() 
    {
        if (!isRunning)
        {
            isClicked = !isClicked;
            if (isClicked)
            {

                StartCoroutine(ScaleGearPanel(0, 1));
            }
            else
            {
                StartCoroutine(ScaleGearPanel(1, 0));
            }
        }
    }
    float elapsedTime ;
    float totalTime = 0.5f;
    IEnumerator ScaleGearPanel(float iniValue,float finalvalue) 
    {

        isRunning = true;
        elapsedTime = 0;
        while (elapsedTime < totalTime) 
        {
            elapsedTime += Time.deltaTime;
            float value = Mathf.Lerp(iniValue, finalvalue, elapsedTime / totalTime);
            if (iniValue == 0)
            {
               gearIcon.transform.Rotate(new Vector3(0, 0, -10));
            }
            else
            {
               gearIcon.transform.Rotate(new Vector3(0, 0, 10));
            }
        
                menuPanel.transform.localScale = new Vector3(value, 1, 1);
            yield return null;
        }

        isRunning = false;
    }

    bool soundState=false;
    public void OnCLickVolumeBtn() 
    {
        soundState = !soundState;
        if (soundState) //turning volume off...
        {
            Debug.Log("volume off");
            volumeImage.GetComponent<Image>().sprite = volumeOff;
            SoundManager.Instance.sfxVolume = 0;
            SoundManager.Instance.backgroundVolume = 0;
        }
        else //turning volume on...
        {
            Debug.Log("volume on");
            volumeImage.GetComponent<Image>().sprite = volumeOn;
            SoundManager.Instance.sfxVolume = 1;
            SoundManager.Instance.backgroundVolume = 0.2f;
        }
        SoundManager.Instance.ChangeVolume();
    }
}
