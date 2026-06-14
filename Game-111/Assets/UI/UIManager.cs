using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Button restartButton,exitButton,escapeButton,hubButton;
    private List<Button> buttonList = new List<Button>();
    private VisualElement goalPrompt,screenFade;
    private Label goalMessage;

    public static Action FadeOutComplete = delegate { };
    public static Action FadeInComplete = delegate { };
    public static Action<int> LoadScene = delegate { };

    private bool bFadeInComplete, bFadeOutComplete,bResetSequence;
    private PlayerInput pInput;
    private int buttonIndex;
   
    private void Awake()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        pInput.Player.Escape.performed += ShowMenu;
    }
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        restartButton = root.Q<Button>("Restart-Button");
        exitButton = root.Q<Button>("Exit-Button");
        escapeButton = root.Q<Button>("Escape-Button");
        hubButton = root.Q<Button>("Hub-Button");

        goalPrompt = root.Q<VisualElement>("GoalPrompt");
        goalMessage = root.Q<Label>("GoalSign");
        screenFade = root.Q<VisualElement>("screenFade");

        restartButton.SetEnabled(true);
        exitButton.SetEnabled(true);
        escapeButton.SetEnabled(true);
        hubButton.SetEnabled(true);

        restartButton.clicked += Restart;
        exitButton.clicked += Exit;
        escapeButton.clicked += Return;
        hubButton.clicked += ReturnToHub;       
        
        WinGA.Goal += Goal;
        SceneLoader.HideGoalScreen += Restart;

        //navigation       
    }
    private void OnDisable()
    {
        WinGA.Goal -= Goal;
        restartButton.clicked -= Restart;
        exitButton.clicked -= Exit;
        escapeButton.clicked -= Return;
        hubButton.clicked -= ReturnToHub;

        pInput.Disable();
        pInput.Player.Escape.performed -= ShowMenu;
        SceneLoader.HideGoalScreen -= Restart;
    }
    private void Restart()
    {
        StartCoroutine(nameof(ResetSequence));        
    }
    private void Exit()
    {
        Application.Quit();
    }
    private void Return()
    {
        screenFade.style.display = DisplayStyle.None;
        CuboidMaster.EnablePlayerMovement();
    }
    private void ReturnToHub()
    {
        LoadScene(0);
    }
    private void ShowMenu(InputAction.CallbackContext c)
    {       
        if (screenFade.style.display != DisplayStyle.Flex)
        {
            ContextMenu();
            screenFade.style.opacity = 1;
            screenFade.style.backgroundColor = Color.clear;
            screenFade.style.display = DisplayStyle.Flex;
            goalPrompt.style.display = DisplayStyle.Flex;
            escapeButton.style.display = DisplayStyle.Flex;
            CuboidMaster.DisablePlayerMovement();
        }
        else
        {
            screenFade.style.display = DisplayStyle.None;
            CuboidMaster.EnablePlayerMovement();
        }
    }
    private void ContextMenu()
    {
        buttonList.Clear();
        restartButton.style.display = DisplayStyle.Flex;
        buttonList.Add(restartButton);       
        escapeButton.style.display = DisplayStyle.Flex;
        buttonList.Add(escapeButton);
        exitButton.style.display = DisplayStyle.None;
        restartButton.Focus();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            hubButton.style.display = DisplayStyle.Flex;
            buttonList.Add(hubButton);
        }
        else
            hubButton.style.display = DisplayStyle.None;
    }
    private void Goal()
    {
        screenFade.style.opacity = 1;
        screenFade.style.backgroundColor = Color.clear;
        screenFade.style.display = DisplayStyle.Flex;
        goalPrompt.style.display = DisplayStyle.None;
        goalMessage.style.display = DisplayStyle.Flex;
    }
    private void NavigateUI(InputAction.CallbackContext c)
    {
        if(pInput.UI.Navigation.ReadValue<Vector2>().y > 0) 
        {
            buttonIndex++;
            buttonIndex = Mathf.Clamp(buttonIndex, 0, buttonList.Count - 1);
            buttonList[buttonIndex].Focus();
        }
        else
        {
            buttonIndex--;
            buttonIndex = Mathf.Clamp(buttonIndex, 0, buttonList.Count - 1);
            buttonList[buttonIndex].Focus();
        }
    }
    IEnumerator FadeIn()
    {
        bFadeInComplete = false;
        screenFade.style.opacity = 1;
        screenFade.style.backgroundColor = Color.black;
        screenFade.style.display = DisplayStyle.Flex;

        float fadeRate = 0;
        while (fadeRate < 1)
        {
            fadeRate += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
            screenFade.style.opacity = Mathf.Lerp(1,0,fadeRate);
        }
        screenFade.style.display = DisplayStyle.None;
        FadeInComplete();
        bFadeInComplete = true;
    }
    IEnumerator FadeOut()
    {
        bFadeOutComplete = false;
        screenFade.style.opacity = 0;
        screenFade.style.backgroundColor = Color.black;
        goalPrompt.style.display = DisplayStyle.None;
        goalMessage.style.display = DisplayStyle.None;
        screenFade.style.display = DisplayStyle.Flex;
        float fadeRate = 0;

        while (fadeRate < 1)
        {
            fadeRate += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
            screenFade.style.opacity = Mathf.Lerp(0,1, fadeRate);
        }
        FadeOutComplete();
        bFadeOutComplete = true;
    }
    IEnumerator ResetSequence()
    {        
        StartCoroutine(nameof(FadeOut));        
        while (!bFadeOutComplete)
            yield return new WaitForEndOfFrame();  
        CuboidMaster.ResetGame();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(nameof(FadeIn));
        while (!bFadeInComplete)
            yield return new WaitForEndOfFrame();
    }
}
