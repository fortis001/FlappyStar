using System;
using LSH.Utils;
using LSH.Utils.Pooling;
using UnityEngine;


namespace FlappyStar.Gameplay.Entities
{
    public class ObstaclePair : MonoBehaviour, IPoolable
    {
        [Header("Slots")]
        [SerializeField] private Transform _upperSlot;
        [SerializeField] private Transform _lowerSlot;

        [Header("Trigger")]
        [SerializeField] private TriggerRelay2D _passTrigger;
        [SerializeField] private SpriteRenderer _triggerRenderer;
        [SerializeField] private float _triggerSpriteMargin = 5f;


        private Obstacle _upperObstacle;
        private Obstacle _lowerObstacle;

        public event Action<int> OnPlayerPassed;
        public event Action OnPlayerTouched;

        public void OnCreated()
        {
            _passTrigger.OnTriggerEntered -= HandleTriggerEntered;
            _passTrigger.OnTriggerEntered += HandleTriggerEntered;
        }

        public void Activate(Obstacle upperObstacle, Obstacle lowerObstacle, float gapHeight, float playAreaHeight)
        {

            _upperObstacle = upperObstacle;
            _lowerObstacle = lowerObstacle;

            AttachToSlot(_upperObstacle, _upperSlot);
            AttachToSlot(_lowerObstacle, _lowerSlot);

            _upperObstacle.OnPlayerTouched += HandlePlayerTouched;
            _lowerObstacle.OnPlayerTouched += HandlePlayerTouched;

            _upperObstacle.TurnOffLights();
            _lowerObstacle.TurnOffLights();

            AlignToGap(gapHeight);
            ResizePole(playAreaHeight);
        }

        private void AlignToGap(float gapHeight)
        {
            if (_upperObstacle == null || _lowerObstacle == null)
                return;

            float halfGapHeight = gapHeight * 0.5f;

            float upperY = halfGapHeight + _upperObstacle.HalfHeight;
            float lowerY = -halfGapHeight - _lowerObstacle.HalfHeight;

            SetLocalY(_upperSlot, upperY);
            SetLocalY(_lowerSlot, lowerY);
        }

        private static void SetLocalY(Transform target, float y)
        {
            Vector3 position = target.localPosition;
            position.y = y;
            target.localPosition = position;
        }

        private static void AttachToSlot(Obstacle target, Transform slot)
        {
            Transform transform = target.transform;

            transform.SetParent(slot, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public void DetachObstacles(
            out Obstacle upperObstacle,
            out Obstacle lowerObstacle)
        {
            if (_upperObstacle != null)
                _upperObstacle.OnPlayerTouched -= HandlePlayerTouched;

            if (_lowerObstacle != null)
                _lowerObstacle.OnPlayerTouched -= HandlePlayerTouched;

            upperObstacle = _upperObstacle;
            lowerObstacle = _lowerObstacle;

            _upperObstacle = null;
            _lowerObstacle = null;
        }

        

        public void Move(float deltaTime, float moveSpeedX)
        {
            transform.position += Vector3.left * moveSpeedX * deltaTime;
        }

        private void ResizePole(float height)
        {
            if (_triggerRenderer == null)
                return;

            _triggerRenderer.drawMode = SpriteDrawMode.Tiled;
            _triggerRenderer.size = new Vector2(_triggerRenderer.size.x, height + _triggerSpriteMargin);
        }

        private void HandleTriggerEntered(Collider2D collider)
        {
            if (!collider.CompareTag("Player"))
                return;

            _upperObstacle.TurnOnLights();
            _lowerObstacle.TurnOnLights();

            int score = _upperObstacle.Score + _lowerObstacle.Score;

            OnPlayerPassed?.Invoke(score);
        }

        private void HandlePlayerTouched()
        {
            OnPlayerTouched?.Invoke();
        }

        private void OnDestroy()
        {
            _passTrigger.OnTriggerEntered -= HandleTriggerEntered;
        }


        #region IPoolable
        public void OnGet()
        {
        }

        public void OnRelease()
        {
        }
        #endregion
    }
}

