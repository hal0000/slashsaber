using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lightsaber : MonoBehaviour
{
    [SerializeField]
    GameObject _blade = null;

    [SerializeField]
    [ColorUsage(true, true)]
    Color _colour = Color.red;

    [SerializeField]
    Slider _slider;

    [SerializeField]
    float _slashAngle;

    IEnumerator _coroutine;
    bool _rotating;
    PredictCollide _pC;
    public virtual void Start()
    {
        Material bladeMaterial = Instantiate(_blade.GetComponent<MeshRenderer>().sharedMaterial);
        bladeMaterial.SetColor("Color_AF2E1BB", _colour);
        _blade.GetComponent<MeshRenderer>().sharedMaterial = bladeMaterial;
        _slider.value = transform.eulerAngles.z;
        TryGetComponent(out _pC);
        EventManager.OnHit += HandleHitEvent;
        EventManager.OnSlash += Slash;
    }

    /// <summary>
    /// Based on slider's value, changes rotation of transform on z axis 
    /// </summary>
    public  void ChangeRotation(float rotZ)
    {
        if(_pC != null)
            _pC.Predict = true;
        var rot = transform.eulerAngles;
        rot.z = rotZ;
        transform.eulerAngles = rot;
    }

    /// <summary>
    /// Calls from simulate button
    /// </summary>
    void Slash()
    {
        if (_rotating)
            return;
        _coroutine = Rotate(Vector3.right, _slashAngle, .5f);
        _rotating = true;
        StartCoroutine(_coroutine);
    }

    /// <summary>
    /// Rotates to given angle on x axis
    /// </summary>
    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.fixedDeltaTime;
            yield return null;
        }
        transform.rotation = to;
        _coroutine = RotateBack(Vector3.right, -_slashAngle, .5f);
        StartCoroutine(_coroutine);
    }

    /// <summary>
    /// rotates back if it enters trigger or finished slash coroutine
    /// </summary>
    public IEnumerator RotateBack(Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.fixedDeltaTime;
            yield return null;
        }
        transform.rotation = to;
        _rotating = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
            EventManager.SaberHit(other.ClosestPointOnBounds(transform.position));
    }

    void HandleHitEvent(Vector3 hit)
    {
        StopCoroutine(_coroutine);
        _coroutine = RotateBack(Vector3.right, 0 - transform.rotation.eulerAngles.x, .5f);
        StartCoroutine(_coroutine);
        //if (_pC != null)Instantiate hit effect
    }
}