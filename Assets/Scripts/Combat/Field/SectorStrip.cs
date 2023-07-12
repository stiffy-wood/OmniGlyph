using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Actors;
using OmniGlyph.Control;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace OmniGlyph.Combat.Field {
    public class SectorStrip : OmniMonoInstance {
        private Actor _actorLeft;
        private Actor _actorRight;

        private Vector3 _center;
        private Vector3 left { get { return transform.position + Vector3.left * (transform.lossyScale.x / 4); } }
        private Vector3 right { get { return transform.position + Vector3.right * (transform.lossyScale.x / 4); } }
        private BoxCollider _collider;
        public Vector3 Center => _center;
        public Sector ParentSector {
            get {
                return GetComponentInParent<Sector>();
            }
        }
        private void Start() {
            _collider = gameObject.AddComponent<BoxCollider>();
            _collider.isTrigger = true;

            Debugger.Watch(
                transform.position - Vector3.up * transform.lossyScale.y,
                new DebugObjectProperties(
                        transform.lossyScale * 0.85f,
                        Color.cyan,
                        PrimitiveType.Cube)
                );

            Init();

            //transform.position = transform.position + Vector3.down * (transform.lossyScale.y / 4f);
        }

        public void SetActor(Side side, Actor actor) {
            if (side == Side.Left) {
                _actorLeft = actor;
                actor.SetPosition(left);
                //actor.SetRotation(Quaternion.Euler(Vector3.right));
            } else {
                _actorRight = actor;
                actor.SetPosition(right);
                //actor.SetRotation(Quaternion.Euler(Vector3.left));
            }
        }
        public Maybe<Actor> GetActor(Side side) {
            if (side == Side.Middle) {
                return Maybe<Actor>.None();
            }
            Actor actor = side == Side.Left ? _actorLeft : _actorRight;
            if (actor == null) {
                return Maybe<Actor>.None();
            }
            return Maybe<Actor>.Some(actor);
        }
        public bool HasActor(Side side) {
            return GetActor(side).HasValue;
        }
        public bool HasActor(Actor actor) {
            return _actorLeft == actor || _actorRight == actor;
        }
        public void RemoveActor(Actor actor) {
            if (_actorLeft == actor) {
                _actorLeft = null;
            } else if (_actorRight == actor) {
                _actorRight = null;
            }
        }
        public bool IsEmpty() {
            return _actorLeft == null && _actorRight == null;
        }

        private void OnMouseOver() {
            if (Context.InputManager.GetMouseButtonUp(MouseButtons.Left)) {
                Context.CombatContext.OnStripSelect(this);
            }
        }

    }
}
