using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGlyph.Internals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmniGlyph.UI {
    public abstract class OmniMonoUI : OmniMonoInstance {
        [SerializeField]
        private TextMeshProUGUI _overheadText;
        [SerializeField]
        private RawImage _overheadImage;

        private TextProperties _defaultTextProps = new TextProperties() { };
        private Texture2D _defaultOverheadImage;

        protected virtual void Start() {
            Init();
        }
        public override void Init() {
            base.Init();
            _defaultOverheadImage = new Texture2D(0, 0) { name = $"{gameObject.name} - DefaultOverheadImage" };


            if (_overheadText == null) {
                _overheadText = Context.UIContext.CreateWorldText(_defaultTextProps);
            }
            if (_overheadImage == null) {
                _overheadImage = Context.UIContext.CreateWorldImage(_defaultOverheadImage);
                _overheadImage.rectTransform.localScale *= 0.5f;
            }
        }

        protected void ShowOverheadText(string text) {
            ShowOverheadText(new TextProperties() {
                Text = text,
                Color = Context.UIContext.DefaultColor,
                FontSize = 24,
                Alignment = TextAlignmentOptions.Center
            });
        }
        protected void ShowOverheadText(TextProperties props) {
            if (IsOverheadTextAlreadyDisplayed(props)) {
                return;
            }
            if (IsOverheadTextSet()) {
                HideOverheadText();
            }

            _overheadText = Context.UIContext.ApplyTextProps(_overheadText, props);
        }
        protected void HideOverheadText() {
            if (_overheadText != null) {
                _overheadText = Context.UIContext.ApplyTextProps(_overheadText, _defaultTextProps);
            }
        }
        protected void ShowOverheadImage(Texture2D image) {
            if (IsOverheadImageAlreadyDisplayed(image)) {
                return;
            }
            if (IsOverheadImageSet()) {
                HideOverheadImage();
            }
            _overheadImage.texture = image;

        }


        protected void HideOverheadImage() {
            if (_overheadImage != null) {
                _overheadImage.texture = _defaultOverheadImage;
            }
        }

        protected virtual void Update() {
            UpdateUI();
        }

        private void UpdateUI() {
            UpdateOverheadUIElement(_overheadText, 2f * transform.lossyScale.y * Vector3.up);
            UpdateOverheadUIElement(_overheadImage, 4f * transform.lossyScale.y * Vector3.up);
        }
        private void UpdateOverheadUIElement<T>(T ui, Vector3 verticalOffset) where T : MaskableGraphic {
            if (ui != null) {
                ui.transform.position = transform.position + verticalOffset;
                ui.transform.LookAt(new Vector3(
                    ui.transform.transform.position.x,
                    Context.PlayerCamera.transform.position.y,
                    Context.PlayerCamera.transform.position.z
                    ));
            }
        }
        private bool IsOverheadTextAlreadyDisplayed(TextProperties props) {
            return _overheadText != null &&
                _overheadText.text == props.Text &&
                _overheadText.color == props.Color &&
                _overheadText.fontSize == props.FontSize &&
                _overheadText.alignment == props.Alignment;
        }
        private bool IsOverheadTextSet() {
            return IsOverheadTextAlreadyDisplayed(_defaultTextProps);
        }
        private bool IsOverheadImageAlreadyDisplayed(Texture2D image) {
            return _overheadImage != null &&
                _overheadImage.texture == image;
        }
        private bool IsOverheadImageSet() {
            return IsOverheadImageAlreadyDisplayed(_defaultOverheadImage);
        }
    }
}
