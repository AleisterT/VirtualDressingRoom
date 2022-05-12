using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RaycastModels : MonoBehaviour
{
    [SerializeField] private HandCursor cursor;
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] private List<GameObject> simulatedCloths;
    [SerializeField] private List<GameObject> canvasModels;
    [SerializeField] private GameObject hat;
    [SerializeField] private GameObject canvasLayer;
    [SerializeField] private GameObject settingsLayer;

    private void Awake()
    {
        userDetectionController.UserJoined += UserJoinedHandler;
        userDetectionController.UserLeft += UserLeftHandler;
        StartCoroutine(InitialDisableRoutine());
    }

    private IEnumerator InitialDisableRoutine()
    {
        yield return null;
        hat.gameObject.SetActive(false);
        foreach (var simulatedCloth in simulatedCloths)
        {
            simulatedCloth.SetActive(false);
        }
        Deselect();
    }

    private void OnEnable()
    {
        canvasLayer.SetActive(true);
        settingsLayer.SetActive(false);
    }

    private void OnDisable()
    {
        if (canvasLayer != null)
        {
            canvasLayer.SetActive(false);
        }

        if (settingsLayer != null)
        {
            settingsLayer.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (userDetectionController != null)
        {
            userDetectionController.UserLeft -= UserLeftHandler;
            userDetectionController.UserJoined -= UserJoinedHandler;
        }
    }

    private void UserLeftHandler()
    {
        cursor.Hide();
        hat.gameObject.SetActive(false);
        foreach (var simulatedCloth in simulatedCloths)
        {
            simulatedCloth.SetActive(false);
        }
        Deselect();
    }

    private void UserJoinedHandler()
    {
        cursor.Show();
        hat.gameObject.SetActive(false);
        gameObject.SetActive(true);
        foreach (var simulatedCloth in simulatedCloths)
        {
            simulatedCloth.SetActive(false);
        }
        Deselect();
    }

    public void SelectMaleRenderer()
    {
        if (userDetectionController.NumUsers == 0)
        {
            return;
        }

        var materials = canvasModels[0].GetComponentsInChildren<Renderer>().SelectMany(r => r.materials);
        foreach (var material in materials)
        {
            material.SetColor("_Color",Color.white);
        }
    }
    
    public void SelectFemaleRenderer()
    {
        if (userDetectionController.NumUsers == 0)
        {
            return;
        }
        var materials = canvasModels[1].GetComponentsInChildren<Renderer>().SelectMany(r => r.materials);
        foreach (var material in materials)
        {
            material.SetColor("_Color",Color.white);
        }
    }

    public void ClickMaleRenderer()
    {
        if (userDetectionController.NumUsers == 0)
        {
            return;
        }
        cursor.Hide();
        var selectedIndex = 0;
        gameObject.SetActive(false);
        for (var i = 0; i < simulatedCloths.Count; i++)
        {
            var simulatedCloth = simulatedCloths[i];
            simulatedCloth.SetActive(selectedIndex == i);
            hat.gameObject.SetActive(selectedIndex == 1);
        }
    }
    
    public void ClickFemaleRenderer()
    {
        if (userDetectionController.NumUsers == 0)
        {
            return;
        }
        cursor.Hide();
        var selectedIndex = 1;
        gameObject.SetActive(false);
        for (var i = 0; i < simulatedCloths.Count; i++)
        {
            var simulatedCloth = simulatedCloths[i];
            simulatedCloth.SetActive(selectedIndex == i);
            hat.gameObject.SetActive(selectedIndex == 1);
        }
    }

    public void Deselect()
    {
        var materials = canvasModels.SelectMany(m=>m.GetComponentsInChildren<Renderer>().SelectMany(r => r.materials));
        foreach (var material in materials)
        {
            material.SetColor("_Color", new Color(0.4f,0.4f,0.4f));
        }
    }

    [Serializable]
    public class RenderersCollection
    {
        public List<Renderer> renderersList;
    }
}
