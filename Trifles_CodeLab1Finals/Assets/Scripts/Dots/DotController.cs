using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DotController : MonoBehaviour
{
    //all dots in each parent
    List<HeadCount1Fl> dots1Fl;
    List<HeadCount2Fl> dots2Fl;
    List<HeadCountHidden> dotsHidden;

    public static DotController instance;

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
        dots1Fl = GetComponentsInChildren<HeadCount1Fl>().ToList();
        dots2Fl = GetComponentsInChildren<HeadCount2Fl>().ToList();
        dotsHidden = GetComponentsInChildren<HeadCountHidden>().ToList();
        
        //hide all dots in each category at start
        Show1FlObjects(false);
        Show2FlObjects(false);
        ShowHiddenObjects(false);
        Debug.Log("Hiding dot: " + dotsHidden[0]);
    }

    public void Show1FlObjects(bool state)
    {
        //for each dot under the parent
        foreach (var dot in dots1Fl)
        {
            dot.gameObject.SetActive(state);
        }
    }

    //TODO: fill it in when 2fl objects are in
    public void Show2FlObjects(bool state)
    {
        foreach (var dot in (dots2Fl))
        {
            dot.gameObject.SetActive(state);
        }
    }

    public void ShowHiddenObjects(bool state)
    {
        foreach (var dot in dotsHidden)
        {
            dot.gameObject.SetActive(state);
        }
    }
}
