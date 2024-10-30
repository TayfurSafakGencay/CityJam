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

    public void Clicked(CollectableObjectKey collectableObjectKey)
    {
      PanelManager.Instance.InGamePanel.Clicked(collectableObjectKey);
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