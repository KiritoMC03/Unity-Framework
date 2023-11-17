using System;
using UnityEngine;

namespace Framework.Base.CSV
{
    [Serializable]
    public struct Rect
    {
        #region Fields

        [SerializeField]
        private int startLine;

        [SerializeField]
        private int startColumn;

        [SerializeField]
        private int endLine;

        [SerializeField]
        private int endColum;

        #endregion

        #region Properties

        public int StartLine
        {
            get => startLine;
            set => startLine = value;
        }

        public int StartColumn
        {
            get => startColumn;
            set => startColumn = value;
        }

        public int EndLine
        {
            get => endLine;
            set => endLine = value;
        }

        public int EndColum
        {
            get => endColum;
            set => endColum = value;
        }

        #endregion

        #region Class lifecycle

        public Rect(int startLine = 0, int startColumn = 0, int endLine = -1, int endColum = -1)
        {
            this.startLine = startLine;
            this.startColumn = startColumn;
            this.endLine = endLine;
            this.endColum = endColum;
        }

        #endregion
    }
}