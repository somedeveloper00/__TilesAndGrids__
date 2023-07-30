using UnityEngine;

namespace BoardGame.View {
    [CreateAssetMenu( fileName = "Resources", menuName = nameof(BoardGame) + "/Resources", order = 0 )]
    public class GameResources : ScriptableObject {
        [field: SerializeField] public TileView inboardTileViewPrefab { get; private set; } 
        [field: SerializeField] public TileView selectableTileViewPrefab { get; private set; } 
    }
}