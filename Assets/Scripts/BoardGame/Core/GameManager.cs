using System;
using System.Collections.Generic;
using Binject;
using UnityEngine;

namespace BoardGame.Core {
    [DefaultExecutionOrder( -1 )]
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(GameManager) )]
    public class GameManager : MonoBehaviour {
        public GameRules rules;
        
        PlayerData playerData;
        BoardData board;
        
        public event Action<TileData, Vector2Int> TilePlaced;
        public event Action PlayerUsableTilesChanged;
        public event Action PlayerSelectedUsableTileIdChanged;

        void Awake() {
            PopulateBoard();
            playerData.canPlay = true;
            InitializePlayerTiles();
            InsertToPlayerUsableTiles();
            BindPlayerData();
            BindBoardData();
            PlayerUsableTilesChanged?.Invoke();
            PlayerSelectedUsableTileIdChanged?.Invoke();
        }


        public void PlaceSelectedTileAt(Vector2Int position) {
            if (playerData.canPlay && TryGetSelectedTileIndex( out var index )) {
                // update player
                var tileData = playerData.UsableTiles[index];
                playerData.UsableTiles[index] = default;
                InsertToPlayerUsableTiles();
                playerData.selectedUsableTileID = Guid.Empty;
                BindPlayerData();

                // update board
                board.tiles[position.x, position.y] = tileData;
                BindBoardData();
                
                CheckWin();
                
                PlayerUsableTilesChanged?.Invoke();
                PlayerSelectedUsableTileIdChanged?.Invoke();
                TilePlaced?.Invoke( tileData, position );
            }

            bool TryGetSelectedTileIndex( out int index ) {
                for (int i = 0; i < playerData.UsableTiles.Count; i++) {
                    if (playerData.UsableTiles[i].ID == playerData.selectedUsableTileID) {
                        index = i;
                        return true;
                    }
                }

                index = default;
                return false;
            }
        }

        public void PutSelectedUsableTileToTrash() {
            for (int i = 0; i < playerData.UsableTiles.Count; i++) {
                if (playerData.UsableTiles[i].ID == playerData.selectedUsableTileID) {
                    playerData.UsableTiles[i] = default;
                    playerData.selectedUsableTileID = Guid.Empty;
                    InsertToPlayerUsableTiles();
                    BindPlayerData();
                    PlayerUsableTilesChanged?.Invoke();
                    PlayerSelectedUsableTileIdChanged?.Invoke();
                    CheckLose();
                    break;
                }
            }
        }
        
        public void SelectUsableTile(Guid tileID) {
            playerData.selectedUsableTileID = tileID;
            BindPlayerData();
            PlayerSelectedUsableTileIdChanged?.Invoke();
        }

        public void DeselectUsableTile() {
            playerData.selectedUsableTileID = Guid.Empty;
            BindPlayerData();
            PlayerSelectedUsableTileIdChanged?.Invoke();
        }

        /// <summary>
        /// returns <c>default</c> if not found
        /// </summary>
        public TileData GetSelectedUsableTile() {
            for (int i = 0; i < playerData.UsableTiles.Count; i++) {
                if (playerData.UsableTiles[i].ID == playerData.selectedUsableTileID) 
                    return playerData.UsableTiles[i];
            }
            return default;
        }

        void CheckWin() {
            for (int x = 0; x < board.tiles.GetLength( 0 ); x++)
            for (int y = 0; y < board.tiles.GetLength( 1 ); y++)
                if (board.tiles[x, y].type.ConsideredEmpty)
                    return;
            playerData.canPlay = false;
            Debug.Log( "YOU WIN!" );
            Debug.Break();
        }

        void CheckLose() {
            if (playerData.AllTileTypes.Count == 0) {
                Debug.Log( "YOU LOSE" );
                Debug.Break();
            }
        }
        
        void InitializePlayerTiles() {
            playerData.AllTileTypes = new List<TileData>( rules.totalTileCount );
            for (int i = 0; i < rules.totalTileCount; i++) {
                playerData.AllTileTypes.Add( new TileData() {
                    type = GetRandomTileType(),
                    ID = Guid.NewGuid()
                } );
            }
        }

        void PopulateBoard() {
            board.tiles = new TileData[rules.boardSize.x, rules.boardSize.y];
            for (int x = 0; x < board.tiles.GetLength( 0 ); x++) {
                for (int y = 0; y < board.tiles.GetLength( 1 ); y++) {
                    board.tiles[x, y] = new TileData() {
                        type = rules.startingTile,
                        ID = Guid.NewGuid()
                    };
                }
            }
        }

        bool InsertToPlayerUsableTiles() {
            bool changed = false;
            if (playerData.UsableTiles == null || playerData.UsableTiles.Count != rules.totalPlayableTileCount) {
                changed = true;
                playerData.UsableTiles = new List<TileData>( rules.totalPlayableTileCount );
                for (int i = 0; i < rules.totalPlayableTileCount; i++) 
                    playerData.UsableTiles.Add( default ); // we'll catch it in next validation
            }

            for (int i = 0; i < rules.totalPlayableTileCount; i++) {
                if (playerData.UsableTiles[i] == default && playerData.AllTileTypes.Count > 0) {
                    changed = true;
                    playerData.UsableTiles[i] = playerData.AllTileTypes[^1];
                    playerData.AllTileTypes.RemoveAt( playerData.AllTileTypes.Count - 1 );
                }
            }

            return changed;
        }

        public TileType GetRandomTileType() => rules.playableTiles[UnityEngine.Random.Range( 0, rules.playableTiles.Length - 1 )];

        void BindPlayerData() => this.FindNearestContext().Bind( playerData );
        void BindBoardData() => this.FindNearestContext().Bind( board );

    }
}