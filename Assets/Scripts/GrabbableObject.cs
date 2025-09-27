using UnityEngine;
using HTC.UnityPlugin.Vive;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _originalParent;
    private bool _isHeld = false;

    [Header("Vive Settings")]
    [SerializeField] private HandRole _hand = HandRole.RightHand;   // Какая рука будет хватать
    [SerializeField] private ControllerButton _grabButton = ControllerButton.Trigger;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _originalParent = transform.parent;
    }

    private void Update()
    {
        if (_isHeld)
        {
            // Отпустить предмет
            if (ViveInput.GetPressUp(_hand, _grabButton))
            {
                ReleaseObject();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Проверяем, что это контроллер
        if (other.CompareTag("Controller"))
        {
            // Нажата кнопка захвата → берём предмет
            if (!_isHeld && ViveInput.GetPressDown(_hand, _grabButton))
            {
                GrabObject(other.transform);
            }
        }
    }

    private void GrabObject(Transform controller)
    {
        _isHeld = true;
        _rb.isKinematic = true;               // отключаем физику
        transform.SetParent(controller);     // "прикрепляем" к руке
        transform.localPosition = Vector3.zero;  // подправить смещение
        transform.localRotation = Quaternion.identity;
    }

    private void ReleaseObject()
    {
        _isHeld = false;
        transform.SetParent(_originalParent); // возвращаем в иерархию
        _rb.isKinematic = false;              // включаем физику обратно
    }
}

