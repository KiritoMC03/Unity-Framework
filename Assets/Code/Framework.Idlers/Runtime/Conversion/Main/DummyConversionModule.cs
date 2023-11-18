using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Framework.Idlers.Conversion
{
    public sealed class DummyConversionModule : ConversionModule<DummyConversionModule.DummyType, DummyConversionModule.DummyItem>
    {
        public struct DummyType{}
        public struct DummyItem{}
        public class DummyRecipe : ConversionRecipe<DummyType> {}

        private static readonly Func<DummyType, DummyItem> OutputCreator = _ => new DummyItem();
        private static readonly Action<DummyItem> InputDestroyer = _ => { };
        private static readonly ConversionRecipe<DummyType>[] Configs = { new DummyRecipe() };

        public DummyConversionModule() : base(OutputCreator, InputDestroyer, Configs)
        { }

        public async UniTask Convert(CancellationToken cancellationToken = default) => 
            await base.Convert(new DummyItem(), new DummyType(), cancellationToken);
    }
}