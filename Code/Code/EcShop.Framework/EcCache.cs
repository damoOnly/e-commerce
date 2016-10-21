using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace EcShop.Framework
{
	public sealed class EcCache
	{
		public const int DayFactor = 17280;
		public const int HourFactor = 720;
		public const int MinuteFactor = 12;
		public const double SecondFactor = 0.2;
		private static Cache _cache;
		private static int Factor;
		private EcCache()
		{
		}
		public static void ReSetFactor(int cacheFactor)
		{
			EcCache.Factor = cacheFactor;
		}
		static EcCache()
		{
			EcCache.Factor = 5;
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				EcCache._cache = current.Cache;
			}
			else
			{
				EcCache._cache = HttpRuntime.Cache;
			}
		}
		public static void Clear()
		{
			IDictionaryEnumerator enumerator = EcCache._cache.GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				arrayList.Add(enumerator.Key);
			}
			foreach (string key in arrayList)
			{
				EcCache._cache.Remove(key);
			}
		}
		public static void RemoveByPattern(string pattern)
		{
			IDictionaryEnumerator enumerator = EcCache._cache.GetEnumerator();
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			while (enumerator.MoveNext())
			{
				if (regex.IsMatch(enumerator.Key.ToString()))
				{
					EcCache._cache.Remove(enumerator.Key.ToString());
				}
			}
		}
		public static void Remove(string key)
		{
			EcCache._cache.Remove(key);
		}
		public static void Insert(string key, object obj)
		{
			EcCache.Insert(key, obj, null, 1);
		}
		public static void Insert(string key, object obj, CacheDependency dep)
		{
			EcCache.Insert(key, obj, dep, 8640);
		}
		public static void Insert(string key, object obj, int seconds)
		{
			EcCache.Insert(key, obj, null, seconds);
		}
		public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
		{
			EcCache.Insert(key, obj, null, seconds, priority);
		}
		public static void Insert(string key, object obj, CacheDependency dep, int seconds)
		{
			EcCache.Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
		}
		public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
		{
			if (obj != null)
			{
				EcCache._cache.Insert(key, obj, dep, DateTime.Now.AddSeconds((double)(EcCache.Factor * seconds)), TimeSpan.Zero, priority, null);
			}
		}
		public static void MicroInsert(string key, object obj, int secondFactor)
		{
			if (obj != null)
			{
				EcCache._cache.Insert(key, obj, null, DateTime.Now.AddSeconds((double)(EcCache.Factor * secondFactor)), TimeSpan.Zero);
			}
		}
		public static void Max(string key, object obj)
		{
			EcCache.Max(key, obj, null);
		}
		public static void Max(string key, object obj, CacheDependency dep)
		{
			if (obj != null)
			{
				EcCache._cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
			}
		}
		public static object Get(string key)
		{
			return EcCache._cache[key];
		}
		public static int SecondFactorCalculate(int seconds)
		{
			return Convert.ToInt32(Math.Round((double)seconds * 0.2));
		}
	}
}
