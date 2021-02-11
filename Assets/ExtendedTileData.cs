using UnityEngine.Tilemaps;

namespace IdleFarmer
{
    public enum TileState
    {
        Highlighted,
        Grass,    
        GrassSelected,
        Road,
        HoedField,
        Planted
    }
    public class ExtTileData
    {
        #region TileStateTracking

        private TileState _state = TileState.Grass;
        private TileState _lastState = TileState.Grass;
        public TileState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                if (_state != TileState.GrassSelected && _state != TileState.Highlighted)
                {
                    _lastState = _state;
                }

                _state = value;
            }
        }
        public Tile Tile;   

        #endregion
        
        #region ExtendedData

        public int Val;
        public bool IsSelected = false;

        #endregion
        
        public void UndoStateChange()
        {
            var v = _state;
            _state = _lastState;
            _lastState = v;
        }
    }
}