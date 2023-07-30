using UnityEngine;

namespace BoardGame.Core {
    [CreateAssetMenu( fileName = "TileType", menuName = nameof(BoardGame) + "/Tile Type", order = 0 )]
    public class TileType : ScriptableObject {
        [field: SerializeField] public bool ConsideredEmpty { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}