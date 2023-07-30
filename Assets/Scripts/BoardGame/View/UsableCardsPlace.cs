using System.Collections.Generic;
using Binject;
using BoardGame.Core;
using UnityEngine;

namespace BoardGame.View {
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(UsableCardsPlace) )]
    public class UsableCardsPlace : MonoBehaviour {
        
        GameManager _manager;
        GameResources _resources;
        List<TileView> _tileViews = new List<TileView>( 3 );

        void Awake() {
            _manager = this.GetDependency<GameManager>();
            _resources = this.GetDependency<GameResources>();
        }

        void OnEnable() => _manager.PlayerUsableTilesChanged += UpdateTiles;
        void OnDisable() => _manager.PlayerUsableTilesChanged -= UpdateTiles;
        void Start() => UpdateTiles();

        void UpdateTiles() {
            var playerData = this.GetDependency<PlayerData>();
            
            // create new
            for (int i = 0; i < playerData.UsableTiles.Count; i++) {
                if (_tileViews.Count <= i) _tileViews.Add( null );
                if (_tileViews[i] == null) {
                    var tileView = Instantiate( _resources.selectableTileViewPrefab, transform );
                    tileView.name = "Tile " + i;
                    var index = i;
                    tileView.OnClick += OnTileClicked;
                    _tileViews[i] = tileView;
                }
            }
            // destroy old
            for (int i = _tileViews.Count - 1; i >= playerData.UsableTiles.Count; i--) {
                Destroy( _tileViews[i].gameObject );
                _tileViews.RemoveAt( i );
            }
            // assign
            for (int i = 0; i < playerData.UsableTiles.Count; i++) {
                var tileView = _tileViews[i];
                tileView.SetData( playerData.UsableTiles[i] );
                tileView.transform.localPosition = new Vector3( i * tileView.size.x, 0, 0 );
            }
        }

        void OnTileClicked(TileData tileData) {
            if (_manager.GetSelectedUsableTile() != tileData)
                _manager.SelectUsableTile( tileData.ID );
            else 
                _manager.DeselectUsableTile();
        }
    }
}