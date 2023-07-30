using System;
using Binject;
using BoardGame.Core;
using UnityEngine;

namespace BoardGame.View {
    [AddComponentMenu( nameof(BoardGame) + " " + nameof(TileView) )]
    [RequireComponent( typeof(BContext) )]
    public class TileView : MonoBehaviour {

        public Vector2 size;
        
        public event Action<TileData> OnClick;

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube( Vector3.zero, size );
        }

        public void SetData(TileData data) {
            GetComponent<BContext>().Bind( data );
            if (data.ID == Guid.Empty) {
                gameObject.SetActive( false );
            }
        }

        void OnMouseDown() {
            OnClick?.Invoke( GetComponent<BContext>().GetDependency<TileData>() );
        }
    }
}