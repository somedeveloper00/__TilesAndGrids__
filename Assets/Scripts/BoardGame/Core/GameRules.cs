using UnityEngine;

namespace BoardGame.Core {
    [CreateAssetMenu( fileName = "GameRules", menuName = nameof(BoardGame) + "/Game Rules", order = 0 )]
    public class GameRules : ScriptableObject {
        [field: SerializeField] public int totalTileCount { get; private set; } = 75;
        [field: SerializeField] public int totalPlayableTileCount { get; private set; } = 3;
        [field: SerializeField] public Vector2Int boardSize { get; private set; } = new Vector2Int( 7, 7 );
        [field: SerializeField] public TileType[] playableTiles { get; private set; }
        [field: SerializeField] public TileType startingTile { get; private set; }
    }
}