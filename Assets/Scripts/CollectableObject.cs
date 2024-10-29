using System;
using System.Collections.Generic;
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
    
    [SerializeField]
    private Vector3 _collectedRotation = new(0, 270, 0);
    
    [SerializeField]
    private Vector3 _collectedScale = new(0.5f, 0.5f, 0.5f);

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

        _outline.enabled = true;

        CollectorManager.Instance.FillingCollector(this);
    }
    
    private const float _moveUpperDuration = 0.4f;
    
    private const float _rotateToCollectedRotation = 0.5f;
    
    private const float _moveToCollectorDuration = 0.5f;
    
    private const float _scaleToCollectedScale = 0.5f;

    public void PlayPlacingAnimation(Vector3 targetPosition)
    {
        transform.DOLocalMoveY(1f, _moveUpperDuration).OnComplete(() =>
        {
            ChangeLayer();

            transform.DOLocalRotate(new Vector3(0, _collectedRotation.y, 0), _rotateToCollectedRotation);

            DOVirtual.DelayedCall(_rotateToCollectedRotation / 2, () =>
            {
                transform.DOMove(targetPosition, _moveToCollectorDuration);
                transform.DOScale(_collectedScale, _scaleToCollectedScale);
                
                DOVirtual.DelayedCall(_moveToCollectorDuration - 0.1f, () =>
                {
                    OnAnimationEnd?.Invoke();
                });
            });
        });
    }

    private void ChangeLayer()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1.4f, transform.localPosition.z);
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
    
    private const float _matchingAnimationDuration = 0.35f;
    
    private const float _matchingAnimationHeight = 0.5f;
    private const float _matchingAnimationRight = 0.2f;
    private const float _matchingAnimationXAxisLeft = -0.2f;
    
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
                transform.DOScale(transform.localScale * 1.25f, _matchingAnimationDuration).OnComplete(() =>
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
