﻿using UnityEngine;
using UnityEngine.UI;
using PQ.Common.Extensions;


namespace PQ.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI subtitle = default;
        
        [Header("Scene to load on game start")]
        [SerializeField] private string sceneName = default;
        
        [Header("Menu Core Components")]
        [SerializeField] private GameObject              buttonPanel             = default;
        [SerializeField] private MainMenuPanelController mainMenuPanelController = default;
        
        [Header("Menu Buttons")]
        [SerializeField] private Button startButton    = default;
        [SerializeField] private Button settingsButton = default;
        [SerializeField] private Button aboutButton    = default;
        [SerializeField] private Button quitButton     = default;
        
        void Awake()
        {
            if (!SceneExtensions.IsSceneAbleToLoad(sceneName))
            {
                Debug.LogError($"Scene cannot be loaded, perhaps `{sceneName}` is misspelled?");
            }
            mainMenuPanelController.SetActionOnStartPressed(() => LoadGame());
            mainMenuPanelController.SetActionOnPanelOpen(()    => ToggleMainMenuVisibility(false));
            mainMenuPanelController.SetActionOnPanelClose(()   => ToggleMainMenuVisibility(true));

            #if UNITY_WEBGL
                UiExtensions.SetButtonActiveAndEnabled(quitButton, false);
            #endif
        }

        void OnEnable()
        {
            startButton   .onClick.AddListener(mainMenuPanelController.OpenStartPanel);
            settingsButton.onClick.AddListener(mainMenuPanelController.OpenSettingsPanel);
            aboutButton   .onClick.AddListener(mainMenuPanelController.OpenAboutPanel);
            quitButton    .onClick.AddListener(SceneExtensions.QuitGame);
        }
        void OnDisable()
        {
            startButton   .onClick.RemoveListener(mainMenuPanelController.OpenStartPanel);
            settingsButton.onClick.RemoveListener(mainMenuPanelController.OpenSettingsPanel);
            aboutButton   .onClick.RemoveListener(mainMenuPanelController.OpenAboutPanel);
            quitButton    .onClick.RemoveListener(SceneExtensions.QuitGame);
        }

        private void LoadGame()
        {
            SceneExtensions.LoadScene(sceneName, () =>
            {
                GameEventCenter.startNewGame.Trigger(mainMenuPanelController.GetGameSettings());
            });
        }

        private void ToggleMainMenuVisibility(bool isVisible)
        {
            UiExtensions.SetLabelVisibility(subtitle, isVisible);
            buttonPanel.SetActive(isVisible);
            #if UNITY_WEBGL
                UiExtensions.SetButtonActiveAndEnabled(quitButton, false);
            #endif
        }
    }
}
