using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Actors;
using UnityEngine;

namespace OmniGlyph.Actors {
    public class PlayerFollowLine {
        private List<Vector3> _points = new List<Vector3>();
        private List<BotActor> _followers = new List<BotActor>();
        private float _frequency;
        private float _spacing;
        private int _maxFollowers;
        private int _maxPoints;
        private int _indexStep;
        public PlayerFollowLine() {
            _frequency = 0.1f;
            _spacing = 2.5f;
            _maxFollowers = 10;
            _maxPoints = Mathf.RoundToInt((1 / _frequency) * _spacing * _maxFollowers);
            _indexStep = Mathf.RoundToInt(_spacing * (1 / _frequency));
        }
        public void AddPoint(Vector3 point) {
            if (_points.Count > 0 && Vector3.Distance(_points[_points.Count - 1], point) < _frequency) {
                return;
            }
            _points.Add(point);
            if (_points.Count > _maxPoints) {
                _points.RemoveAt(0);
            }

            int index = _indexStep;
            foreach (BotActor follower in _followers) {
                if (_points.Count - index <= 0) {
                    break;
                }
                follower.SetPosition(_points[_points.Count - index]);
                index += _indexStep;

            }

        }
        public void AddFollower(BotActor follower) {
            _followers.Add(follower);
        }
        public void RemoveFollower(BotActor follower) {
            _followers.Remove(follower);
        }
    }
}
