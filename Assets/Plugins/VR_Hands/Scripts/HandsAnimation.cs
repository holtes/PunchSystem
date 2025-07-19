using UnityEngine;
using UnityEngine.InputSystem;

public class HandsAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference _gripReference;
    [SerializeField] private InputActionReference _triggerReference;
    [SerializeField] private Animator _handAnimator;

    private void Update()
    {
        var gripValue = _gripReference.action.ReadValue<float>();
        _handAnimator.SetFloat("Grip", gripValue);

        var triggerValue = _triggerReference.action.ReadValue<float>();
        _handAnimator.SetFloat("Trigger", triggerValue);
    }
}
