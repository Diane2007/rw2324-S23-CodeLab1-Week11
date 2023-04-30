using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI location;
    public Button nextButton;
    
    //UI elements
    public GameObject map1, map2, player;
    public Button forward, backward, left, right;
    
    //connect with scriptable objects
    public LocationScriptableObject currentLocation;

    //init File.IO stuff
    const string TEXT_NAME = "textNum.txt";
    const string TEXT_DIR = "/Resources/Texts/";
    const string DATA_DIR = "/Resources/Data/";
    string TEXT_PATH;

    public AudioSource typewriterSound;

    //numbers to load texts
    int currentTextFile = 0;
    public int CurrentTextFile
    {
        get { return currentTextFile; }
        set
        {
            currentTextFile = value;

            if (currentTextFile < 4)
            {
                DialogueSystem();
            }
            else if (currentTextFile == 4)  //we enter the house here
            {
                //show the player and 1 fl minimap
                map1.gameObject.SetActive(true);
                player.gameObject.SetActive(true);
                
                //enable the rest of UI elements
                location.gameObject.SetActive(true);
                EnableLocationButtons(true);
                
                //load scriptable object files
                ScrObjWithDialogue();
            }
        }
    }

    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //no next button when the game begins
        nextButton.gameObject.SetActive(false);
        location.gameObject.SetActive(false);
        
        //disable UI elements until player enters the ground floor
        map1.gameObject.SetActive(false);
        map2.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        EnableLocationButtons(false);

        //define file paths
        TEXT_PATH = Application.dataPath + TEXT_DIR + TEXT_NAME;
        
        //clear the text at the first frame of each scene
        dialogue.text = string.Empty;
        
        //start the dialogue
        DialogueSystem();
        
    }

    //controlling all direction buttons in one go to make life easier
    void EnableLocationButtons(bool state)
    {
        forward.gameObject.SetActive(state);
        backward.gameObject.SetActive(state);
        left.gameObject.SetActive(state);
        right.gameObject.SetActive(state);
    }

    //TODO Fix the typing time issue
    void DialogueSystem()
    {
        //define the new text path to load
        string newTextPath = TEXT_PATH.Replace("Num", currentTextFile + "");

        //put each line in the text file into an array
        string[] fileLines = File.ReadAllLines(newTextPath);
        
        //play typewriter sound
        typewriterSound.PlayOneShot(typewriterSound.clip);

        for (int lineNum = 0; lineNum < fileLines.Length; lineNum++)
        {
            string lineContents = fileLines[lineNum];
            
            //break down the line into individual characters and put in an array
            char[] lineChar = lineContents.ToCharArray();

            //start typing individual characters!!
             for (int charNum = 0; charNum < lineChar.Length + 1; charNum++)
             {
                 //every character takes 0.05 sec to type
                 if (charNum < lineChar.Length)
                 {
                     dialogue.text += lineChar[charNum];
                 }
                 else if (charNum == lineChar.Length)
                 {
                     dialogue.text += "\n" + "\n";
                 }
             }
        }
        typewriterSound.Stop();
        nextButton.gameObject.SetActive(true);
        
    }

    //TODO incorporate scriptable objects with dialogue system
    void ScrObjWithDialogue()
    {
        location.text = currentLocation.locationName;
        
    }

    //TODO: figure out how to invoke with parameters
     void Type(int num, char[] charArray)
    {
        //type characters
        if (num < charArray.Length)
        {
            dialogue.text += charArray[num];
        }
        //when we are at the end of the line, make an empty line
        else if (num == charArray.Length)
        {
            dialogue.text += "\n" + "\n";
        }
    }
}
