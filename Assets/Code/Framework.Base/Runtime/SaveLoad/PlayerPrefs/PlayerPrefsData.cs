namespace General.SaveLoad
{
    internal struct PlayerPrefsData
    {
        internal readonly string Kay;
        internal readonly string Value;

        internal PlayerPrefsData(string kay, string value)
        {
            Kay = kay;
            Value = value;
        }
    }
}