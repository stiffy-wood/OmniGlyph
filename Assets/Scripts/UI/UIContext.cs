using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Internals;
using TMPro;
using UnityEngine;

namespace OmniGlyph.UI {
    public class UIContext : OmniMono {
        private Color _defaultColor = Color.white;
        private Color _actionColor = Color.yellow;

        private record TextProperties {
            public string Text { get; set; }
            public Color Color { get; set; }
            public int FontSize { get; set; }
            public TextAlignmentOptions Alignment { get; set; }
            public TextProperties() {

            }
        }
        public record UIElement {
            public GameObject ParentObject { get; set; }
            public GameObject TextObject { get; set; }
            public Vector3 Offset { get; set; }
            public bool ShouldDispose { get; set; }
            public UIElement() {
            }
            public void Dispose() {
                ShouldDispose = true;
            }
        }
        [SerializeField]
        private Canvas _worldSpaceCanvas;
        [SerializeField]
        private Canvas _screenSpaceCanvas;

        private List<UIElement> _worldUiElements = new List<UIElement>();

        private void Start() {
            if (!(_worldSpaceCanvas == null || _screenSpaceCanvas == null)) {
                return;
            }
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (Canvas canvas in canvases) {
                if (canvas.renderMode == RenderMode.WorldSpace && _worldSpaceCanvas == null) {
                    _worldSpaceCanvas = canvas;
                } else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay && _screenSpaceCanvas == null) {
                    _screenSpaceCanvas = canvas;
                }
            }
        }
        public override void Init(GameContext context) {
            base.Init(context);
            Context.GameStateChanged += OnGameStateChange;
        }

        private void OnGameStateChange(GameStates newState) {
            HideInteractOption();
        }
        private bool CanExecute(GameStates requiredState) {
            return Context.CurrentGameState == requiredState;
        }
        private bool CanExecute(GameStates[] requiredStates) {
            return requiredStates.Contains(Context.CurrentGameState);
        }
        private TextMeshProUGUI CreateWorldText(TextProperties props) {

            GameObject txtObj = new GameObject("World_Text");
            TextMeshProUGUI text = txtObj.AddComponent<TextMeshProUGUI>();

            txtObj.transform.SetParent(_worldSpaceCanvas.transform, true);

            text.fontSize = props.FontSize;
            text.color = props.Color;
            text.text = props.Text;
            text.alignment = props.Alignment;

            text.rectTransform.localScale = Vector3.one;

            return text;
        }
        private string ColorizeText(string text, Color color) {
            byte ToByte(float value) {
                return (byte)(Mathf.Clamp01(value) * 255);
            }
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
        }

        #region Interact
        private TextMeshProUGUI _interactText;
        public void ShowInteractOption() {
            if (!CanExecute(GameStates.Roam)) {
                HideInteractOption();
                return;
            } else if (_interactText != null) {
                return;
            }
            _interactText = CreateWorldText(new TextProperties() {
                Text = $"Press [{ColorizeText("E", _actionColor)}] to interact",
                Alignment = TextAlignmentOptions.Center,
                FontSize = 16,
                Color = _defaultColor
            });
            return;
        }
        public void HideInteractOption() {
            if (_interactText != null) {
                Destroy(_interactText.gameObject);
                _interactText = null;
            }
        }
        #endregion
        private void Update() {
            if (_interactText != null) {
                _interactText.gameObject.transform.position = Context.Player.transform.position + Context.Player.Size.y * Vector3.up;
            }
            for (int i = 0; i < _worldUiElements.Count; i++) {
                UIElement element = _worldUiElements[i];
                if (element.ShouldDispose) {
                    Destroy(element.TextObject);
                    _worldUiElements.Remove(element);
                    i--;
                } else {
                    element.TextObject.transform.position = element.ParentObject.transform.position + element.Offset;
                }
            }
        }
    }
}
