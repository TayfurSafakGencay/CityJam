using System.Collections.Generic;
using Enum;
using Managers;
using TMPro;
using UnityEngine;

namespace Panels
{
  public class InGamePanel : MonoBehaviour
  {
    public TextMeshProUGUI TimerText;
    
    private float _timer;
    
    private bool _isTimerActive;

    private void OnEnable()
    {
      _timer = 180;
      _isTimerActive = true;
      
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    
    private void OnDisable()
    {
      GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
      if (GameManager.Instance.GameState == GameState.Game)
      {
        CloseAllTargets();
        SetAllTargets();
      }
    }

    private void Update()
    {
      if (!_isTimerActive) return;
      
      _timer -= Time.deltaTime;
      TimerText.text = _timer.ToString("F0");

      if (_timer > 0) return;
      _isTimerActive = false;
      LevelManager.Lose();
    }

    [SerializeField]
    private List<TargetItem> _targets;
    
    private void CloseAllTargets()
    {
      foreach (TargetItem target in _targets)
      {
        target.gameObject.SetActive(false);
      }
    }

    private void SetAllTargets()
    {
      List<TargetDataVo> targetsData = LevelManager.Instance.GetTargetsData();

      for (int i = 0; i < targetsData.Count; i++)
      {
        if (i > _targets.Count)
        {
          return;
        }
        TargetDataVo item = targetsData[i];

        _targets[i].SetInitialValues(item.Image, item.Count, item.CollectableObjectKey);  
        _targets[i].gameObject.SetActive(true);
      }
    }

    public void Clicked(CollectableObjectKey collectableObjectKey)
    {
      foreach (TargetItem target in _targets)
      {
        if (target.Key != collectableObjectKey) continue;
        target.Clicked();
        return;
      }
    }
  }
}