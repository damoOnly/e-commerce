using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.SqlDal.Comments;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.ControlPanel.Comments
{
	public sealed class ProductCommentHelper
	{
		private ProductCommentHelper()
		{
		}
		public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			return new ProductConsultationDao().GetConsultationProducts(consultationQuery);
		}
		public static ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			return new ProductConsultationDao().GetProductConsultation(consultationId);
		}
		public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			return new ProductConsultationDao().ReplyProductConsultation(productConsultation);
		}
		public static int DeleteProductConsultation(int consultationId)
		{
			return new ProductConsultationDao().DeleteProductConsultation(consultationId);
		}
		public static int DeleteReview(IList<int> reviews)
		{
			int result;
			if (reviews == null || reviews.Count == 0)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				ProductReviewDao productReviewDao = new ProductReviewDao();
				foreach (int current in reviews)
				{
					num++;
					productReviewDao.DeleteProductReview((long)current);
				}
				result = num;
			}
			return result;
		}
		public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			return new ProductReviewDao().GetProductReviews(reviewQuery);
		}
		public static ProductReviewInfo GetProductReview(int reviewId)
		{
			return new ProductReviewDao().GetProductReview(reviewId);
		}
		public static int DeleteProductReview(long reviewId)
		{
			return new ProductReviewDao().DeleteProductReview(reviewId);
		}


        public static IList<ProductReviewInfo> GetSimpleProductReviews(int productId)
        {
            return new ProductReviewDao().GetSimpleProductReviews(productId);
        }

        public static DataTable GetReviewsCountAndAvg(int productId)
        {
            return new ProductReviewDao().GetReviewsCountAndAvg(productId);
        }

        public static ProductReviewInfo GetProductReview(int productId, string skuId, string orderId, int userId)
        {
            return new ProductReviewDao().GetProductReview(productId, skuId, orderId, userId);
        }
	}
}
