using Binject;
using BoardGame.Core;
using TMPro;
using UnityEngine;

namespace BoardGame.View.TileViewExtras {
    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(TileText) )]
    public class TileText : MonoBehaviour {

        TMP_Text _text;

        void Awake() => _text = GetComponent<TMP_Text>();

        void Update() {
            if (this.TryGetDependency(out TileData tileData)) {
                if (_text.text != tileData.type.name)
                    _text.text = tileData.type.name;
            }
        }
    }
}