<%@ Page Language="C#"%>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.AccountCenter.CodeBehind" Assembly="EcShop.UI.AccountCenter.CodeBehind" %>
 <Hi:MyChangeCoupons id="MyChangeCoupons" runat="server"></Hi:MyChangeCoupons>
<script>
//左侧导航start
$(function(){
	function setHeight(){
		height = $('.my_left_category').height();
		$('.shadow_border').height(height+2);
	};
	$('.my_left_category>div').filter('.my_left_category>div:gt(6)').hide ();
	$('.my_left_category>div').filter ('.my_left_category>div:nth-child(14)').mouseover(function(){
		$(this).siblings ('.my_left_category>div:gt(6)').show();
		setHeight();
	})
	$('.my_left_category>div').filter ('.my_left_category>div:nth-child(14)').mouseout(function(){
		$(this).siblings ('.my_left_category>div:gt(6)').hide();
		setHeight();
	})
	$('.my_left_category>div').filter ('.my_left_category>div:nth-child(14)').siblings ('.my_left_category>div:gt(6)').mouseover(function(){
		$('.my_left_category>div').filter ('.my_left_category>div:nth-child(6)').siblings ('.my_left_category>div:gt(6)').show();
		setHeight();
	})
	$('.my_left_category>div').filter ('.my_left_category>div:nth-child(14)').siblings ('.my_left_category>div:gt(6)').mouseout(function(){
		$('.my_left_category>div').filter ('.my_left_category>div:nth-child(14)').siblings ('.my_left_category>div:gt(6)').hide();
		setHeight();
	})
});
//左侧导航over
</script>

