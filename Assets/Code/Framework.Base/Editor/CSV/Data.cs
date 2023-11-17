using System.Collections.Generic;

namespace Framework.Base.Editor
{
    internal class Data
    {
        #region Fields

        private readonly List<List<string>> data;
        private readonly List<string> headers;

        #endregion

        #region Properties

        public List<List<string>> GetData => data;
        public List<string> Headers => headers;

        #endregion

        #region Class lifecycle

        public Data(List<List<string>> data, List<string> headers)
        {
            this.data = data;
            this.headers = headers;
        }

        #endregion
    }
}