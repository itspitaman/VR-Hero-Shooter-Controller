using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Grabber : MonoBehaviour
{
    public int currentlyInteractingLayerInt;
    private int previousLayerInt;

    GameObject heldObject;

    public bool inBackpack;
    public InputActionReference gripAction;
    public GameObject marshmallowPrefab;
    public GameObject kindlingPrefab;

    XRDirectInteractor directInteractor;

    // Start is called before the first frame update
    void Start()
    {
        directInteractor = GetComponent<XRDirectInteractor>();
        gripAction.action.started += GripStarted;
        gripAction.action.canceled += GripCanceled;
    }

    private void OnDestroy()
    {
        gripAction.action.started -= GripStarted;
        gripAction.action.canceled -= GripCanceled;
    }

    // Update is called once per frame
    void Update() 
    {
        
    }

    void GripStarted(InputAction.CallbackContext context)
    {
        if (inBackpack && heldObject == null)
        {
            if(Backpack.Instance.currentGrabMode == Backpack.GrabMode.Marshmallow && Backpack.Instance.marshmallowCount > 0)
            {
                Debug.Log("Removing marshmallow.");
                Backpack.Instance.marshmallowCount--;
                Backpack.Instance.UpdateUIText();
                heldObject = Instantiate(marshmallowPrefab, transform.position, transform.rotation);
                directInteractor.StartManualInteraction(heldObject.GetComponent<IXRSelectInteractable>());
            }
            
            if(Backpack.Instance.currentGrabMode == Backpack.GrabMode.Kindling && Backpack.Instance.kindlingCount > 0)
            {
                Debug.Log("Removing kindling.");
                Backpack.Instance.kindlingCount--;
                Backpack.Instance.UpdateUIText();
                heldObject = Instantiate(kindlingPrefab, transform.position, transform.rotation);
                directInteractor.StartManualInteraction(heldObject.GetComponent<IXRSelectInteractable>());
            }
        }
    }

    void GripCanceled(InputAction.CallbackContext context)
    {
        if(heldObject != null && directInteractor.isPerformingManualInteraction)
        {
            directInteractor.EndManualInteraction();
        }
    }

    public void AssignHeldObject(SelectEnterEventArgs selectionInfo)
    {
        heldObject = selectionInfo.interactableObject.transform.gameObject;
        previousLayerInt = heldObject.layer;
        heldObject.layer = currentlyInteractingLayerInt;
    }

    public void UnassignHeldObject(SelectExitEventArgs selectionInfo)
    {
        if(heldObject != null)
        {
            heldObject.layer = previousLayerInt;
            heldObject = null;
        }
    }
}
