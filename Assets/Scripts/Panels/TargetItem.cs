using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class TargetItem : MonoBehaviour
  {
    [HideInInspector]
    public CollectableObjectKey Key;

    private Image _image;

    public TextMeshProUGUI CountText;
    
    public ParticleSystem CounterDownParticle;

    private int _count;

    private void Awake()
    {
      _image = GetComponent<Image>();
    }

    public void SetInitialValues(Sprite sprite, int count, CollectableObjectKey key)
    {
      _count = count;

      Key = key;
      _image.sprite = sprite;
      CountText.text = _count.ToString();
    }

    public void Clicked()
    {
      _count--;
      CountText.text = _count.ToString();
      CounterDownParticle.Play();

      if (_count == 0)
      {
        //TODO: Safak: Play animation & particle effect.
        gameObject.SetActive(false);
      }
    }
  }
}