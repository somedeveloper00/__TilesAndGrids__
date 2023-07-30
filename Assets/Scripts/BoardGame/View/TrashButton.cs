using System;
using Binject;
using BoardGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace BoardGame.View {
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(TrashButton) )]
    public class TrashButton : MonoBehaviour {

        [SerializeField] UnityEvent onCanUse;
        [SerializeField] UnityEvent onCantUse;
        
        GameManager _manager;
        bool _lastCanUse;

        void Awake() => _manager = this.GetDependency<GameManager>();

        void LateUpdate() {
            var canUse = this.GetDependency<PlayerData>().selectedUsableTileID != Guid.Empty;
            if (_lastCanUse != canUse) {
                if (canUse) onCanUse?.Invoke();
                else onCantUse?.Invoke();
                _lastCanUse = canUse;
            }
        }

        void OnMouseDown() {
            if (!_lastCanUse) return;
            _manager.PutSelectedUsableTileToTrash();
        }
    }
}