using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame.Core {
    [Serializable] public struct PlayerData {
        public bool canPlay;
        public List<TileData> AllTileTypes;
        
        /// <summary>
        /// <c>default</c> means going to be replaced
        /// </summary>
        public List<TileData> UsableTiles;
        
        /// <summary>
        /// <see cref="Guid.Empty"/> means not selected anything
        /// </summary>
        public Guid selectedUsableTileID;
    }


    [Serializable] public struct BoardData {
        public TileData[,] tiles;

        public Vector2Int GetIndexesOf(Guid ID) {
            for (int x = 0; x < tiles.GetLength( 0 ); x++) {
                for (int y = 0; y < tiles.GetLength( 1 ); y++) {
                    if (tiles[x, y].ID == ID) return new Vector2Int( x, y );
                }
            }

            throw new Exception( $"no tile found with id: {ID}" );
        }
    }


    [Serializable] public struct TileData : IEquatable<TileData> {
        public TileType type;
        public Guid ID;

        public bool Equals(TileData other) => ID.Equals( other.ID );
        public override int GetHashCode() => ID.GetHashCode();
        public static bool operator ==(TileData left, TileData right) => left.Equals( right );
        public static bool operator !=(TileData left, TileData right) => !left.Equals( right );
    }
}