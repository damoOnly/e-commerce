using Ecdev.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
namespace Ecdev.Weixin.MP.Util
{
	public static class EntityHelper
	{
		public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : AbstractRequest, new()
		{
			T tEntity;
			if ((tEntity = entity) == null)
			{
				tEntity = Activator.CreateInstance<T>();
			}

			entity = tEntity;

			XElement root = doc.Root;
			PropertyInfo[] properties = entity.GetType().GetProperties();
			PropertyInfo[] array = properties;

			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				string propertyName = propertyInfo.Name;

				if (root.Element(propertyName) != null)
				{
					string propertyTypeName = propertyInfo.PropertyType.Name;

					if (propertyTypeName == null)
					{
                        propertyInfo.SetValue(entity, root.Element(propertyName).Value, null);
                        continue;
					}

					if (!(propertyTypeName == "DateTime"))
					{
						if (!(propertyTypeName == "Boolean"))
						{
							if (!(propertyTypeName == "Int64"))
							{
								if (!(propertyTypeName == "RequestEventType"))
								{
									if (!(propertyTypeName == "RequestMsgType"))
									{
                                        propertyInfo.SetValue(entity, root.Element(propertyName).Value, null);
                                        continue;
									}
									propertyInfo.SetValue(entity, MsgTypeHelper.GetMsgType(root.Element(propertyName).Value), null);
								}
								else
								{
									propertyInfo.SetValue(entity, EventTypeHelper.GetEventType(root.Element(propertyName).Value), null);
								}
							}
							else
							{
								propertyInfo.SetValue(entity, long.Parse(root.Element(propertyName).Value), null);
							}
						}
						else
						{
							if (!(propertyName == "FuncFlag"))
							{
                                propertyInfo.SetValue(entity, root.Element(propertyName).Value, null);
                                continue;
							}
							propertyInfo.SetValue(entity, root.Element(propertyName).Value == "1", null);
						}
					}
					else
					{
						propertyInfo.SetValue(entity, new DateTime(long.Parse(root.Element(propertyName).Value)), null);
					}
				}
			}
		}
		public static XDocument ConvertEntityToXml<T>(T entity) where T : class, new()
		{
			T tEntity;

			if ((tEntity = entity) == null)
			{
				tEntity = Activator.CreateInstance<T>();
			}
			entity = tEntity;

			XDocument xDocument = new XDocument();
			xDocument.Add(new XElement("xml"));

			XElement root = xDocument.Root;

			List<string> @object = new string[]
			                            {
				                            "ToUserName",
				                            "FromUserName",
				                            "CreateTime",
				                            "MsgType",
				                            "Content",
				                            "ArticleCount",
				                            "Articles",
				                            "FuncFlag",
				                            "Title ",
				                            "Description ",
				                            "PicUrl",
				                            "Url"
			                            }.ToList<string>();

			Func<string, int> orderByPropName = new Func<string, int>(@object.IndexOf);
			
            List<PropertyInfo> list = (
				from p in entity.GetType().GetProperties()
				orderby orderByPropName(p.Name)
				select p).ToList<PropertyInfo>();

			foreach (PropertyInfo current in list)
			{
				string name = current.Name;
				if (name == "Articles")
				{
					XElement xElement = new XElement("Articles");
					List<Article> list2 = current.GetValue(entity, null) as List<Article>;
					foreach (Article current2 in list2)
					{
						IEnumerable<XElement> content = EntityHelper.ConvertEntityToXml<Article>(current2).Root.Elements();
						xElement.Add(new XElement("item", content));
					}
					root.Add(xElement);
				}
				else
				{
					string propertyTypeName = current.PropertyType.Name;
					if (propertyTypeName == null)
					{
                        root.Add(new XElement(name, current.GetValue(entity, null)));
                        continue;
					}
					if (!(propertyTypeName == "String"))
					{
						if (!(propertyTypeName == "DateTime"))
						{
							if (!(propertyTypeName == "Boolean"))
							{
								if (!(propertyTypeName == "ResponseMsgType"))
								{
									if (!(propertyTypeName == "Article"))
									{
                                        root.Add(new XElement(name, current.GetValue(entity, null)));
                                        continue;
									}
									root.Add(new XElement(name, current.GetValue(entity, null).ToString().ToLower()));
								}
								else
								{
									root.Add(new XElement(name, current.GetValue(entity, null).ToString().ToLower()));
								}
							}
							else
							{
								if (!(name == "FuncFlag"))
								{
                                    root.Add(new XElement(name, current.GetValue(entity, null)));
                                    continue;
								}
								root.Add(new XElement(name, ((bool)current.GetValue(entity, null)) ? "1" : "0"));
							}
						}
						else
						{
							root.Add(new XElement(name, ((DateTime)current.GetValue(entity, null)).Ticks));
						}
					}
					else
					{
						root.Add(new XElement(name, new XCData((current.GetValue(entity, null) as string) ?? "")));
					}
				}
			}

			return xDocument;
		}
	}
}
