using Binject;
using BoardGame.Core;
using UnityEngine;

namespace BoardGame.View {
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(PlayerInput) )]
    public class PlayerInput : MonoBehaviour {

        [SerializeField] LayerMask tileLayer;
        
        GameManager _manager;
        BoardView _boardView;

        void Awake() {
            _manager = this.GetDependency<GameManager>();
            _boardView = this.GetDependency<BoardView>();
        }

        void OnEnable() => _boardView.TileCreated += OnBoardTileCreated;
        void OnDisable() => _boardView.TileCreated += OnBoardTileCreated;


        void OnBoardTileCreated(TileView tileView) {
            tileView.OnClick += PlaceTile;

            void PlaceTile(TileData tileData) {
                if (!this || !enabled) return;
                if (!tileData.type.ConsideredEmpty) return;
                this.GetDependency( out PlayerData playerData, out BoardData boardData );
                if (!playerData.canPlay) return;
                _manager.PlaceSelectedTileAt( boardData.GetIndexesOf( tileData.ID ) );
            }
        }
    }
}