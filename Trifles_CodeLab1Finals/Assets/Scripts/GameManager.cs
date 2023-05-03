using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Text")]
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI location;
    public TextMeshProUGUI exploreQuestion;

    [Header("UI Elements")]
    //UI elements
    public GameObject map1;
    public GameObject map2;
    public GameObject player;
    
    [Header("Buttons")]
    public Button nextButton;
    public Button exploreButton;
    
    [Space(10)]
    public Button forward;
    public Button backward;
    public Button left;
    public Button right;
    
    [Space(10)]
    //connect with scriptable objects
    public LocationScriptableObject currentLocation;
    public AudioSource typewriterSound;

    //init File.IO stuff
    const string TEXT_NAME = "textNum.txt";
    const string TEXT_DIR = "/Resources/Texts/";
    const string DATA_DIR = "/Resources/Data/";
    string TEXT_PATH;
    
    int lineIndex = 0;
    int charIndex = 0;
    string[] fileLines;
    
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
                nextButton.gameObject.SetActive(false);
                //Debug.Log("Current text file num is: " + currentTextFile);
                DialogueSystem();
            }
            else if (currentTextFile == 4)  //we enter the house here
            {
                //disable next button for now
                nextButton.gameObject.SetActive(false);
                
                //show the player and 1 fl minimap
                map1.gameObject.SetActive(true);
                player.gameObject.SetActive(true);
                
                //enable the rest of UI elements
                location.gameObject.SetActive(true);
                EnableLocationButtons(true);
                
                //load scriptable object files
                UpdateLocation();
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
        //no buttons when the game begins
        nextButton.gameObject.SetActive(false);
        exploreButton.gameObject.SetActive(false);
        
        //don't show location at the beginning
        location.gameObject.SetActive(false);
        
        //disable UI elements until player enters the ground floor
        map1.gameObject.SetActive(false);
        map2.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        exploreQuestion.gameObject.SetActive(false);
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

    public void LoadNextFile()
    {
        CurrentTextFile++;
    }

    //TODO Fix the audio issue.
    //isPlaying is not null at the supposed play time, and the audio clip is debugged correctly. Just no sound.
    void DialogueSystem()
    {
        //play typewriter sound
        if (typewriterSound)
        {
            typewriterSound.Play();
            Debug.Log("Play: " + typewriterSound.clip);
        }
        
        if (typewriterSound.isPlaying)
        {
            Debug.Log("Sound playing!");
        }
        
        //define the new text path to load
        string newTextPath = TEXT_PATH.Replace("Num", currentTextFile + "");
        
        //put each line in the text file into an array
        fileLines = File.ReadAllLines(newTextPath);
        
        //Debug.Log("Should print text now!");
        InvokeRepeating("TypeChar", 0, 0.05f);
    }

    void TypeChar()
    {
        //stop typing if we've finished all the lines
        if (lineIndex >= fileLines.Length)
        {
            //don't repeat invoke this function again
            CancelInvoke("TypeChar");
            
            //stop the typewriter sound
            //typewriterSound.Stop();
            
            //show the next button
            nextButton.gameObject.SetActive(true);

            //reset the line index or it would skip the next load
            lineIndex = 0;
            
            //stop running this code
            return;
        }

        //find the current line
        string typeLine = fileLines[lineIndex];

        //if we haven't finished typing a line
        if (charIndex < typeLine.Length)
        {
            //type the character
            dialogue.text += typeLine[charIndex];
            
            //if there is a * in the txt file
            if (typeLine[charIndex] == '*')
            {
                Invoke("ClearPage", 5f);   //clear the page after 3 sec
            }
            
            //char index increases for the next repeat invoke
            charIndex++;
        }
        //if we have finished typing a line
        else
        {
            //add an empty line
            dialogue.text += "\n" + "\n";
            
            //move to the next line
            lineIndex++;
            
            //start at the first character
            charIndex = 0;
        }
    }

    public void ClearPage()
    {
        dialogue.text = string.Empty;
    }

    //attached to the exploreButton to show triggered text only (from crime scene objects)
    public void ShowTriggeredText()
    {
        //clear the screen first
        dialogue.text = string.Empty;
        exploreQuestion.gameObject.SetActive(false);
        
        //read the triggered txt attached to that SO
        //and split it by lines, store into this array
        fileLines = currentLocation.triggeredText.text.Split('\n');
        
        //the button is no longer interactable
        exploreButton.interactable = false;
        
        //type characters like a typewriter again
        InvokeRepeating("TypeChar", 1, 0.05f);
        exploreButton.gameObject.SetActive(false);

    }

     void UpdateLocation()
     {
         //load the text from current scriptable object
         location.text = currentLocation.locationName;
         dialogue.text = currentLocation.locationDescription.text;

         //if the buttonInt is 1, show the exploreButton to trigger more text
         if (currentLocation.buttonInt == 1)
         {
             exploreButton.gameObject.SetActive(true);
             exploreQuestion.text = currentLocation.exploreQuestion;
         }
         
         //
         // //if there is some explore question in the SO
         // if (currentLocation.exploreQuestion != null)
         // {
         //     //customize the explore question
         //     exploreQuestion.text = currentLocation.exploreQuestion;
         // }
         // else
         // {
         //     //if there isn't, don't show the explore question
         //     exploreQuestion.gameObject.SetActive(false);
         // }

         //if currentLocation's forwardLocation doesn't exist
         //forward button is not interactable
         if (!currentLocation.forwardLocation)
         {
             forward.interactable = false;
         }
         else
         {
             forward.interactable = true;
             currentLocation.forwardLocation.backwardLocation = currentLocation;
         }
         
         //if current location's backwardLocation doesn't exist
         if (!currentLocation.backwardLocation)
         {
             backward.interactable = false;
         }
         else
         {
             backward.interactable = true;
             currentLocation.backwardLocation.forwardLocation = currentLocation;
         }
         
         //if current's left doesn't exist
         if (!currentLocation.leftLocation)
         {
             left.interactable = false;
         }
         else
         {
             left.interactable = true;
             currentLocation.leftLocation.rightLocation = currentLocation;
         }
         
         //if current's right doesn't exist
         if (!currentLocation.rightLocation)
         {
             right.interactable = false;
         }
         else
         {
             right.interactable = true;
             currentLocation.rightLocation.leftLocation = currentLocation;
         }
     }
     
     //the button directions
     //TODO: remember to update player position, and add button sound!
     public void ButtonDirection(int direction)
     {
         switch (direction)
         {
             case 0:    //forward
                 currentLocation = currentLocation.forwardLocation;
                 break;
             case 1:    //backward
                 currentLocation = currentLocation.backwardLocation;
                 break;
             case 2:    //left
                 currentLocation = currentLocation.leftLocation;
                 break;
             case 3:    //right
                 currentLocation = currentLocation.rightLocation;
                 break;
         }
         
         //update the texts
         UpdateLocation();
     }
     
     
}
