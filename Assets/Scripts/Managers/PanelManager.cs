using Panels;
using UnityEngine;

namespace Managers
{
  public class PanelManager : MonoBehaviour
  {
    public static PanelManager Instance;
    
    private GameManager _gameManager;
    
    public InGamePanel InGamePanel;

    public WinPanel WinPanel;
    
    public LosePanel LosePanel;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start()
    {
      rectTransform = GetComponent<RectTransform>();
      
      _gameManager = GameManager.Instance;
      _gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
      switch (_gameManager.GameState)
      {
        case GameState.Game:
          CloseAllPanels();
          InGamePanel.gameObject.SetActive(true);
          break;
        case GameState.Win:
          CloseAllPanels();
          WinPanel.gameObject.SetActive(true);
          break;
        case GameState.Lose:
          CloseAllPanels();
          LosePanel.gameObject.SetActive(true);
          break;
      }
    }

    private void CloseAllPanels()
    {
      InGamePanel.gameObject.SetActive(false);
      WinPanel.gameObject.SetActive(false);
      LosePanel.gameObject.SetActive(false);
    }

    private RectTransform rectTransform;

    private void ApplySafeArea()
    {
      Rect safeArea = Screen.safeArea;
      Vector2 anchorMin = safeArea.position;
      Vector2 anchorMax = safeArea.position + safeArea.size;

      anchorMin.x /= Screen.width;
      anchorMin.y /= Screen.height;
      anchorMax.x /= Screen.width;
      anchorMax.y /= Screen.height;

      rectTransform.anchorMin = anchorMin;
      rectTransform.anchorMax = anchorMax;
      
      InGamePanel.GetComponent<RectTransform>().anchorMin = anchorMin;
      InGamePanel.GetComponent<RectTransform>().anchorMax = anchorMax;
    }
  }
}