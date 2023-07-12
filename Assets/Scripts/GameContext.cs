using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Actors;
using OmniGlyph.Cam;
using OmniGlyph.Combat;
using OmniGlyph.Console;
using OmniGlyph.Control;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace OmniGlyph {
    public class GameContext : OmniMono {
        [SerializeField]
        private PlayerActor _player;
        [SerializeField]
        private PlayerCamera _playerCamera;
        [SerializeField]
        private InputManager _inputManager;
        [SerializeField]
        private GameStates _currentGameState = GameStates.Roam;
        [SerializeField]
        private GameStateContext _stateContext;
        [SerializeField]
        private CombatContext _combatContext;
        [SerializeField]
        private UIContext _uiContext;
        [SerializeField]
        private StoryContext _storyContext;
        [SerializeField]
        private DevConsoleController _devConsole;

        public PlayerActor Player => _player;
        public PlayerCamera PlayerCamera => _playerCamera;
        public InputManager InputManager => _inputManager;
        public CombatContext CombatContext => _combatContext;
        public UIContext UIContext => _uiContext;
        public StoryContext StoryContext => _storyContext;
        public GameStates CurrentGameState {
            get {
                return _currentGameState;
            }
            set {
                if (!IsGameStateEqual(value, _currentGameState)) {
                    _currentGameState = value;
                    GameStateChanged?.Invoke(value);
                }
            }
        }
        public GameStateContext StateContext => _stateContext;
        public bool isInit = false;
        public event Action<GameStates> GameStateChanged;
        void Awake() {
            OnGameStateChange(_currentGameState);
            GameStateChanged += OnGameStateChange;
            _uiContext = InitComponent<UIContext>();
            _inputManager = InitComponent<InputManager>();
            _combatContext = InitComponent<CombatContext>();
            _storyContext = InitComponent<StoryContext>();
            _devConsole = InitComponent<DevConsoleController>();

            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerActor>();
            _playerCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<PlayerCamera>();

            if (_player == null) {
                Debugger.ThrowCriticalError("Could not find PlayerActor");
            }
            if (_playerCamera == null) {
                Debugger.ThrowCriticalError("Could not find PlayerCamera");
            }

            isInit = true;
        }

        private void OnGameStateChange(GameStates newState) {
            _stateContext = Resources.Load<GameStateContext>($"Contexts/GameStates/{newState}GameStateContext");
            if (_stateContext == null) {
                Debugger.ThrowCriticalError($"Could not load GameState for {newState}");
            }
            Debugger.Log($"Game State Changed to {newState}");
        }
        private T InitComponent<T>() where T : OmniMonoComponent {
            return InitComponent<T>(gameObject);
        }
        private T InitComponent<T>(GameObject targetObject) where T : OmniMonoComponent {
            T comp = targetObject.GetComponent<T>();
            if (comp == null) {
                comp = targetObject.AddComponent<T>();
            }
            comp.Init(this);
            return comp;
        }

        public Actor[] GetActorsInScene() {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_playerCamera.Cam);

            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            if (allObjects.Length == 0) {
                return new Actor[0];
            }

            List<Actor> actors = new List<Actor>();
            for (int i = 0; i < allObjects.Length; i++) {
                Actor actor = allObjects[i].GetComponent<Actor>();
                if (actor == null) {
                    continue;
                }

                Renderer renderer = allObjects[i].GetComponent<Renderer>();
                if (renderer == null) {
                    continue;
                }

                if (!GeometryUtility.TestPlanesAABB(planes, renderer.bounds)) {
                    Debug.Log("Renderer " + renderer.name + " is not visible!");
                    continue;
                }

                actors.Add(actor);
            }
            return actors.ToArray();
        }
        public static bool HasGameState(GameStates a, GameStates b) {
            return ((byte)a & (byte)b) > 0;
        }
        public static bool IsGameStateEqual(GameStates a, GameStates b) {
            return a == b;
        }

    }
}
