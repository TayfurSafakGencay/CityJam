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
    
    public bool CheckMatching(CollectableObjectKey collectableObjectKey)
    {
      List<CollectorView> _targetCollectorViews = new();
      
      foreach (CollectorView collectorView in CollectorViews)
      {
        if (!collectorView.IsMatchable()) continue;
        if (collectorView.GetCollectableObjectKey() != collectableObjectKey) continue;
        _targetCollectorViews.Add(collectorView);

        if (_targetCollectorViews.Count == RequiredMatchingCount) break;
      }

      if (_targetCollectorViews.Count != RequiredMatchingCount) return false;
      // TODO: Eger bir collect animasyonu varsa yana kayma oynamiyor.
      foreach (CollectorView targetCollectorView in _targetCollectorViews)
      {
        targetCollectorView.Matched(_targetCollectorViews[1].GetCollectableObject().transform);
      }

      return true;
    }
  }
}