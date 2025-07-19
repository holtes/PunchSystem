using _Project.Develop.Runtime.Core.Signals;
using _Project.Develop.Runtime.Domain.Models;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

namespace _Project.Develop.Runtime.Domain.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractor _leftHand;
        [SerializeField] private XRBaseInteractor _rightHand;

        private PlayerModel _model;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(PlayerModel playerModel, SignalBus signalBus)
        {
            _model = playerModel;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<OnEndGameSignal>(DestroyWeapons);

            _leftHand.selectEntered.AddListener(OnLeftGrab);
            _rightHand.selectEntered.AddListener(OnRightGrab);

            _leftHand.selectExited.AddListener(OnLeftRelease);
            _rightHand.selectExited.AddListener(OnRightRelease);
        }

        private void OnLeftGrab(SelectEnterEventArgs args)
        {
            var obj = args.interactableObject.transform.gameObject;
            _model.TryGrabWeapon(obj, false);
        }

        private void OnRightGrab(SelectEnterEventArgs args)
        {
            var obj = args.interactableObject.transform.gameObject;
            _model.TryGrabWeapon(obj, true);
        }

        private void OnLeftRelease(SelectExitEventArgs args)
        {
            _model.DropWeapon(false);
        }

        private void OnRightRelease(SelectExitEventArgs args)
        {
            _model.DropWeapon();
        }

        private void DestroyWeapons()
        {
            var leftWeapon = _model.GetHeldWeapon(false);
            var rightWeapon = _model.GetHeldWeapon();

            if (leftWeapon != null)
            {
                _leftHand.EndManualInteraction();
                Destroy(leftWeapon);
            }

            if (rightWeapon != null)
            {
                _rightHand.EndManualInteraction();
                Destroy(rightWeapon);
            }

            _model.DropAllWeapons();
        }
    }
}