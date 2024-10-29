using System;
using UnityEngine;

namespace Managers
{
  public class PanelManager : MonoBehaviour
  {
    public static PanelManager Instance;
    
    private GameManager _gameManager;
    
    public GameObject InGamePanel;

    public GameObject WinPanel;
    
    public GameObject LosePanel;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start()
    {
      _gameManager = GameManager.Instance;
      _gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
      switch (_gameManager.GameState)
      {
        case GameState.Game:
          CloseAllPanels();
          InGamePanel.SetActive(true);
          break;
        case GameState.Win:
          CloseAllPanels();
          WinPanel.SetActive(true);
          break;
        case GameState.Lose:
          CloseAllPanels();
          LosePanel.SetActive(true);
          break;
      }
    }

    private void CloseAllPanels()
    {
      InGamePanel.SetActive(false);
      WinPanel.SetActive(false);
      LosePanel.SetActive(false);
    }
  }
}