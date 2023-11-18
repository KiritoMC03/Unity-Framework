namespace Framework.Idlers.Conversion
{
    public struct ConversionResult<TObject>
    {
        public bool isSuccess;
        public TObject output;

        public static ConversionResult<TObject> Default => new ConversionResult<TObject>(false, default);

        public ConversionResult(bool isSuccess, TObject output)
        {
            this.isSuccess = isSuccess;
            this.output = output;
        }

        public void Deconstruct(out bool isSuccess, out TObject output)
        {
            isSuccess = this.isSuccess;
            output = this.output;
        }
    }
}