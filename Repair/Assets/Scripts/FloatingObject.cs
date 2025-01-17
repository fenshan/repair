﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//good and bad things floating in the screen [created from PopUpBadThings and DraggableObject]
public class FloatingObject : MonoBehaviour
{
    public const float RANGE = 0.20f; // min: 2 layers // max: 4 layers // [because bad things pop up in a random place in a range of the currentScroll]
    public const float MAX_VOLUME_BAD = 0.35f;

    public float ScrollingPlace; //scrolling place [0, 1] of this floating object
    public bool active = true;
    public bool gif;
    public bool bad;
    public bool wasBad = false;
    public bool CanFade = true;

    void Update()
    {
        //ACTIVE
        if (ScrollingPlace < GameManager.currentScroll + RANGE && ScrollingPlace > GameManager.currentScroll - RANGE)
        {
            active = true;
            gameObject.GetComponent<Image>().enabled = true;
            if (gif) gameObject.GetComponent<Animator>().enabled = true;
            if (bad) gameObject.GetComponent<AudioSource>().enabled = true;

            float aux = Mathf.Min(Mathf.Abs(GameManager.currentScroll + RANGE - ScrollingPlace), Mathf.Abs(-GameManager.currentScroll + RANGE + ScrollingPlace));

            //fade out fade in
            if (aux < RANGE / 2.0f && CanFade)
            {
                float alpha = aux * 2 / RANGE; //values between 0 and 1
                gameObject.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Lerp(0.3f, 1, alpha));
                if (bad) gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(0.1f, MAX_VOLUME_BAD, alpha);
            }
            //not fade in or fade out
            else
            {
                gameObject.GetComponent<Image>().color = Color.white;
                if (bad) gameObject.GetComponent<AudioSource>().volume = MAX_VOLUME_BAD;
                if (aux >= RANGE / 2.0f) CanFade = true;
            }
        }

        //INACTIVE
        else
        {
            active = false;
            CanFade = true;
            gameObject.GetComponent<Image>().enabled = false;
            if (gif) gameObject.GetComponent<Animator>().enabled = false;
            if (bad) gameObject.GetComponent<AudioSource>().enabled = false;
        }
    }

    //DEACTIVATE AUDIO
    public void DeactivateAudio()
    {
        if (bad)
        {
            gameObject.GetComponent<AudioSource>().volume = 0;
            bad = false;
            wasBad = true;
        }
    }
}
