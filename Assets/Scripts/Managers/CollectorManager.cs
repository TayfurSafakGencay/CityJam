using System;
using System.Collections.Generic;
using Collector;
using Enum;
using UnityEngine;

namespace Managers
{
  public class CollectorManager : MonoBehaviour
  {
    public static CollectorManager Instance;

    [HideInInspector]
    public List<CollectorView> CollectorViews;

    [HideInInspector]
    public int TotalCollectableObjects;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      
      for (int i = 0; i < transform.childCount; i++)
      {
        CollectorView child = transform.GetChild(i).GetComponent<CollectorView>();
        CollectorViews.Add(child);
      }
    }

    private void Start()
    {
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
      if (GameManager.Instance.GameState == GameState.Game)
      {
        TotalCollectableObjects = 0;
      }
    }

    public void FillingCollector(CollectableObject collectableObject)
    {
      foreach (CollectorView collectorView in CollectorViews)
      {
        if (collectorView.GetCollectableObjectKey() != CollectableObjectKey.None) continue;
        collectorView.Filling(collectableObject);
        break;
      }
    }

    private const int RequiredMatchingCount = 3;
    
    public void CheckMatching(CollectableObjectKey collectableObjectKey)
    {
      List<CollectorView> _targetCollectorViews = new();
      
      foreach (CollectorView collectorView in CollectorViews)
      {
        if (!collectorView.IsMatchable()) continue;
        if (collectorView.GetCollectableObjectKey() != collectableObjectKey) continue;
        _targetCollectorViews.Add(collectorView);

        if (_targetCollectorViews.Count == RequiredMatchingCount) break;
      }

      if (_targetCollectorViews.Count == RequiredMatchingCount)
      {
        foreach (CollectorView targetCollectorView in _targetCollectorViews)
        {
          targetCollectorView.Matched(_targetCollectorViews);
          TotalCollectableObjects--;
        }
        
        return;
      }

      CheckLoseCondition();
    }
    
    public void CheckLoseCondition()
    {
      foreach (CollectorView collectorView in CollectorViews)
      {
        if (collectorView.GetCollectableObjectKey() == CollectableObjectKey.None) return;
      }
      
      LevelManager.Lose();
    }

    public void CheckWinCondition()
    {
      foreach (CollectorView collectorView in CollectorViews)
      {
        if (collectorView.GetCollectableObjectKey() != CollectableObjectKey.None) return;
      }
      
      if(TotalCollectableObjects == 0)
        LevelManager.Win();
    }

    public void SlideToLeft()
    {
      for (int i = 0; i < CollectorViews.Count; i++)
      {
        CollectorView collectorView = CollectorViews[i];
        if (i == 0) continue;
        
        if (collectorView.GetCollectableObjectKey() != CollectableObjectKey.None)
        {
          collectorView.GetCollectableObject().SlideToLeft(i);
        }
      }
    }
  }
}