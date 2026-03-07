using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    public void ShowHint()
    {
        if (hint != null)
            hint.SetActive(true);
    }

    public void HideHint()
    {
        if (hint != null)
            hint.SetActive(false);
    }
}