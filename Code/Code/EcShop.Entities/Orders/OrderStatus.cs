using System;
namespace EcShop.Entities.Orders
{
	public enum OrderStatus
	{
		All,
        WaitBuyerPay, //�ȴ���Ҹ��� 1
        BuyerAlreadyPaid,//�Ѹ���,�ȴ����� 2
        SellerAlreadySent,//�ѷ���  3
        Closed,//�ѹر� 
        Finished,//��������� 5
        ApplyForRefund,//�����˿� 6 
        ApplyForReturns,//�����˻� 7
        ApplyForReplacement,//���뻻�� 8
        Refunded,//���˿� 9 
        Returned,//���˻� 10
        UnpackOrMixed= 98,//��ֻ����Ѿ��ϲ���ԭ��
        History = 99 //��ʷ����
	}

    public enum OrderHandleReasonType
    {
        /// <summary>
        /// �˿�
        /// </summary>
        Refund,
        /// <summary>
        /// �˻�
        /// </summary>
        Return
    }

    /// <summary>
    /// �����ر�����
    /// </summary>
    public enum CloseOrderType
    {
        /// <summary>
        /// ϵͳ�Զ��رգ�����δ���
        /// </summary>
        Auto,
        /// <summary>
        /// �û��ֶ��ر�
        /// </summary>
        Manually
    }

    /// <summary>
    /// �������ضԽ�״̬
    /// </summary>
    public enum HSStatus
    {
        /// <summary>
        /// δ��ʼ
        /// </summary>
        NotStarted,
        /// <summary>
        /// ������
        /// </summary>
        Underway,
        /// <summary>
        /// �����
        /// </summary>
        Accomplish,
        /// <summary>
        /// ʧ��
        /// </summary>
        BeDefeated,
        /// <summary>
        /// �ȴ�����
        /// </summary>
        WaitingForRetry
    }
}
