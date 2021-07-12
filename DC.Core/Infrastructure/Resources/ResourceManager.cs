using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace DC.Core.Infrastructure.Resources
{
    public sealed class ResourceManager
    {
		private string ResourceNamespace { get; set; }

		private Assembly ResourceAssembly { get; }

		public ResourceManager(string resourceNamespace, Assembly resourceAssembly)
		{
			ResourceNamespace = resourceNamespace ?? throw new ArgumentNullException(nameof(resourceNamespace));
			ResourceAssembly = resourceAssembly ?? throw new ArgumentNullException(nameof(resourceAssembly));
		}

		public string GetString(string resourceName)
		{
			if (string.IsNullOrEmpty(resourceName))
				throw new ArgumentNullException(nameof(resourceName));

			using (Stream resourceStream = ResourceAssembly.GetManifestResourceStream($"{ResourceNamespace}.{resourceName}"))
			{
				if (resourceStream is null)
					throw new Exception($"{ResourceNamespace}.{resourceName}");
				//throw new ResourceNotFoundException($"{ResourceNamespace}.{resourceName}");

				using (StreamReader reader = new StreamReader(resourceStream, Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
