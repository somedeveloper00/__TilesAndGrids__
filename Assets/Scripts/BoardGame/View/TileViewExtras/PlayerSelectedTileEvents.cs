using Binject;
using BoardGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace BoardGame.View.TileViewExtras {
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(PlayerSelectedTileEvents) )]
    public class PlayerSelectedTileEvents : MonoBehaviour {
        [SerializeField] UnityEvent OnSelect;
        [SerializeField] UnityEvent OnDeselect;
        
        bool _lastSelected;
        
        void LateUpdate() {
            this.GetDependency( out PlayerData playerData, out TileData tileData );
            var selected = playerData.canPlay && playerData.selectedUsableTileID == tileData.ID;
            if (selected != _lastSelected) {
                if (selected) OnSelect?.Invoke();
                else OnDeselect?.Invoke();
                _lastSelected = selected;
            }
        }
    }
}