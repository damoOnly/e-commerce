using System;
namespace EcShop.Entities
{
	public class AppVersionRecordInfo
	{
		public int Id
		{
			get;
			set;
		}
		public string Device
		{
			get;
			set;
		}
		public decimal Version
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public bool IsForcibleUpgrade
		{
			get;
			set;
		}
		public string UpgradeUrl
		{
			get;
			set;
		}
        public string UpgradeInfoUrl
        {
            get;
            set;
        }

        public string VersionName
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            set;
        }

        public string AppPackageName
        {
            get;
            set;
        }

	}
}
