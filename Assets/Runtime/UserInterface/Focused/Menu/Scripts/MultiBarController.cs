using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FiveSQD.WebVerse.Input.Focused
{
    public class MultiBarController : BaseController
    {
        public TMP_InputField inputField;

        public Button prevButton;

        public Button nextButton;

        private bool isSelected = false;

        private Stack<string> prevURLs;

        private Stack<string> nextURLs;

        public override void Initialize()
        {
            base.Initialize();
            prevURLs = new Stack<string>();
            nextURLs = new Stack<string>();
            UpdateNavButtons();
        }

        public void Selected()
        {
            isSelected = true;
        }

        public void Deselected()
        {
            isSelected = false;
        }

        public void Enter(InputAction.CallbackContext context)
        {
            if (isSelected && context.phase == InputActionPhase.Started && !string.IsNullOrEmpty(inputField.text))
            {
                prevURLs.Push(WebVerseRuntime.Instance.currentURL);
                LoadURL(inputField.text);
                UpdateNavButtons();
            }
        }

        public void GoNext()
        {
            if (nextURLs.Count > 0)
            {
                prevURLs.Push(WebVerseRuntime.Instance.currentURL);
                LoadURL(nextURLs.Pop());
                UpdateNavButtons();
            }
        }

        public void GoBack()
        {
            if (prevURLs.Count > 0)
            {
                nextURLs.Push(WebVerseRuntime.Instance.currentURL);
                LoadURL(prevURLs.Pop());
                UpdateNavButtons();
            }
        }

        private void LoadURL(string url)
        {
            WebVerseRuntime.Instance.LoadURL(url);
        }

        private void UpdateNavButtons()
        {
            if (prevURLs.Count > 0)
            {
                prevButton.interactable = true;
            }
            else
            {
                prevButton.interactable = false;
            }

            if (nextURLs.Count > 0)
            {
                nextButton.interactable = true;
            }
            else
            {
                nextButton.interactable = false;
            }
        }
    }
}