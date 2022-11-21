using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    private const string Tip = "Find light switcher on left side.";

    public static UiController Instance;

    [SerializeField] private CanvasGroup _interactionDisplay;
    [SerializeField] private TextMeshProUGUI _interactionText;
    [SerializeField] private TextMeshProUGUI _contentText;

    [SerializeField] private float _tipTime = 2f;
 
     private void Awake()
     {
         Instance = this;
     }

     public void DisplayTextHandler(float value, string msg, bool status = true)
     {
         _interactionText.gameObject.SetActive(status);
         _contentText.text = msg;
         _interactionDisplay.DOFade(value, 0.35f);
     }

     public IEnumerator ObjectTip()
     {
         yield return new WaitForSeconds(_tipTime/2);
         DisplayTextHandler(1f, Tip, false);
         yield return new WaitForSeconds(_tipTime);
         DisplayTextHandler(0f, Tip, false);
     }
}
