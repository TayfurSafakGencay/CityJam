using System;
using UnityEngine;

namespace Managers
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    public Action OnGameStateChanged;
    
    private GameState _gameState;
    
    public GameState GameState
    {
      get => _gameState;
      set
      {
        _gameState = value;
        OnGameStateChanged?.Invoke();
      }
    }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start()
    {
      ChangeGameState(GameState.Game);
    }

    public void ChangeGameState(GameState gameState)
    {
      GameState = gameState;
    }
  }

  public enum GameState
  {
    Game,
    Lose,
    Win,
  }
}