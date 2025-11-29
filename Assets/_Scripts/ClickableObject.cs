using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    public UnityEvent<GameObject> OnClick;
    public UnityEvent _Onclick;
    public UnityEvent OnRelease;
    public void HandleClick(GameObject go)
    {
        if (OnClick != null)
        {
            OnClick.Invoke(go);
        }
        if (_Onclick != null)
        {
            _Onclick.Invoke();
        }
    }

    public void HandleRelease()
    {
        if (OnRelease != null)
        {
            OnRelease.Invoke();
        }
    }
}
