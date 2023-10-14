using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerArea : XRBaseInteractor
{
    private XRBaseInteractable currentInteractable = null;

    public UnityEvent entered;
    public UnityEvent exited;

    private void OnTriggerEnter(Collider other)
    {
        //SetInteractable(other);
        entered.Invoke();
    }

    //private void SetInteractable(Collider other)
    //{
    //    if(TryGetInteractable(other, out XRBaseInteractable interactable))
    //    {
    //        if(currentInteractable == null) {
    //            currentInteractable = interactable;
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        //ClearInteractable(other);
        exited.Invoke();
    }

    //private void ClearInteractable(Collider other)
    //{
    //    if (TryGetInteractable(other, out XRBaseInteractable interactable)) {
    //        if (currentInteractable == interactable) {
    //            currentInteractable = null;
    //        }
    //    }
    //}

    //private bool TryGetInteractable(Collider collider, out XRBaseInteractable interactable)
    //{
    //    interactable = interactionManager.GetInteractableForCollider(collider);
    //    return interactable != null;
    //}

    //public override void GetValidTargets(List<XRBaseInteractable> targets)
    //{
    //    targets.Clear();
    //    targets.Add(currentInteractable);
    //}

    //public override bool CanHover(XRBaseInteractable interactable)
    //{
    //    return base.CanHover(interactable) && currentInteractable == interactable;
    //}

    //public override bool CanSelect(XRBaseInteractable interactable)
    //{
    //    return false;
    //}
}
