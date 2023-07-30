using Binject;
using BoardGame.Core;
using UnityEngine;

namespace BoardGame.View.TileViewExtras {
    [RequireComponent( typeof(SpriteRenderer) )]
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(TileSpriteRendererColor) )]
    public class TileSpriteRendererColor : MonoBehaviour {

        SpriteRenderer _spriteRenderer;

        void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        void Update() {
            if (this.TryGetDependency( out TileData tileData )) {
                if (_spriteRenderer.color != tileData.type.Color)
                    _spriteRenderer.color = tileData.type.Color;
            }
        }
    }
}