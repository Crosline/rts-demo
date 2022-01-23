using _Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    [SerializeField]
    private InputHandler _inputHandler;

    void Start() {
        if (_inputHandler == null) {
            _inputHandler = FindObjectOfType<InputHandler>();
        }

        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                Init();
                break;
            case GameState.Selecting:
                Selecting();
                break;
            case GameState.ExecutingInput:
                ExecutingInput();
                break;
            case GameState.Restart:
                Restart();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New State: {newState}");
    }
    #region State Functions

    private void Init() {

        GridManager.Instance.GridSetup();

        ChangeState(GameState.Selecting);
    }

    private void Selecting() {

        if (!_inputHandler.isActive)
        _inputHandler.isActive = true;
        if (_inputHandler.isGridObjectSelected)
        _inputHandler.isGridObjectSelected = false;

    }

    private void ExecutingInput() {

        if (!_inputHandler.isGridObjectSelected)
            _inputHandler.isGridObjectSelected = true;

    }
    private void Restart() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region Button Functions

    public void RestartGame() {
        ChangeState(GameState.Restart);
    }

    #endregion

    [Serializable]
    public enum GameState {
        Starting = 0, // Initializing the grid etc.
        Selecting = 1, // Waiting Input from Procution Menu(BuildProduct) / Game Board(ProcutionSelect)
        ExecutingInput = 2, // Started to execute inputs
        Restart = 3, //
    }
}
