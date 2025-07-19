using UnityEngine;
using R3.Triggers;
using R3;

namespace _Project.Develop.Runtime.Core.Services
{
    public class CollisionDetectionService : MonoBehaviour
    {
        public Observable<Collision> OnCollisionDetected => _onCollisionDetected;

        private Subject<Collision> _onCollisionDetected = new Subject<Collision>();

        private void Awake()
        {
            this.OnCollisionEnterAsObservable()
                .Subscribe(collision => DetectCollision(collision))
                .AddTo(this);
        }

        private void DetectCollision(Collision collision)
        {
            _onCollisionDetected.OnNext(collision);
        }
    }
}