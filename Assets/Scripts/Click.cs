using Interface;
using UnityEngine;

public class Click : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    public void Update()
    {
        if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
        Ray ray = _camera.ScreenPointToRay(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, _camera.nearClipPlane));

        if (!Physics.Raycast(ray, out RaycastHit hit, 100000)) return;
        
        if (hit.collider.gameObject.GetComponent<IClickable>() != null)
        {
            hit.collider.gameObject.GetComponent<IClickable>().OnClick();
        }
    }
    
    // TODO: Safak 
    // Post Processing eklenecek.
    // Lighting ayarlamalari yapilacak. Light Baking.
    // yakinlastirma, uzaaklastirma yapilacak.
    // Water duzenlemesi
    
    // Music eklenecek.
    // 4 Level eklenecek.
}
