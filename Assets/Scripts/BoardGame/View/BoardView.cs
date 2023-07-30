using System;
using Binject;
using BoardGame.Core;
using UnityEngine;

namespace BoardGame.View {
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(BoardView) )]
    public class BoardView : MonoBehaviour {
        
        public TileView[,] TileViews { get; private set; }
        
        GameResources _gameResources;
        GameManager _manager;

        public event Action<TileView> TileCreated;
        public event Action<TileView> TileDestroyed;
        
        void Awake() {
            _gameResources = this.GetDependency<GameResources>();
            _manager = this.GetDependency<GameManager>();
        }
        void Start() {
            var boardData = this.GetDependency<BoardData>();
            SpawnBoardTiles( boardData );
        }

        void OnEnable() => _manager.TilePlaced += OnTilePlaced;
        void OnDisable() => _manager.TilePlaced -= OnTilePlaced;


        void OnTilePlaced(TileData tileData, Vector2Int position) {
            // destroy old
            var destroyingTile = TileViews[position.x, position.y];
            TileDestroyed?.Invoke( destroyingTile );
            Destroy( destroyingTile );
            // create new
            CreateTileAt( tileData, position );
        }

        void SpawnBoardTiles(BoardData boardData) {
            TileViews = new TileView[boardData.tiles.GetLength( 0 ), boardData.tiles.GetLength( 1 )];
            for (int x = 0; x < boardData.tiles.GetLength( 0 ); x++) {
                for (int y = 0; y < boardData.tiles.GetLength( 1 ); y++) {
                    CreateTileAt( boardData.tiles[x, y], new Vector2Int( x, y ) );
                }
            }
        }

        void CreateTileAt(TileData tileData, Vector2Int position) {
            var tileView = Instantiate( _gameResources.inboardTileViewPrefab, transform );
            tileView.name = "Tile " + position.x + " " + position.y;
            tileView.SetData( tileData );
            tileView.transform.localPosition = new Vector3(
                position.x * tileView.size.x,
                position.y * tileView.size.y,
                0 );
            TileViews[position.x, position.y] = tileView;
            TileCreated?.Invoke( tileView );
        }
    }
}