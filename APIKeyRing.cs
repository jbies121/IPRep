
namespace IPRep
{
    internal class APIKeyRing
	{
        internal APIKeyRing()
		{
            AIPDBKey = System.Environment.GetEnvironmentVariable("AIPDBKey");
			//If AIPDBKey env var doesn't exist, prompt user to set it.
		}

		internal string AIPDBKey { get; }
	}
}