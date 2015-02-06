namespace SDK.Common
{
    public class FactoryBuild : IFactoryBuild
    {
        public IByteArray buildByteArray()
        {
            return new ByteArray();
        }
    }
}