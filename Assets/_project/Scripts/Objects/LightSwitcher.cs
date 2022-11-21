using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
   public const string Switch = "to turn on/off light";

   [SerializeField] private InteractiveObject _lightSwitcherObj;
   [SerializeField] private GameObject[] lights;

   private bool _isLightsOn = true;

   private void Start()
   {
      Subscribe();
   }

   private void SwitcherLightsHandler()
   {
      _isLightsOn = !_isLightsOn;

      foreach (GameObject l in lights)
      {
         l.SetActive(_isLightsOn);
      }
   }

   private void SwitcherDisplay(bool status)
   {
      if (status)
      {
         UiController.Instance.DisplayTextHandler(1f, Switch);
         return;
      }

      UiController.Instance.DisplayTextHandler(0f, Switch);
   }

   private void Subscribe()
   {
      _lightSwitcherObj.onCanInteract += SwitcherDisplay;

      _lightSwitcherObj.onInteract += (p) => SwitcherLightsHandler();
   }
   
   private void UnSubscribe()
   {
      _lightSwitcherObj.onCanInteract -= SwitcherDisplay;

      _lightSwitcherObj.onInteract -= (p) => SwitcherLightsHandler();
   }

   private void OnDestroy()
   {
      UnSubscribe();
   }
}
