﻿namespace Framework.Idlers.Conversion
{
    public interface IConvertible<TMatcher>
    {
        public TMatcher Matcher { get; }
    }
}