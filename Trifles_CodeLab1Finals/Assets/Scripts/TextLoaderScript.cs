using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLoaderScript : MonoBehaviour
{
    public AudioSource buttonSound;
    
    //when the next button is clicked
    public void NextTextFile()
    {
        //clean the dialogue on screen
        GameManager.instance.ClearPage();
        
        //play button sound
        buttonSound.PlayOneShot(buttonSound.clip);
        
        InvokeRepeating("WaitUntilSoundEnds", 1, 0.5f);

    }

    void WaitUntilSoundEnds()
    {
        if (buttonSound.isPlaying)
        {
            return;
        }
        else
        {
            //read the next text file
            GameManager.instance.LoadNextFile();
            CancelInvoke("WaitUntilSoundEnds");
        }
    }

}
