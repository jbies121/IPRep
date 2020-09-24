using System;

namespace IPRep
{
    internal class APIKeyRing
	{
		internal string AIPDBKey { get; }
		internal APIKeyRing()
		{
            AIPDBKey = Environment.GetEnvironmentVariable("AIPDBKey", EnvironmentVariableTarget.User);
			if (AIPDBKey == null)
            {
				string env = "AIPDBKey";
				//If AIPDBKey env var doesn't exist, prompt user to set it.
				Console.WriteLine("Windows Users only: This key will be stored as AIPDBKey in your user environment variables.");
				Console.WriteLine("Enter an Abuse IPDB API key:");
				AIPDBKey = Console.ReadLine();
                Environment.SetEnvironmentVariable(env, AIPDBKey, EnvironmentVariableTarget.User);
			}
		}
	}
}