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
    [SerializeField] private StartScreen startScreen;

    private bool _userPresent;
    
    private void Awake()
    {
        userDetectionController.UserJoined += UserJoinedHandler;
        userDetectionController.UserLeft += UserLeftHandler;
        Observable.NextFrame().Subscribe(_ =>
        {
            hat.gameObject.SetActive(false);
            foreach (var simulatedCloth in simulatedCloths)
            {
                simulatedCloth.SetActive(false);
            }
            Deselect();
        });
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
        _userPresent = false;
        hat.gameObject.SetActive(false);
        startScreen.FadeIn();
        foreach (var simulatedCloth in simulatedCloths)
        {
            simulatedCloth.SetActive(false);
        }
        Deselect();
    }

    private void UserJoinedHandler()
    {
        cursor.Show();
        _userPresent = true;
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
        if (!_userPresent)
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
        if (!_userPresent)
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
        if (!_userPresent)
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
        if (!_userPresent)
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
