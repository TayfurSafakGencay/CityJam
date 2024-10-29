using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collector;
using DG.Tweening;
using Enum;
using Interface;
using Managers;
using UnityEngine;

public class CollectableObject : MonoBehaviour, IClickable
{
    [SerializeField]
    private CollectableObjectKey _key;

    public Action OnAnimationEnd;

    private bool _clicked;
    
    private bool _isMatched;
    
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    private void Start()
    {
        CollectorManager.Instance.TotalCollectableObjects++;
    }

    public void OnClick()
    {
        if (_clicked) return;
        _clicked = true;

        OutlineEffect();

        CollectorManager.Instance.FillingCollector(this);
    }

    private async void OutlineEffect()
    {
        _outline.enabled = true;

        await Task.Delay(600);

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            child.gameObject.layer = 6;
        }
        
        if (_outline != null)
        { 
            _outline.enabled = false;
        }
    }

    public void PlayPlacingAnimation(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, 1f).OnComplete(() =>
        {
            OnAnimationEnd?.Invoke();
        });
    }
    
    private const float _matchingAnimationDuration = 0.85f;
    
    private const float _matchingAnimationHeight = 0.5f;
    private const float _matchingAnimationRight = 0.25f;
    private const float _matchingAnimationXAxisLeft = -0.25f;
    
    private const Ease _matchingAnimationEase = Ease.InCubic;
    
    public void PlayMatchingAnimation(List<CollectorView> collectorViews)
    {
        _isMatched = true;
        
        if (collectorViews[0].GetCollectableObject() == this)
        {
            transform.DOMove(transform.position + new Vector3(_matchingAnimationXAxisLeft, _matchingAnimationHeight,0), _matchingAnimationDuration)
                .SetEase(_matchingAnimationEase).OnComplete(() =>
            {
                transform.DOMove(collectorViews[1].GetCollectableObject().transform.position, _matchingAnimationDuration).SetEase(_matchingAnimationEase).OnComplete(() =>
                {
                    collectorViews[0].Remove();
                    Destroy(gameObject);
                });
            });
        }
        else if (collectorViews[1].GetCollectableObject() == this)
        {
            transform.DOMove(transform.position + new Vector3(0, _matchingAnimationHeight,0), _matchingAnimationDuration)
                .SetEase(_matchingAnimationEase).OnComplete(() =>
            {
                transform.DOScale(transform.localScale * 1.25f, _matchingAnimationDuration * 1.5f).OnComplete(() =>
                {
                    collectorViews[1].Remove();
                    CollectorManager.Instance.SlideToLeft();
                    CollectorManager.Instance.CheckWinCondition();
                    Destroy(gameObject);
                });
            });
        }
        else if (collectorViews[2].GetCollectableObject() == this)
        {
            transform.DOMove(transform.position + new Vector3(_matchingAnimationRight, _matchingAnimationHeight,0), _matchingAnimationDuration)
                .SetEase(_matchingAnimationEase).OnComplete(() =>
                {
                    transform.DOMove(collectorViews[1].GetCollectableObject().transform.position, _matchingAnimationDuration).SetEase(_matchingAnimationEase).OnComplete(() =>
                    {
                        collectorViews[2].Remove();
                        Destroy(gameObject);
                    });
                });
        }
    }

    public void SlideToLeft(int index)
    {
        if (_isMatched) return;
        if (index == 0) return;
        
        index--;

        if (CollectorManager.Instance.CollectorViews[index].GetCollectableObjectKey() != CollectableObjectKey.None) return;
        CollectorManager.Instance.CollectorViews[index].Filling(this, false);
        CollectorManager.Instance.CollectorViews[index + 1].Remove();
        SlideAnimation(index);
    }

    private void SlideAnimation(int index)
    {
        CollectorView collectorView = CollectorManager.Instance.CollectorViews[index];
        float height = collectorView.PlacementHeight;
        float duration = 0.2f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            transform.DOMoveX(collectorView.transform.position.x, duration).SetEase(Ease.Linear) // X ekseni boyunca hareket
        );

        sequence.Join(
            transform.DOMoveY(collectorView.transform.position.y + 0.2f, duration / 2).SetEase(Ease.OutQuad) // Y ekseni boyunca yükselme
                .OnComplete(() =>
                {
                    transform.DOMoveY(collectorView.transform.position.y + height, duration / 2).SetEase(Ease.InQuad).OnComplete(() =>
                    {
                        CollectorManager.Instance.CollectorViews[index].Slided();
                        SlideToLeft(index);
                    });
                })
        );
    }

    public CollectableObjectKey GetKey()
    {
        return _key;
    }
}
