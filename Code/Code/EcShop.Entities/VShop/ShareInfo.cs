using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.VShop
{
    public class ShareInfo
    {
        public int ShareId
        {
            get;
            set;
        }
        
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public System.DateTime ShareTime
        {
            get;
            set;
        }


        /// <summary>
        /// ������� Ĭ��Ϊ0
        /// </summary>
        public int ShareTimes
        {
            get;
            set;
        }

        /// <summary>
        /// ������
        /// </summary>
        public string SharePerson
        {
            get;
            set;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string ShareContent
        {
            get;
            set;
        }

        //��ǰ����
        public string Link
        {
            get;
            set;
        }

        /// <summary>
        /// �������ͣ�1����������Ȧ 2����������� 3������QQ  4������΢�� 5������QQ�ռ�
        /// </summary>
        public int ShareType
        {
            get;
            set;
        }


        /// <summary>
        /// ��ƷId
        /// </summary>
        public int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// ����Id 
        /// </summary>
        public string OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// �������û�ID
        /// </summary>
        public int ShareUserId
        {
            get;
            set;
        }

    }
}
