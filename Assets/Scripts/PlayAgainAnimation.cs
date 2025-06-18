using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgainAnimation : MonoBehaviour
{

    GameObject play_again_btn;
    int screenHeight;
    int screenWidth;
    Vector3 resetPos;

    float firstAnim = 0.5f; // First Animation Duration
    float secondAnim = 0.25f;//second animation duration
    float timeElapsed = 0;

    public Transform target1;
    public Transform target2;
  
    void Awake()
    {
        play_again_btn = transform.GetChild(0).gameObject;
        resetPos = play_again_btn.transform.localPosition;
    }
   
    private void OnEnable()
    {
        play_again_btn.transform.localPosition = resetPos;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
       
        StartCoroutine(PlayButtonAnimation());
    }

    IEnumerator PlayButtonAnimation() 
    {
        Vector3 startPos = play_again_btn.transform.localPosition;
        Vector3 endPos1 = target1.transform.localPosition;
        Vector3 endPos2 = target2.transform.localPosition;

        while (timeElapsed<firstAnim)
        {
            timeElapsed += Time.deltaTime;        
            play_again_btn.transform.localPosition = Vector3.Lerp(startPos, endPos1,  timeElapsed/firstAnim);
            yield return null;
        }

        startPos = play_again_btn.transform.localPosition;
        timeElapsed = 0;

        while (timeElapsed < secondAnim)
        {
            timeElapsed += Time.deltaTime;
            play_again_btn.transform.localPosition = Vector3.Lerp(startPos, endPos2, timeElapsed / secondAnim);
            yield return null;
        }

    }

}
