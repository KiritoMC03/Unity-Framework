using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Idlers.Conversion
{
    public class ConversionModule<TMatcher, TObject>
    {
        #region Events
        
        public event Action ConversionStartedCallback;
        public event Action ConversionStoppedCallback;

        #endregion

        #region Fields

        private ConversionRecipe<TMatcher>[] configs;
        private Func<TMatcher, TObject> defaultOutputCreator;
        private Action<TObject> defaultInputDestroyer;
        private CancellationTokenSource rootCancellationTokenSource;
        
        #endregion

        #region Properties

        public bool IsWorks { get; private set; }
        public float WorkDuration { get; set; }

        #endregion

        #region Constructors

        public ConversionModule(Func<TMatcher, TObject> defaultOutputCreator,
            Action<TObject> defaultInputDestroyer, 
            params ConversionRecipe<TMatcher>[] newConfigs)
        {
            defaultOutputCreator.LogIfNull();
            defaultInputDestroyer.LogIfNull();
            
            if (newConfigs.NotNull()) RefreshConversionConfigs(newConfigs);
            ReplaceOutputCreator(defaultOutputCreator);
            ReplaceInputDestroyer(defaultInputDestroyer);
        }

        #endregion
        
        #region Methods

        public void RefreshConversionConfigs(params ConversionRecipe<TMatcher>[] newConfigs)
        {
            this.configs = newConfigs;
        }

        public void ReplaceOutputCreator(Func<TMatcher, TObject> defaultOutputCreator)
        {
            this.defaultOutputCreator = defaultOutputCreator;
        }

        public void ReplaceInputDestroyer(Action<TObject> defaultInputDestroyer)
        {
            this.defaultInputDestroyer = defaultInputDestroyer;
        }

        public void BreakConversion()
        {
            rootCancellationTokenSource.Cancel();
            IsWorks = false;
        }

        public async UniTask<ConversionResult<TObject>> Convert(TObject input, 
            TMatcher inputMatcher,
            CancellationToken cancellationToken = default)
        {
            if (configs.IsNullOrEmpty())
            {
                Debug.LogWarning("Configs is null or empty.");
                return default;
            }
            
            if (IsWorks)
            {
                Debug.LogWarning($"Conversion is run now, can not run new conversion.");
                return ConversionResult<TObject>.Default;
            }

            ConversionRecipe<TMatcher> resultConfig = default;
            foreach (ConversionRecipe<TMatcher> current in configs)
            {
                if (inputMatcher.Equals(current.Input))
                {    
                    resultConfig = current;
                    break;
                }
            }

            if (resultConfig.IsNull())
            {
                Debug.LogWarning($"Config with matcher of input {input} not found.");
                return ConversionResult<TObject>.Default;
            }

            rootCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            return new ConversionResult<TObject>(true, await ConvertCycle(resultConfig, input));
        }

        private async UniTask<TObject> ConvertCycle(ConversionRecipe<TMatcher> config, TObject input)
        {
            IsWorks = true;
            ConversionStartedCallback?.Invoke();
            defaultInputDestroyer.Invoke(input);
            await UniTask.WaitForSeconds(WorkDuration);

            IsWorks = false;
            TObject output = defaultOutputCreator.Invoke(config.Output);
            ConversionStoppedCallback?.Invoke();
            return output;
        }

        #endregion
    }
}
