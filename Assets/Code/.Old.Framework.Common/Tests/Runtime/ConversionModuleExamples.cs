namespace GameKit.Conversion.Runtime.Test
{
    public class ConversionModuleExamples
    {
        public void CreateModule()
        {
            ConversionModule<ResourceType, IResource> conversionModule = new ConversionModule<ResourceType, IResource>(
                CreateResource,
                DestroyRes);
        }

        public IResource CreateResource(ResourceType type) => default;
        public void DestroyRes(IResource res) { } 
        public void HandleRes(IResource res) { }
    }

    public enum ResourceType
    {
        Source = 0,
        Result = 1
    }

    public interface IResource
    {
        
    }
}