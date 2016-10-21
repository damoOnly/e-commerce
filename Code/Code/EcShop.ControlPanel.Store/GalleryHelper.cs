using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.ControlPanel.Store
{
	public static class GalleryHelper
	{
		public static bool AddPhotoCategory(string name)
		{
			return new PhotoGalleryDao().AddPhotoCategory(name);
		}
		public static int MovePhotoType(List<int> pList, int pTypeId)
		{
			return new PhotoGalleryDao().MovePhotoType(pList, pTypeId);
		}
		public static int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			return new PhotoGalleryDao().UpdatePhotoCategories(photoCategorys);
		}
		public static bool DeletePhotoCategory(int categoryId)
		{
			return new PhotoGalleryDao().DeletePhotoCategory(categoryId);
		}
		public static System.Data.DataTable GetPhotoCategories()
		{
			return new PhotoGalleryDao().GetPhotoCategories();
		}
		public static void SwapSequence(int categoryId1, int categoryId2)
		{
			new PhotoGalleryDao().SwapSequence(categoryId1, categoryId2);
		}
		public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, PhotoListOrder order)
		{
			Pagination pagination = new Pagination();
			pagination.PageSize = 20;
			pagination.PageIndex = pageIndex;
			pagination.IsCount = true;
			switch (order)
			{
			case PhotoListOrder.UploadTimeDesc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UploadTimeAsc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.NameAsc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.NameDesc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UpdateTimeDesc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UpdateTimeAsc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.SizeDesc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.SizeAsc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Asc;
				break;
			}
			return new PhotoGalleryDao().GetPhotoList(keyword, categoryId, pagination);
		}
		public static bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
		{
			return new PhotoGalleryDao().AddPhote(categoryId, photoName, photoPath, fileSize);
		}
		public static bool DeletePhoto(int photoId)
		{
			return new PhotoGalleryDao().DeletePhoto(photoId);
		}
		public static void RenamePhoto(int photoId, string newName)
		{
			new PhotoGalleryDao().RenamePhoto(photoId, newName);
		}
		public static void ReplacePhoto(int photoId, int fileSize)
		{
			new PhotoGalleryDao().ReplacePhoto(photoId, fileSize);
		}
		public static string GetPhotoPath(int photoId)
		{
			return new PhotoGalleryDao().GetPhotoPath(photoId);
		}
		public static int GetPhotoCount()
		{
			return new PhotoGalleryDao().GetPhotoCount();
		}
		public static int GetDefaultPhotoCount()
		{
			return new PhotoGalleryDao().GetDefaultPhotoCount();
		}
	}
}
