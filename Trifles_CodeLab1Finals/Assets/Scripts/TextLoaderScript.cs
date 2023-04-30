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
        GameManager.instance.dialogue.text = string.Empty;
        
        //play button sound
        buttonSound.PlayOneShot(buttonSound.clip);
        
        //read the next text file
        GameManager.instance.CurrentTextFile++;
    }
    
}
