﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rimaethon.Scripts.Core.Enums;
using Rimaethon.Scripts.Managers;
using Rimaethon.Scripts.UI.UIElements;
using Rimaethon.Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rimaethon.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public List<UIPage> pages;

        public int currentPage;
        public int defaultPage;
        public int pausePageIndex = 1;
        public bool allowPause = true;


        [HideInInspector] public EventSystem eventSystem;
        private bool _isPaused;
        private List<UIElement> _uIelements;
        
        private void Awake()
        {
            EventManager.Instance.AddHandler(GameEvents.OnTogglePause, TogglePause);
        }
        
        private void Start()
        {
            SetUpEventSystem();
            SetUpUIElements();
            InitializeFirstPage();
            UpdateUI();
        }
        private void OnEnable()
        {
            SetupGameManagerUIManager();
        }

        public void CreateBackEffect()
        {
            
            AudioManager.Instance.PlaySFX(0);
        }

        public void CreateClickEffect()
        {
            AudioManager.Instance.PlaySFX(1);

        }

        public void CreateNavigationEffect()
        {
            AudioManager.Instance.PlaySFX(2);

        }

        private void SetupGameManagerUIManager()
        {
            if (GameManager.Instance != null && GameManager.Instance.uiManager == null)
                GameManager.Instance.uiManager = this;
        }


        private void SetUpUIElements()
        {
            _uIelements = FindObjectsOfType<UIElement>().ToList();
        }


        private void SetUpEventSystem()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
                Debug.LogWarning("There is no event system in the scene but you are trying to use the UIManager. /n" +
                                 "All UI in Unity requires an Event System to run. /n" +
                                 "You can add one by right clicking in hierarchy then selecting UI->EventSystem.");
        }

        public void TogglePause()
        {
            if (!allowPause) return;
            if (_isPaused)
            {
                GoToPage(defaultPage);
                Time.timeScale = 1;
                _isPaused = false;
                Debug.Log("Unpaused");
            }
            else
            {
                GoToPage(pausePageIndex);
                Time.timeScale = 0;
                _isPaused = true;
                Debug.Log("Paused");
            }
        }


        public void UpdateUI()
        {
            foreach (var uiElement in _uIelements) uiElement.UpdateUI();
        }


        private void InitializeFirstPage()
        {
            GoToPage(defaultPage);
        }


       


        public void GoToPage(int pageIndex)
        {
            if (pageIndex < pages.Count && pages[pageIndex] != null)
            {
                SetActiveAllPages(false);
                pages[pageIndex].gameObject.SetActive(true);
                pages[pageIndex].SetSelectedUIToDefault();
            }
        }


        public void GoToPageByName(string pageName)
        {
            var page = pages.Find(item => item.name == pageName);
            var pageIndex = pages.IndexOf(page);
            GoToPage(pageIndex);
        }


        private void SetActiveAllPages(bool activated)
        {
            if (pages == null) return;
            foreach (var page in pages.Where(page => page != null)) page.gameObject.SetActive(activated);
        }
        public void GıveTestDebug()
        {
            Debug.Log("Test");
        }
    }
}