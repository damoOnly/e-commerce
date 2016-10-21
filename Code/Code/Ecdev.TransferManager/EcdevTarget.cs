using System;
namespace Ecdev.TransferManager
{
	public class EcdevTarget : Target
	{
		public const string TargetName = "Ecdev";
        public EcdevTarget(Version version)
            : base("Ecdev", version)
		{
		}
        public EcdevTarget(string versionString)
            : base("Ecdev", versionString)
		{
		}
        public EcdevTarget(int major, int minor, int build)
            : base("Ecdev", major, minor, build)
		{
		}
	}
}
