using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance;

    public enum GrabMode { Marshmallow, Kindling };
    public GrabMode currentGrabMode;

    public int marshmallowCount;
    public int kindlingCount;

    public Text marshmallowText, kindlingText;

    public GameObject marshmallowSelectBackground;
    public GameObject kindlingSelectBackground;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        marshmallowCount = 0;
        kindlingCount = 0;
        SetGrabModeMarshmallow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUIText()
    {
        marshmallowText.text = $"Marshmallow = {marshmallowCount}";
        kindlingText.text = $"Kindling = {kindlingCount}";
    }

    public void SetGrabModeMarshmallow()
    {
        currentGrabMode = GrabMode.Marshmallow;
        marshmallowSelectBackground.SetActive(true);
        kindlingSelectBackground.SetActive(false);
    }

    public void SetGrabModeKindling()
    {
        currentGrabMode = GrabMode.Kindling;
        marshmallowSelectBackground.SetActive(false);
        kindlingSelectBackground.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Marshmallow"))
        {
            marshmallowCount++;
            UpdateUIText();
            Debug.Log("Added a marshmallow. Current count is: " + marshmallowCount);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Kindling"))
        {
            kindlingCount++;
            UpdateUIText();
            Debug.Log("Added some kindling. Current count is: " + kindlingCount);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Controller"))
        {
            other.gameObject.GetComponent<Grabber>().inBackpack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Controller"))
        {
            other.gameObject.GetComponent<Grabber>().inBackpack = false;
        }
    }
}
