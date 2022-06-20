using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loader_Dots : MonoBehaviour
{
    public TextMeshProUGUI LoadingText; 

    void Start()
    {
        StartCoroutine(Dots());
    }

    public bool o;
    public bool oo;
    public bool ooo = true;

    public IEnumerator Dots()
    {
        yield return new WaitForSeconds(2);

        if (LoadingText.text == "Loading...")
        {
            ooo = false;
            oo = false;
            o = true;
        }
        if (LoadingText.text == "Loading..")
        {
            ooo = true;
            oo = false;
            o = false;
        }
        if (LoadingText.text == "Loading.")
        {
            ooo = false;
            oo = true;
            o = false;
        }

        // ooo = ... ,  oo = .. , o = . Loading(...)

        if (ooo)
        {
            LoadingText.text = "Loading...";
        }
        if (oo)
        {
            LoadingText.text = "Loading..";
        }
        if (o)
        {
            LoadingText.text = "Loading.";
        }
        StartCoroutine(Dots());
        yield return null;
    }
}
