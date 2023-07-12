using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Internals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmniGlyph.UI {
    public class UIContext : OmniMonoComponent {
        private Color _defaultColor = Color.white;
        private Color _actionColor = Color.yellow;

        [SerializeField]
        private Canvas _worldSpaceCanvas;
        [SerializeField]
        private Canvas _screenSpaceCanvas;

        private List<UIElement> _worldUiElements = new List<UIElement>();

        public Color DefaultColor { get { return _defaultColor; } }
        public Color ActionColor { get { return _actionColor; } }

        private void Awake() {
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

        public TextMeshProUGUI CreateWorldText(TextProperties props) {

            GameObject txtObj = new GameObject(props.Text);
            TextMeshProUGUI text = txtObj.AddComponent<TextMeshProUGUI>();

            txtObj.transform.SetParent(_worldSpaceCanvas.transform, true);

            text.fontSize = props.FontSize;
            text.color = props.Color;
            text.text = props.Text;
            text.alignment = props.Alignment;

            text.rectTransform.localScale = Vector3.one;

            return text;
        }
        public RawImage CreateWorldImage(Texture2D image) {
            GameObject imgObj = new GameObject(image.name);
            imgObj.transform.SetParent(_worldSpaceCanvas.transform, true);

            RawImage newImage = imgObj.AddComponent<RawImage>();
            newImage.texture = image;
            newImage.rectTransform.localScale = Vector3.one;

            return newImage;
        }
        public string ColorizeText(string text, Color color) {
            byte ToByte(float value) {
                return (byte)(Mathf.Clamp01(value) * 255);
            }
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
        }
        public TextMeshProUGUI ApplyTextProps(TextMeshProUGUI textObject, TextProperties textProps) {
            textObject.text = textProps.Text;
            textObject.color = textProps.Color;
            textObject.fontSize = textProps.FontSize;
            textObject.alignment = textProps.Alignment;
            return textObject;
        }
        private void Update() {
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
