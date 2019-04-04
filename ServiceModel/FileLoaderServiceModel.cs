namespace ServiceModel
{
    public class FileLoaderServiceModel : ServiceModelBase
    {
        public string FilePath
        {
            get;
            set;
        }
        
        public bool Verbose
        {
            get;
            set;
        }
        
        public string ScopeName
        {
            get;
            set;
        }
    }
}
