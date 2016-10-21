// JavaScript Document
//保存 cookies
function setMyCookie(key,value){
	if(arguments.length == 1){
		  if(window.localStorage){
			  return localStorage[key];
		  }else{
			  var objValue = null;
			  var arrStr = document.cookie.split("; ");
			  for (var i = 0; i < arrStr.length; i++) {
				var temp = arrStr[i].split("=");
				if (temp[0] == key) {
					objValue = unescape(temp[1]);
					break;
				}
			  }
			  return objValue;
		  }
	  }else{
		 if(window.localStorage){
			 if(typeof value != "string"){
				 value = JSON.stringify(value);
			 }
			 localStorage[key] = value;
		 }else{			 
			var str = key + "=" + escape(value);
			var expires = 0;
			if (expires > 0) {//为0时不设定过期时间，浏览器关闭时cookie自动消失
				var date = new Date();
				var ms = expires * 3600 * 1000;
				date.setTime(date.getTime() + ms);
				str += "; expires=" + date.toGMTString();
			}
			document.cookie = str; 
		 } 
	}
}
function removeViewDom(view){
	if(!view){
		return;
	}
	if(view.$field){
		view.$field = null;
	}
	if(view.$el){
		view.$el = null;
	}
	if(view.el){
		view.el = null;
	}
	if(view.afView){
		view.afView=null;
	}
	return view;
}
function formatView(list){
	for(var i=0;i<list.length;i++){
		removeViewDom(list[i]);
	}
}
var viewList;
function arrayRemove(arr,obj){
	for(var i=0;i<arr.length;i++){
		if(arr[i].cid == obj.cid){
			arr.splice(i,1);
			break;
		}
	}
}
$(function(){	
	//懒加载 透明图片
	var lazyloadImg = "resource/default/image/lazy.png";
	//商品
	var goodModel =  Backbone.Model.extend({
	    defaults : {
			gid:"",
			title:'此处显示商品名称',
			alink:"",
			date:"",
			rate:'5%',
			price:'888.00',
			img:'resource/default/image/temp/g1.jpg'
		},	
		initialize: function(){
			
		}
	});
	var collection = Backbone.Collection.extend({
		//World对象的集合
		initialize: function (models, options) {
				
		}
	});
	//商品 面板
	var goodPanelModel = Backbone.Model.extend({				
		initialize: function(){	
			
		},
		defaults:{
			goodList:null,
			size:2,
			type:0,
			showBuyBtn:true,
			buyBtnStyle:1,
			showTitle:true,
			showIntr:true,
			showPrice:true,
		}
	});
	var delView = Backbone.View.extend({				
		initialize: function(){	
			
		},
		render: function(context) {
			//使用underscore这个库，来编译模板
			var template = _.template($("#pop_template").html(), context);
			//加载模板到对应的el属性中			
			$(this.el).html(template);
			this.ele = context.ele;
			var position = $(this.ele).show().offset();
			var $pop = $(this.el).find(".popover").show();
			var width = $pop.outerWidth(true);
			var height = $pop.outerHeight(true);			
			var pos = {
				left:position.left - width,
				top:position.top - height/2
			}
			$pop.css(pos);
			this.bindEvents();			
		},
		bindEvents:function(){
			var self = this;
			$(this.el).find(".js-btn-confirm").off("click").on("click",function(e){
				var ele = this;
				self.confirmFn(e,self,ele);
			});			
			$(this.el).find(".js-btn-cancel").off("click").on("click",function(e){
				var ele = this;
				self.cancel(e,self);
			});
		},		
		confirmFn: function(e,self){
			var $field = $(self.ele).closest(".app-field");
			var $preField = $field.prev(".app-field");			
			$(self.el).find(".popover").hide().html("");
			var view = $(self.el).data("view");
			//viewList.remove(view);
			arrayRemove(viewList,view);
			view.$field.remove();
			var $sidebar = $(".app-sidebar");
			var position = {top:0}
			if($preField.length){
				$preField.find(".action.edit").click();
			}else{
				$sidebar.css({"margin-top":0});					
				var afView = new addFieldView({el: $(".js-sidebar-region div")});
			}
		},
		cancel: function(e,self){
			$(self.el).find(".popover").hide().html("");
		}
	});
	var delPop;
	var fieldView = Backbone.View.extend({				
		initialize: function(option){	
			if(option.sidebar){
				this.sidebar = option.sidebar;
			}			
		},
		render: function(context) {
		},
		setSideBarPos:function(ele){
			var position = $(ele).closest(".app-field").position();
			$(".app-sidebar").css({"margin-top":position.top});
		},
		add: function(e,ele){
			$(this.el).find(".editing").removeClass("editing");
			this.$field.addClass("editing");
			this.setSideBarPos(ele);			
			this.afView = new addFieldView({el: $(".js-sidebar-region div")});			
		},		
		del: function(e,ele){			
			delPop = new delView({el:$("#pop-con")});
			delPop.render({ele:ele});
			$(delPop.el).data("view",this);
		},		
		edit: function(e,ele){
			this.setSideBarPos(ele);
			$(this.el).find(".editing").removeClass("editing");
			this.$field.addClass("editing");
			this.buildToolPanel();
		},
		getHeaderHtml:function(){
			var str = '<div class="app-field clearfix editing"><div class="control-group">';
			return str;
		},
		getFooterHtml:function(){
			var str = '<div class="component-border"></div>';
				str += '</div>';
				str += '<div class="actions">';
				str += '<div class="actions-wrap"><span class="action edit">编辑</span><span class="action add">加内容</span><span class="action delete">删除</span></div></div>';
				str += '<div class="sort"><i class="sort-handler"></i></div>';
				str += '</div>';
			return str;
		}
	});
	var goodFieldView = fieldView.extend({
		render: function(context) {
			this.proto = "goodFieldView";
			this.model = context.model;			
			this.buildTem();			
		},		
		buildTem:function(isReplace){
			this.buildGoods(isReplace);
			this.buildToolPanel();
		},
		buildToolPanel:function(){			
			var str = '';
			str += '<div class="control-group">';
			str += '<label class="control-label">选择商品：</label>';
        	str += '<div class="controls"><ul class="module-goods-list clearfix ui-sortable" name="goods">';
			var self = this;
            var model = self.model;
			var data = model.get("goodList");
			if(data && data.models){
				for(var i=0;i<data.models.length;i++){
					var obj = JSON.parse(JSON.stringify(data.models[i]));
					str += '<li class="sort"><a href="'+obj.alink+'" target="_blank">';
					str += '<img src="'+obj.img+'" alt="'+obj.title+'" width="50" height="50"></a>';
					str += '<a class="close-modal js-delete-goods small" data-id="'+obj.gid+'" title="删除">×</a></li>';
				}
			}
			
            str += '<li><a class="js-add-goods add-goods" data-toggle="modal" data-target="#proModal"><i class="icon-add"></i></a></li>';
            str += '</ul></div></div>';
			/*规格*/			
			str += '<div class="js-goods-style-region" style="margin-top: 10px;"><div>';
			str += '<div class="control-group"><label class="control-label">列表样式：</label>';
        	str += '<div class="controls">';
			var size = model.get("size");
            str += '<label class="radio inline"><input type="radio" name="size" value="0" '+(size == 0?"checked=true":"")+'>大图</label>';
            str += '<label class="radio inline"><input type="radio" name="size" value="1" '+(size == 1?"checked=true":"")+'>小图</label>';
            str += '<label class="radio inline"><input type="radio" name="size" value="2" '+(size == 2?"checked=true":"")+'>一大两小</label>';
            str += '<label class="radio inline"><input type="radio" name="size" value="3" '+(size == 3?"checked=true":"")+'>详细列表</label>';
			str += '</div></div>';
			var type = model.get("type");
    		str += '<div class="control-group"><div class="controls"><div class="controls-card">';
			str += '<div class="controls-card-tab">';
            str += '<label class="radio inline"><input type="radio" name="size_type" value="0" '+(type == 0?"checked=true":"")+'>卡片样式</label>';
            str += '<label class="radio inline"><input type="radio" name="size_type" value="2" '+(type == 2?"checked=true":"")+'>极简样式</label>';
            str += '</div><div class="controls-card-item"><div>';
			str += '<label class="checkbox inline"><input type="checkbox" name="buy_btn" value="1" '+(model.get("showBuyBtn")?"checked=true":"")+'>显示购买按钮</label>';
            str += '</div>';
            var buyBtnStyle = model.get("buyBtnStyle");
            str += '<div style="margin: 10px 0 0 20px; '+(type == 2?"display:none":"")+'" >';
			str += '<label class="radio inline"><input type="radio" name="buy_btn_type" value="1" '+(buyBtnStyle == 1?"checked=true":"")+'>样式1</label>';
            str += '<label class="radio inline"><input type="radio" name="buy_btn_type" value="2" '+(buyBtnStyle == 2?"checked=true":"")+'>样式2</label>';
            str += '<label class="radio inline"><input type="radio" name="buy_btn_type" value="3" '+(buyBtnStyle == 3?"checked=true":"")+'>样式3</label>';
            str += '<label class="radio inline"><input type="radio" name="buy_btn_type" value="4" '+(buyBtnStyle == 4?"checked=true":"")+'>样式4</label>';
            str += '</div></div>';
                
            str += ' <div class="controls-card-item">';
			str += '<label class="checkbox inline"><input type="checkbox" name="title" value="1" '+(model.get("showTitle")?"checked=true":"")+'>显示商品名</label></div>';
            str += '<div class="controls-card-item"><label class="checkbox inline"><input type="checkbox" name="price" value="1" '+(model.get("showPrice")?"checked=true":"")+'>显示价格</label></div>';
			str += '<div class="controls-card-item"><label class="checkbox inline"><input type="checkbox" name="intr" value="1" '+(model.get("showIntr")?"checked=true":"")+'>显示税率</label></div>';
           str += '</div></div></div></div></div>';
		   
		   var $sidebar = $(".app-sidebar-inner");
		   $sidebar.find(">div").html(str);		   
		   //面板 事件
			/*defaults:{
				goodList:null,
				size:2,
				type:0,
				showBuyBtn:true,
				buyBtnStyle:1,
				showTitle:true,
				showIntr:false,
				showPrice:true,
			}*/
			// 类型
			$sidebar.find("input[name=size]").off("change").on("change",function(e){
				var ele = this;
				var val = parseInt($(this).val());
				self.model.set({size:val});
				self.buildGoods(true);
			});
			//卡片 极简
			$sidebar.find("input[name=size_type]").off("change").on("change",function(e){
				var ele = this;
				var val = parseInt($(this).val());
				self.model.set({type:val});
				var $item = $(ele).closest(".controls-card-tab").next(".controls-card-item");
				if(val == 2){
					self.model.set({showBuyBtn:false});
					$item.hide();
				}else{
					self.model.set({showBuyBtn:true});
					$item.show();
				}				
				self.buildGoods(true);
			});
			//是否显示 购买按钮
			$sidebar.find("input[name=buy_btn]").off("change").on("change",function(e){
				var ele = this;
				checked = ele.checked;
				self.model.set({showBuyBtn:checked});				
				self.buildGoods(true);
			});
			//购买 按钮 样式
			$sidebar.find("input[name=buy_btn_type]").off("change").on("change",function(e){
				var ele = this;
				var val = parseInt($(this).val());
				self.model.set({buyBtnStyle:val});				
				self.buildGoods(true);
			});
			//是否显示 标题
			$sidebar.find("input[name=title]").off("change").on("change",function(e){
				var ele = this;
				checked = ele.checked;
				self.model.set({showTitle:checked});			
				self.buildGoods(true);
			});
			//是否显示 税率
			$sidebar.find("input[name=intr]").off("change").on("change",function(e){
				var ele = this;
				checked = ele.checked;
				self.model.set({showIntr:checked});			
				self.buildGoods(true);
			});
			//是否显示价格
			$sidebar.find("input[name=price]").off("change").on("change",function(e){
				var ele = this;
				checked = ele.checked;
				self.model.set({showPrice:checked});			
				self.buildGoods(true);
			});
			//点击 商品添加 按钮
			$sidebar.find("a.add-goods").off("click").on("click",function(){				
				if(profv){
					profv.remove();
				}
				profv = new proFieldView;
				profv.render({view:self});
			});	
			//点击 商品添加 按钮
			$sidebar.find("a.js-delete-goods").off("click").on("click",function(){				
				var ele = this;
				var gid = $(this).attr("data-id");
				var m = data.where({"gid":gid});
				data.remove(m);							
				self.buildTem(true);
			});	
		},
		buildGoods:function(isReplace){
			var self = this;
			var str = self.getHeaderHtml();			
			var model = self.model;
			var data = model.get("goodList");			
			if(!data || data.length == 0){
				var option = {
					gid:"",
					title:'商品名称',
					alink:"",
					date:"",
					rate:'0%',
					price:'888.00',
					img:'resource/default/image/temp/g4.jpg'
				}
				var g1 = new goodModel(option);
				option.gid = "";
				var g2 = new goodModel(option);
				option.gid = "";
				var g3 = new goodModel(option);
				var goodList = new collection;
				goodList.add(g1);
				goodList.add(g2);
				goodList.add(g3);
				data = goodList;
			}						
			var size = model.get("size");
			var type = model.get("type");
			str += '<div class="clumn"><div class="clumn-con"><ul class="theme '+(size == 3?"theme2 ":" ")+(type == 2?"min-theme":"")+'  clearfix">';
			for(var i=0;i<data.models.length;i++){
				var obj = JSON.parse(JSON.stringify(data.models[i]));
				var liStyle = "";
				var aStyle = "";
				if(size == 0){
					liStyle = "w100";
				}else if(size == 1){
					if(i%2 == 0){
						aStyle = "mr5";
					}else{
						aStyle = "ml5";
					}
				}else if(size == 2){
					if(i%3 == 0){
						liStyle = "w100";
					}else if(i%3 == 1){
						aStyle = "mr5";
					}else{
						aStyle = "ml5";
					}
				}else if(size == 3){
					
				}
				var href = obj.alink;
				if(!href || href == "#"){
					href = "javascript:void(0)";
				}
				str += '<li class="'+liStyle+'"><div class="good-wrap '+aStyle+'" > <a href="'+href+'">';
                str += '<div class="good-img"> <img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></div>';
                str += '<div class="good-detail">';
				if(model.get("showTitle")){
					str += '<p class="good-title">'+obj.title+'</p>';
				}
                str += '<div class="good-info">';
				if(model.get("showPrice")){
					str += '<p class="good-price">￥<strong>'+obj.price+'</strong></p>';
				}
				if(model.get("showIntr")){
               		str += '<p class="rate">税率<span>'+obj.rate+'</span></p>';
				}
				str += '</div></div></a>';
				if(model.get("showBuyBtn")){
					str += '<a class="fast-add show-skus add-cart cart-'+(model.get("buyBtnStyle"))+'" title="加入购物车" smallImg="'+obj.img+'" fastbuy_skuid="'+obj["fastbuy_skuid"]+'" productid="'+obj.gid+'" price='+obj.price+'></a>';
				}
				str += '</div></li>';			
			}		
			str += '</ul></div></div>';	
			/**/
			str += this.getFooterHtml();
			if(!isReplace){
				var $str;
				if(this.sidebar){
					var $editingField = $(this.el).find(".editing");
					$str = $(str);
					if($editingField.length){						
						$editingField.after($str);
					}else{
						$str = $(str).appendTo($(this.el));
					}					
					this.$field = $str;
				}else{
					$str = this.$field = $(str).appendTo($(this.el));
				}
				var position = $str.position();
				$(".app-sidebar").css({"margin-top":position.top});
				$(this.el).find(".editing").removeClass("editing");
				this.$field.addClass("editing");
				//保存 对象
				$str.data("view",self);				
			}else{
				self.$field.html($(str).html());
			}
			//绑定事件			
			$(self.el).find(".action.add").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.add(e,ele);
			});
			$(self.el).find(".action.edit").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.edit(e,ele);
			});
			$(self.el).find(".action.delete").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");				
				view.del(e,ele);
			});
		}
	});
	
	//广告对象
	var bannerModel =  Backbone.Model.extend({
	    defaults : {
			bid:"",			
			title:'此处显示广告名称',
			linktype:0,
			selectedIndex:0,
			linklist:[
				{title:"首页",value:"/"},
				{title:"类别",value:"/"},
				{title:"详情页",value:"/"}
			],
			alink:'',			
			img:'resource/default/image/temp/b1.jpg',
			showTitle:false, //是否 显示标题框
			imgSize:"320*320" //图片规格
		},	
		initialize: function(){
			
		}
	});	
	//广告 面板
	var bannerPanelModel = Backbone.Model.extend({				
		initialize: function(){	
			
		},
		defaults:{
			bannerList:null,			
			type:0,
			size:1,
			showTitle:true
		}
	});	
	var adView = Backbone.View.extend({	
		tagName:"li",
		className:"choice",			
		initialize: function(option){
			this.option = option;	
			this.render(option);
		},
		render: function(context) {
			var view = context.view;
			var data = view.model.get("bannerList");
			var obj = JSON.parse(JSON.stringify(data.models[context.index]));
			context.container.append($(this.el));
			this.buildHtml(obj,context.index);
		},
		events:{  
			'click .delete' : 'delete',
			'click .add' : 'add',
			'change .linktype' : 'change',
			'change .sel':'selectChange',
			'keyup input[name=titleinput]':'titleChange',
			'keyup input[name=linkinput]':'linkChange'
		},
		buildHtml:function(obj,index){
			var data = this.option.view.model.get("bannerList");
			var m = data.models[index];
			str = '';
			str += '<div class="choice-image"><img src="'+obj.img+'" class="thumb-image">';
			/*str += '<a class="modify-image js-trigger-image" href="javascript: void(0);">重新上传</a>';*/
			str += '<div class="control-group"><div class="controls"><input type="hidden" name="image_url"></div>';
			str += '</div></div>';
			str += '<div class="choice-content">';
			//showTitle:false, //是否 显示标题框
			//if(obj.showTitle){
				str += '<div class="control-group"><label class="control-label">文字：</label><div class="controls">';
				var val = obj.title;
				if(!val || val == "名称"){
					val = "";
				}
				str += '<input type="text" name="titleinput" placeholder="名称" value="'+val+'" readonly="true"/>';
				str += '</div></div>';
			//}			
			/*str += '<div class="control-group"><label class="control-label">链接类型：</label><div class="controls">';
			str += '<label class="radio inline"><input type="radio" class="linktype" name="linktype_'+m.cid+'" value="0" '+(obj.linktype == 0?"checked=checked":"")+'>内链接</label>';
			str += '<label class="radio inline"><input type="radio" class="linktype" name="linktype_'+m.cid+'" value="1" '+(obj.linktype == 1?"checked=checked":"")+'>自定义链接</label>';
			str += '</div></div>';*/
			str += '<div class="control-group"><label class="control-label">链接地址：</label>';
			var linkinputVal = obj.alink;
			/*if(obj.linktype == 1){
				linkinputVal = obj.alink;
			}*/
			str += '<div class="controls" name="link-ctrl" ><input type="text" name="linkinput" value="'+linkinputVal+'" readonly="true"/></div>';
			
			/*str += '<div class="controls '+(obj.linktype == 0?"":"dis-none")+'" name="sel-ctrl"><select class="sel" name="link_sel_'+m.cid+'">';
			for(var i=0;i<obj.linklist.length;i++){
				var lo = obj.linklist[i];
				str += '<option value="'+lo.value+'" '+(i == obj.selectedIndex?"selected":"")+'>'+lo.title+'</option>';
			}
			str += '</select></div>';	*/	
			
			str += '</div></div>';			
			str += '<div class="actions"><span class="action add close-modal" title="添加">+</span><span class="action delete close-modal" title="删除">×</span></div>';
			
			$(this.el).html(str);
			$(this.el).attr("data-index",index);
		},		
		change:function(e){
			var data = this.option.view.model.get("bannerList");
			var m = data.models[this.option.index];
			
			var $ele = $(e.target);
			var val = $ele.val();
			var $group = $ele.closest(".control-group").next(".control-group");
			var $linkCtrl = $group.find("[name=link-ctrl]");
			var $selCtrl = $group.find("[name=sel-ctrl]");
			
			if(val == 0){//内链接
				$linkCtrl.hide();
				$selCtrl.show();
				m.set({alink:$selCtrl.find("select").val(),linktype:0});
			}else{
				$linkCtrl.show();
				$selCtrl.hide();
				m.set({alink:$linkCtrl.find("input").val(),linktype:1});
			}
			//重新绘制
			this.option.view.buildBanners(true);
		},
		selectChange:function(e){
			var data = this.option.view.model.get("bannerList");
			var m = data.models[this.option.index];
			
			var $ele = $(e.target);
			var val = $ele.val();
			var index = $ele.find("option:selected").index();
			m.set({alink:val,selectedIndex:index});
			//重新绘制
			this.option.view.buildBanners(true);
		},
		titleChange:function(e){
			var data = this.option.view.model.get("bannerList");
			var m = data.models[this.option.index];
			
			var $ele = $(e.target);
			var val = $ele.val();
			m.set({title:val});
			//重新绘制
			this.option.view.buildBanners(true);
		},
		linkChange:function(e){
			var data = this.option.view.model.get("bannerList");
			var m = data.models[this.option.index];
			
			var $ele = $(e.target);
			var val = $ele.val();
			m.set({alink:val});
			//重新绘制
			this.option.view.buildBanners(true);
		},
		delete:function(e){
			var data = this.option.view.model.get("bannerList");
			var m = data.models[this.option.index];
			data.remove(m);			
			//重新绘制
			this.option.view.buildTem(true);
		},
		buildAddHtml:function($ele){
			var view = this.option.view;
			var data = view.model.get("bannerList");
			var m = data.models[this.option.index];
			var obj = JSON.parse(JSON.stringify(data.models[this.option.index]));
			var str = '';
			str += '<li class="choice addchoice"><div class="choice-image">';    
        	str += '<a class="add-image js-trigger-image" data-toggle="modal" data-target="#testModal"><i class="icon-add"></i>添加图片</a>';
    		str += '<div class="control-group"><div class="controls"><input type="hidden" name="image_url"></div></div></div>';
			str += '<div class="choice-content">';
			
			
			//if(obj.showTitle){
				str += '<div class="control-group"><label class="control-label">文字：</label><div class="controls">';
				var val = obj.title;
				if(!val || val == "名称"){
					val = "";
				}
				str += '<input type="text" name="titleinput" placeholder="名称" value="'+val+'" readonly="true"/>';
				str += '</div></div>';
			//}			
			/*str += '<div class="control-group"><label class="control-label">链接类型：</label><div class="controls">';
			str += '<label class="radio inline"><input type="radio" class="linktype" name="linktype_'+m.cid+'" value="0" '+(obj.linktype == 0?"checked=checked":"")+'>内链接</label>';
			str += '<label class="radio inline"><input type="radio" class="linktype" name="linktype_'+m.cid+'" value="1" '+(obj.linktype == 1?"checked=checked":"")+'>自定义链接</label>';
			str += '</div></div>';*/
			str += '<div class="control-group"><label class="control-label">链接地址：</label>';
			
			str += '<div class="controls" name="link-ctrl" ><input type="text" name="linkinput" readonly="true"/></div>';
			
			/*str += '<div class="controls '+(obj.linktype == 0?"":"dis-none")+'" name="sel-ctrl"><select class="sel" name="link_sel_'+m.cid+'">';
			for(var i=0;i<obj.linklist.length;i++){
				var lo = obj.linklist[i];
				str += '<option value="'+lo.value+'" '+(i == obj.selectedIndex?"selected":"")+'>'+lo.title+'</option>';
			}
			str += '</select></div>';	*/	
			
			str += '</div></div>';
			str += '</li>';
			var $str = $(str);
			$ele.after($str);
			//链接 类型改变
			$str.find(".linktype").on("change",function(e){
				var $ele = $(e.target);
				var val = $ele.val();
				var $group = $ele.closest(".control-group").next(".control-group");
				var $linkCtrl = $group.find("[name=link-ctrl]");
				var $selCtrl = $group.find("[name=sel-ctrl]");
				if(val == 0){//内链接
					$linkCtrl.hide();
					$selCtrl.show();
					//m.set({alink:$selCtrl.find("select").val(),linktype:0});
				}else{
					$linkCtrl.show();
					$selCtrl.hide();
					//m.set({alink:$linkCtrl.find("input").val(),linktype:1});
				}
			})
			//添加 图片
			$str.find(".js-trigger-image").on("click",function(){
				var $ele = $(this);
				var liIndex = $ele.closest("li").prev(":not(.addchoice)").index();
				liIndex = liIndex + 1;
				var $content = $ele.parent().next(".choice-content");
				var title = $content.find("[name=titleinput]").val();
				/*var linktype = parseInt($content.find(".linktype:checked").val());*/
				var linktype = 0;
				var alink = $content.find("[name=link-ctrl] input").val();;
				var index = 0;				
				var b = new bannerModel({
					bid:"",			
					title:title,
					linktype:linktype,
					selectedIndex:index,
					/*linklist:[
						{title:"首页",value:"/"},
						{title:"类别",value:"/"},
						{title:"详情页",value:"/"}
					],*/
					alink:alink,			
					img:'',
					showTitle:true, //是否 显示标题框
					imgSize:"" //图片规格
				});
				if(imgfv2){
					imgfv2.remove();
				}
				imgfv2 = new imgFieldView2({index:liIndex,merge:true,m:b});
				imgfv2.render({view:view});
			})
		},
		add:function(e){
			var $li = $(e.target).closest("li");
			this.buildAddHtml($li);
		}
	});
	//广告
	var bannerFieldView = fieldView.extend({
		render: function(context) {	
			this.proto = "bannerFieldView";		
			this.model = context.model;			
			this.buildTem();			
		},		
		buildTem:function(isReplace){
			this.buildBanners(isReplace);
			this.buildToolPanel();
		},
		buildToolPanel:function(){			
			var str = '';			
			var self = this;
            var model = self.model;
			var data = model.get("bannerList");
			var type = model.get("type");
			var size = model.get("size");
			str += '';
			str += '<div class="control-group"><label class="control-label">显示方式：</label>';
        	str += '<div class="controls">';
			
			str += '<label class="radio inline"><input type="radio" name="show_method" value="0" '+(type == 0?"checked=checked":"")+'>折叠轮播</label>';
            str += '<label class="radio inline"><input type="radio" name="show_method" value="1" '+(type == 1?"checked=checked":"")+'>专题</label>';
            str += '</div></div>';
			
			str += '<div class="control-group '+(type == 1?"":"dis-none")+'">';
        	str += '<label class="control-label">显示大小：</label>';
        	str += '<div class="controls">';
            str += '<label class="radio inline"><input type="radio" name="size" value="0" '+(size == 0?"checked=checked":"")+'>大图</label>';
			str += '<label class="radio inline"><input type="radio" name="size" value="1" '+(size == 1?"checked=checked":"")+'>小图</label>';
			str += '<label class="radio inline"><input type="radio" name="size" value="2" '+(size == 2?"checked=checked":"")+'>一大两小</label>';
			str += '<label class="radio inline"><input type="radio" name="size" value="3" '+(size == 3?"checked=checked":"")+'>一大四小</label>';
            str += '</div></div>';
			
			str += '<div class="control-group clearfix">';
            str += '<label class="control-label"><input type="checkbox" name="showtitle" value="0" class="vt" '+(model.get("showTitle")?"checked=checked":"")+'><span class="ml3">显示标题</span></label>';
			
            str += '</div>';
			
			str += '<div class="control-group js-choices-region"><ul class="choices ui-sortable">';			
			      
           str += '</ul></div>';
		   str += '<div class="control-group options"><a class="add-option js-add-option" data-toggle="modal" data-target="#testModal"><i class="icon-add"></i>添加一个广告</a>    </div>';
		   str += '';
		   
		   var $sidebar = $(".app-sidebar-inner");
		   $sidebar.find(">div").html(str);
		   
		   if(data && data.models){
				for(var i=0;i<data.models.length;i++){
					var obj = JSON.parse(JSON.stringify(data.models[i]));
					var ad = new adView({view:self,index:i,container:$sidebar.find("ul.choices")});									
				}
			}  		   
		   //面板 事件
		   //显示大小			
			$sidebar.find("input[name=size]").off("change").on("change",function(e){
				var ele = this;
				var val = parseInt($(this).val());
				self.model.set({size:val});
				self.buildBanners(true);
			});
			//显示方式
			$sidebar.find("input[name=show_method]").off("change").on("change",function(e){
				var ele = this;
				var val = parseInt($(this).val());
				self.model.set({type:val});
				var $item = $(ele).closest(".control-group").next(".control-group");
				if(val == 0){					
					$item.hide();
				}else{					
					$item.show();
				}				
				self.buildBanners(true);
			});	
			 //是否显示 标题	
			$sidebar.find("input[name=showtitle]").off("change").on("change",function(e){
				var ele = this;
				var showTitle = ele.checked;
				self.model.set({showTitle:showTitle});
				self.buildBanners(true);
			});
			//添加 图标
			$sidebar.find("a.add-option").off("click").on("click",function(){				
				if(imgfv){
					imgfv.remove();
				}
				imgfv = new imgFieldView;
				imgfv.render({view:self});
			});		
		},
		buildBanners:function(isReplace){
			var self = this;
			var str = self.getHeaderHtml();			
			var model = self.model;
			var data = model.get("bannerList");
			var type = parseInt(model.get("type"));
			var size = parseInt(model.get("size"));
			var showTitle = model.get("showTitle");			
			if(!data || data.length == 0){
				/*data = new collection;
				var option = {
					title:'名称',
					alink:'',			
					img:'resource/default/image/theme/t1.png'
				}
				var b1 = new bannerModel(option);				
				var b2 = new bannerModel(option);
				var b3 = new bannerModel(option);
				data.add(b1);
				data.add(b2);
				data.add(b3);
				if(type == 1 && (size == 2)){
					b1.set({img:"resource/default/image/theme/t3.png"});
					b2.set({img:"resource/default/image/theme/t4.png"});
					b3.set({img:"resource/default/image/theme/t5.png"});
				}
				if(type == 1 && (size == 1 || size == 3)){
					var b4 = new bannerModel(option);
					data.add(b4);
				}
				if(type == 1 && size == 3){
					var b5 = new bannerModel(option);
					data.add(b5);
				}*/
				var option = {
					bid:"",
					title:'名称',
					linktype:0,
					selectedIndex:0,
					linklist:[
						{title:"首页",value:"/"},
						{title:"类别",value:"/"},
						{title:"详情页",value:"/"}
					],
					alink:'',
					img:'resource/default/image/banner/b1.png',
					showTitle:false, //是否选择
					imgSize:"320*320" //图片规格
				}
				var b1 = new bannerModel(option);				
				var b2 = new bannerModel(option);				
				var b3 = new bannerModel(option);				
				var b4 = new bannerModel(option);
				var bannerList = new collection;
				bannerList.add(b1);
				bannerList.add(b2);
				bannerList.add(b3);
				bannerList.add(b4);
				data = bannerList;
			}			
			/**/			
			if(type == 1){
				str += '<table class="clumn-table" cellpadding="0" cellspacing="0">';
				for(var i=0;i<data.models.length;i++){
					var obj = JSON.parse(JSON.stringify(data.models[i]));
					var href = obj.alink;
					if(!href || href == "#"){
						href = "javascript:void(0);"
					}
					var title = showTitle?'<span class="th-tit">'+obj.title+'</span>':'';
					var liStyle = "";
					var classStyle = "";	
					if(size == 0){//大图						
						str += '<tr><td class="w100"><a class="mb10" href="'+href+'">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td></tr>';
					}else if(size == 1){//小图
						if(i%2 == 0){//单数
							classStyle = "mr5";
							str += '<tr><td><a class="mb10 mr5" href="'+href+'">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td>';
						}else{//双数
							str += '<td><a class="mb10 ml5" href="'+href+'">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td></tr>';
						}						
					}
					else if(size == 2){//一大两小（横排）
						if(i%3 == 0){//第一个							
							str += '<tr><td rowspan="2"><a href="'+href+'" class="mr5">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td>';
						}else if(i%3 == 1){//第二个
							str += '<td><a href="'+href+'" class="ml5">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td></tr>';
						}else{//第三个
							str += '<tr><td class="vb"><a href="'+href+'" class="ml5">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td></tr>';
						}
					}
					else if(size == 3){//一大四小（竖排）
						if(i%5 == 0){//第一个
							str += '<tr><td class="w100" colspan="4"><a href="'+href+'" class="mb10">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td></tr><tr>';
						}else{
							str += '<td class="w25"><a href="'+href+'" class="mb10">'+title+'<img class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/></a></td>';
						}
						if(i%5 == 4){
							str += '</tr>';
						}
					}									
				}
				str += '</table>';		
			}
			else{
				str += '<div class="banner"><div class="swiper-container" id="swiper-'+this.cid+'">';
				str += '<div class="swiper-wrapper">';
				
				for(var i=0;i<data.models.length;i++){
					var obj = JSON.parse(JSON.stringify(data.models[i]));
					var url = obj.alink;
					if(!url || url == "#"){
						url = ""
					}
					var title = showTitle?'<span class="swiper-tit">'+obj.title+'</span>':'';					
					str += '<div class="swiper-slide" data-src="'+url+'"><img src="'+obj.img+'" width="100%"/>'+title+'</div>';
				}
				str += '</div></div><div class="slider-bar"></div></div>';				
			}
			/**/
			str += this.getFooterHtml();			
			
			if(!isReplace){
				var $str;
				if(this.sidebar){
					var $editingField = $(this.el).find(".editing");
					$str = $(str);
					if($editingField.length){						
						$editingField.after($str);
					}else{
						$str = $(str).appendTo($(this.el));
					}					
					this.$field = $str;
				}else{
					$str = this.$field = $(str).appendTo($(this.el));
				}
				var position = $str.position();
				$(".app-sidebar").css({"margin-top":position.top});
				$(this.el).find(".editing").removeClass("editing");
				this.$field.addClass("editing");
				//保存 对象
				$str.data("view",self);				
			}else{
				self.$field.html($(str).html());
			}
			//创建滑动 banner 脚本			
			if(type == 0){				
				var scriptStr = '<script type="text/javascript">createSwiper("'+this.cid+'");</script>';
				$(scriptStr).appendTo(self.$field);
			}
			//绑定事件			
			$(self.el).find(".action.add").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.add(e,ele);
			});
			$(self.el).find(".action.edit").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.edit(e,ele);
			});
			$(self.el).find(".action.delete").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");				
				view.del(e,ele);
			});
		}
	});
	
	//标题对象
	var titleModel =  Backbone.Model.extend({
	    defaults : {			
			title:'此处标题名称',
			smallTitle:''
		},	
		initialize: function(){
			
		}
	});	
	//标题
	var titleFieldView = fieldView.extend({
		render: function(context) {
			this.proto = "titleFieldView";			
			this.model = context.model;			
			this.buildTem();			
		},		
		buildTem:function(){
			this.buildTitle();
			this.buildToolPanel();
		},
		buildToolPanel:function(){			
			var str = '';			
			var self = this;
            var model = self.model;
			var data = model.get("bannerList");
			var type = model.get("");
			str += '';
			str += '<div class="control-group"><label class="control-label">标题名：</label>';
        	str += '<div class="controls">';
			var tVal = model.get("title");
			if(!tVal || tVal == "此处标题名称"){
				tVal = "";
			}
			str += '<input type="text" name="title" value="'+tVal+'" maxlength="100"><p class="help-block error-message">标题不能为空。</p>';            
            str += '</div></div>';
			var fVal = model.get("smallTitle");
			if(!fVal || fVal == "此处标题名称"){
				fVal = "";
			}
			str += '<div class="control-group">';
        	str += '<label class="control-label">副标题：</label>';
        	str += '<div class="controls">';
            str += '<input type="text" name="sub_title" value="'+fVal+'" maxlength="100">';			
            str += '</div></div>';
		   
		   var $sidebar = $(".app-sidebar-inner");
		   $sidebar.find(">div").html(str);		   
		   //面板 事件			
			$sidebar.find("input[name=title]").off("keyup").on("keyup",function(e){
				var ele = this;
				var val = $(this).val();
				self.model.set({title:val});
				self.buildTitle(true);
			});
			$sidebar.find("input[name=sub_title]").off("keyup").on("keyup",function(e){
				var ele = this;
				var val = $(this).val();
				self.model.set({smallTitle:val});
				self.buildTitle(true);
			});	
		},
		buildTitle:function(isReplace){
			var self = this;
			var str = self.getHeaderHtml();			
			var model = self.model;
			str += '<div class="clumn"><h3><span>'+model.get("title")+'</span><em>'+model.get("smallTitle")+'</em></h3></div>';
			/**/
			str += this.getFooterHtml();			
			
			if(!isReplace){
				var $str;
				if(this.sidebar){
					var $editingField = $(this.el).find(".editing");
					$str = $(str);
					if($editingField.length){						
						$editingField.after($str);
					}else{
						$str = $(str).appendTo($(this.el));
					}					
					this.$field = $str;
				}else{
					$str = this.$field = $(str).appendTo($(this.el));
				}
				var position = $str.position();
				$(".app-sidebar").css({"margin-top":position.top});
				$(this.el).find(".editing").removeClass("editing");
				this.$field.addClass("editing");
				//保存 对象
				$str.data("view",self);				
			}else{
				self.$field.html($(str).html());
			}			
			
			//绑定事件			
			$(self.el).find(".action.add").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.add(e,ele);
			});
			$(self.el).find(".action.edit").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.edit(e,ele);
			});
			$(self.el).find(".action.delete").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");				
				view.del(e,ele);
			});
			
		}
	});
	//导航
	var navFieldView = fieldView.extend({
		render: function(context) {
			this.proto = "navFieldView";
			this.model = context.model;			
			this.buildTem();			
		},		
		buildTem:function(isReplace){
			this.buildBanners(isReplace);
			this.buildToolPanel();
		},
		buildToolPanel:function(){			
			var str = '';			
			var self = this;
            var model = self.model;
			var data = model.get("bannerList");
			var type = model.get("");
			str += '';
			str += '<div class="control-group js-collection-region"><ul class="choices ui-sortable"></ul></div>';			
			str += '<div class="control-group options"><a class="add-option js-add-option" data-toggle="modal" data-target="#testModal"><i class="icon-add"></i>添加一个导航</a></div>';
		   str += '';
		   
		   var $sidebar = $(".app-sidebar-inner");
		   $sidebar.find(">div").html(str);
		   
		   if(data && data.models){
				for(var i=0;i<data.models.length;i++){
					var obj = JSON.parse(JSON.stringify(data.models[i]));
					var ad = new adView({view:self,index:i,container:$sidebar.find("ul.choices")});									
				}
			} 
		   //面板 事件			
			$sidebar.find("a.add-option").off("click").on("click",function(){				
				if(imgfv){
					imgfv.remove();
				}
				imgfv = new imgFieldView;
				imgfv.render({view:self});
			});	
		},
		buildBanners:function(isReplace){
			var self = this;
			var str = self.getHeaderHtml();			
			var model = self.model;
			var data = model.get("bannerList");					
			if(!data || data.length == 0){
				var option = {
					bid:"",
					title:'名称',
					linktype:0,
					selectedIndex:0,
					linklist:[
						{title:"首页",value:"/"},
						{title:"类别",value:"/"},
						{title:"详情页",value:"/"}
					],
					alink:'',
					img:'resource/default/image/nav/n1.png',
					showTitle:true, //是否选择
					imgSize:"320*320" //图片规格
				}
				var b1 = new bannerModel(option);
				option.bid = "pbid_2";
				option.img = 'resource/default/image/nav/n2.png';
				var b2 = new bannerModel(option);
				option.bid = "pbid_3";
				option.img = 'resource/default/image/nav/n3.png';
				var b3 = new bannerModel(option);
				option.bid = "pbid_4";
				option.img = 'resource/default/image/nav/n4.png';
				var b4 = new bannerModel(option);
				var bannerList = new collection;
				bannerList.add(b1);
				bannerList.add(b2);
				bannerList.add(b3);
				bannerList.add(b4);	
				data = bannerList;				
			}
			str += '<div class="nav-wrap"><ul class="clearfix">';
			for(var i=0;i<data.models.length;i++){
				var obj = JSON.parse(JSON.stringify(data.models[i]));
				var href = obj.alink;
				if(!href || href == "#"){
					href = "javascript:void(0);";
				}
				str += '<li> <a href="'+href+'"> <img  class="lazyload" data-src="'+lazyloadImg+'" src="'+obj.img+'"/><span>'+(obj.title?obj.title:"名称")+'</span> </a> </li>';
			}
			str += '</ul></div>';
			/**/
			str += this.getFooterHtml();			
			
			if(!isReplace){
				var $str;
				if(this.sidebar){
					var $editingField = $(this.el).find(".editing");
					$str = $(str);
					if($editingField.length){						
						$editingField.after($str);
					}else{
						$str = $(str).appendTo($(this.el));
					}					
					this.$field = $str;
				}else{
					$str = this.$field = $(str).appendTo($(this.el));
				}
				var position = $str.position();
				$(".app-sidebar").css({"margin-top":position.top});
				$(this.el).find(".editing").removeClass("editing");
				this.$field.addClass("editing");
				//保存 对象
				$str.data("view",self);				
			}else{
				self.$field.html($(str).html());
			}		
			
			//绑定事件			
			$(self.el).find(".action.add").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.add(e,ele);
			});
			$(self.el).find(".action.edit").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");
				view.edit(e,ele);
			});
			$(self.el).find(".action.delete").off("click").on("click",function(e){
				var ele = this;
				var view = $(ele).closest(".app-field").data("view");				
				view.del(e,ele);
			});
			
		}
	});
	//添加 内容工具按钮
	var imgfv;
	var imgFieldView = Backbone.View.extend({		
		tagName:"tbody",		
		initialize: function(){
			$("#btable tbody").remove();			
			$("#btable").append(this.el);
			/*显示按钮*/	
			$("#testModal").find(".modal-footer").show();					
		},
		render: function(context) {			
			this.view = context.view;
			var model = this.view.model;
			//存下选中的		
			var bannerList = model.get("bannerList");
			setMyCookie("selectList_"+this.view.cid,bannerList);			
			/*this.proto = "bannerFieldView";	*/
			var proto = this.view.proto;
			if(proto == "navFieldView"){ //导航 获取数据
				this.param = {action:"LoadMoreTplCfgInfo",type:2};
			}else{//广告
				if(this.view.model.get("type") == 0){ //轮播
					this.param = {action:"LoadMoreTplCfgInfo",type:1};
				}else{ // 专题
					this.param = { action:"LoadHomeTopicsList"};
				}
			}			
			this.getData();
			var self = this;
			//点击 确定按钮
			$("#testModal-ok").off("click.ok").on("click.ok",function(){
				self.ok.apply(self);
			})
			//点击 搜索按钮
			$("#test-search").off("click.ok").on("click.ok",function(){
				var val = $("#test-search-input").val();				
			})			
		},
		events:{  
			'click .js-choose' : 'selected'
		},
		getData:function(param){
			/*轮播图：data: { action: "LoadMoreTplCfgInfo", type:1,pageIndex:1,pageSize:8 },//pageIndex=1第一页
			导航图标：data: { action: "LoadMoreTplCfgInfo",type:2, pageIndex:1,pageSize:8 },//pageIndex=1第一页,和转播图的type不一样，方法一样
			专题图：data: { action: "LoadHomeTopicsList", pageIndex:1,pageSize:8 },//pageIndex=1第一页
			商品：data: { action: "LoadMoreProduct", productName:'',pageIndex:1,pageSize:8 }
*/
			var self = this;
			var param = self.param;
			param.pageIndex = 1;
			param.pageSize = 6;
			$.ajax({
				url: "/API/VshopDesignHandler.ashx",
				type: 'post', dataType: 'json', timeout: 10000,
				data: param,
				async: false,
				success: function(resultData) {
					var totalPage;
					if(param.action == "LoadMoreTplCfgInfo" ||　param.action == "LoadMoreTplCfgInfo"){//轮播 //导航
						self.buildHtml(resultData.tplCfgInfo);
						totalPage = resultData.totalPage;
					}else{ // 专题
						self.buildHtml(resultData.Table);
						totalPage = resultData.Table1[0].Column1;
					}
					//创建分页
					$("#main").find(".page-wrap").createPage({
						pageCount:totalPage,
						current:1,
						backFn:function(p){
							//单击回调方法，p是当前页码					
							self.ajax(p);
						}
					});
				}
			});
		},
		ajax:function(p){
			var self = this;
			var param = self.param;
			param.pageIndex = p;
			param.pageSize = 6;
			$.ajax({
				url: "/API/VshopDesignHandler.ashx",
				type: 'post', dataType: 'json', timeout: 10000,
				data: param,
				async: false,
				success: function(resultData) {	
					var totalPage;
					if(param.action == "LoadMoreTplCfgInfo" ||　param.action == "LoadMoreTplCfgInfo"){//轮播 //导航
						self.buildHtml(resultData.tplCfgInfo);
						totalPage = resultData.totalPage;
					}else{ // 专题
						self.buildHtml(resultData.Table);
						totalPage = resultData.Table1[0].Column1;
					}				
					//self.buildHtml(resultData.tplCfgInfo);
				}
			});
		},
		buildHtml:function(data){			
			var selectList = eval(setMyCookie("selectList_"+this.view.cid));
			var list = new collection(selectList);			
			var str = '';
			/*
			bid:"",			
			title:'此处显示广告名称',
			alink:'',
			selected:false,			
			img:'resource/default/image/temp/b1.jpg',
			imgSize:"",
			*/
			$(this.el).html("");
			if(!data){
				return;
			}
			var param = this.param;
			/*for(var i=0;i<data.length;i++){
				var obj = JSON.parse(JSON.stringify(data.models[i]));*/
				
			/*BannerId: 31
			Client: 1
			DisplaySequence: 24
			Id: 31
			ImageUrl: "/Storage/master/banner/20150605200357_1842.jpg"
			IsDisable: false
			LocationType: 12
			LoctionUrl: "/vshop/BrandList.aspx"
			ShortDesc: "brand"
			Type: 1
			Url: "BrandList.aspx"*/	
			
			/*
			  专题			  
			AddedDate: "2015-04-16T16:42:31.033"
			DisplaySequence: 0
			IconUrl: "/Storage/master/topic/3fb8c69632b64509bc39203b8d732887.jpg"
			Keys: ""
			Title: "鞋包配件"
			TopicId: 1
			no: 1
			*/
			for(var i=0;i<data.length;i++){	
				var o = data[i];
				var obj;
				if(param.action == "LoadMoreTplCfgInfo" ||　param.action == "LoadMoreTplCfgInfo"){//轮播 //导航			
					obj = {
						bid:""+o.Id,
						title:o.ShortDesc,
						linktype:0,
						selectedIndex:0,
						linklist:[
							{title:"首页",value:"/"},
							{title:"类别",value:"/"},
							{title:"详情页",value:"/"}
						],
						alink:o.LoctionUrl,
						img:o.ImageUrl,
						showTitle:true, //是否 显示标题框
						imgSize:"320*320" //图片规格
					}
				}else{ // 专题
					obj = {
							bid:""+o.TopicId,
							title:o.Title,
							linktype:0,
							selectedIndex:0,
							linklist:[
								{title:"首页",value:"/"},
								{title:"类别",value:"/"},
								{title:"详情页",value:"/"}
							],
							alink:"/Vshop/Topics.aspx?TopicId="+o.TopicId,
							img:o.IconUrl,
							showTitle:true, //是否 显示标题框
							imgSize:"320*320" //图片规格
						}
				}
				//是否存在
				var m = list.where({"bid":obj.bid});
				var selected = false;
				if(m && m.length){
					selected = true;
				}
				var href = obj.alink;
				if(!href || href == "#"){
					href = "javascript:void(0);";
				}
				str = '<tr data-bid="'+obj.bid+'">';
				str += '<td class="title"><div class="td-cont"> <a target="_blank" class="new_window clearfix" href="'+href+'"><span class="img"><img src="'+obj.img+'" /></span></a> </div></td>';				
                str += '<td><a target="_blank" class="new_window clearfix" href="'+href+'"><span>'+obj.title+'</span></a></td>';
				if(href == "javascript:void(0);"){
					href = "";
				}
                str += '<td class="time"><div class="ovh"><p class="ellipsis">'+href+'</div></div></td>';
                str += '<td class="opts"><div class="td-cont"><button class="btn js-choose '+(selected?"btn-primary":"")+'" href="javascript:void(0);">选取</button></div></td>';
                str += '</tr>';
				var $tr = $(str).appendTo($(this.el));
				$tr.data("model",obj);
			}						
		},
		selected:function(e){			
			var model = this.view.model;
			//获取选中的	
			var selectList = eval(setMyCookie("selectList_"+this.view.cid));
			var list = new collection(selectList);
			var $ele = $(e.currentTarget);
			var $tr = $ele.closest("tr");
			var bid = $tr.attr("data-bid");
			var theme = "btn-primary";			
			if($ele.hasClass(theme)){
				$ele.removeClass(theme);
				var m = list.where({"bid":bid});
				if(m){
					list.remove(m);
				}
			}else{
				$ele.addClass(theme);
				var m = list.where({"bid":bid});
				if(!m.length){					
					list.add($tr.data("model"));
				}
			}
			setMyCookie("selectList_"+this.view.cid,list);			
		},
		ok:function(){
			var model = this.view.model;
			var data = model.get("bannerList");
			//获取选中的	
			var selectList = eval(setMyCookie("selectList_"+this.view.cid));
			var list = new collection(selectList);			
			model.set({"bannerList":list});		
			this.view.buildTem(true);
		}
	});	
	var imgfv2;
	var imgFieldView2 = imgFieldView.extend({
		initialize: function(option){	
			$("#btable tbody").remove();			
			$("#btable").append(this.el);	
			/*隐藏 按钮*/	
			$("#testModal").find(".modal-footer").hide();
			
			this.index = option.index;
			this.merge = option.merge;	
			this.m = option.m;	
		},
		selected:function(e){
			$("#testModal").modal('hide');
			
			var theme = "btn-primary";		
			var $ele = $(e.currentTarget).addClass(theme);
			var $tr = $ele.closest("tr");
			var bid = $tr.attr("data-bid");						
			var m = $tr.data("model");			
			var obj = this.m;
			obj.set({"bid":m.bid,"img":m.img,"title":m.title,"alink":m.alink});
						
			var index = this.index;
			var merge = this.merge?true:false;	
			var model = this.view.model;
			var data = model.get("bannerList");
			//获取选中的	
			data.add(obj,{at:index,merge:merge});			
			this.view.buildTem(true);
		}
	})
	var profv;
	var proFieldView = Backbone.View.extend({
		tagName:"tbody",		
		initialize: function(){	
				$("#table tbody").remove();			
				$("#table").append(this.el);					
		},
		render: function(context) {			
			this.view = context.view;
			var model = this.view.model;
			//存下选中的		
			var goodList = model.get("goodList");
			setMyCookie("selectList_"+this.view.cid,goodList);
			
			this.param = {action: "LoadMoreProduct",keyWord:''};
			this.getData();
			var self = this;			
			//点击 确定按钮
			$("#proModal-ok").off("click.ok").on("click.ok",function(){
				self.ok.apply(self);
			})
			//点击 搜索按钮
			$("#proModal-search").off("click.ok").on("click.ok",function(){
				var val = $.trim($("#proModal-search-input").val());
				self.param.keyWord = val;
				self.getData();
			})
			
		},
		events:{  
			'click .js-choose' : 'selected'
		},
		getData:function(){
			/*轮播图：data: { action: "LoadMoreTplCfgInfo", type:1,pageIndex:1,pageSize:8 },//pageIndex=1第一页
			导航图标：data: { action: "LoadMoreTplCfgInfo",type:2, pageIndex:1,pageSize:8 },//pageIndex=1第一页,和转播图的type不一样，方法一样
			专题图：data: { action: "LoadHomeTopicsList", pageIndex:1,pageSize:8 },//pageIndex=1第一页
			商品：data: { action: "LoadMoreProduct", productName:'',pageIndex:1,pageSize:8 }
*/
			var self = this;
			var param = self.param;
			param.pageNumber = 1;
			param.size = 6;
			$.ajax({
				url: "/API/VshopDesignHandler.ashx",
				type: 'post', dataType: 'json', timeout: 10000,
				data: param,
				async: false,
				success: function(resultData) {
					self.buildHtml(resultData.products);
					//创建分页
					$("#goodmain").find(".page-wrap").createPage({
						pageCount:resultData.totalPage,
						current:1,
						backFn:function(p){
							//单击回调方法，p是当前页码					
							self.ajax(p);
						}
					});	
				}
			});
		},
		ajax:function(p){
			var self = this;
			var param = self.param;
			param.pageNumber = p;
			param.size = 6;
			$.ajax({
				url: "/API/VshopDesignHandler.ashx",
				type: 'post', dataType: 'json', timeout: 10000,
				data: param,
				async: false,
				success: function(resultData) {					
					self.buildHtml(resultData.products);
				}
			});
		},
		buildHtml:function(data){			
			var selectList = eval(setMyCookie("selectList_"+this.view.cid));
			var list = new collection(selectList);
			var str = '';			
			$(this.el).html("");						
			/*
			ActivityId: 0
			MarketPrice: null
			ProductCode: "06186"
			ProductId: 907
			ProductName: "我是日本的"
			RowNumber: 1
			SaleCounts: 1
			SalePrice: 100
			ShortDescription: ""
			TaxRate: 0
			ThumbnailUrl60: null
			ThumbnailUrl100: null
			ThumbnailUrl160: null
			ThumbnailUrl180: null
			ThumbnailUrl220: null
			ThumbnailUrl310: null
			VistiCounts: 16
			fastbuy_skuid: "907_0"
			
			*/	
				
			for(var i=0;i<data.length;i++){	
				var o = data[i];			
				var obj = {
					gid:""+o.ProductId,
					alink:"/Vshop/ProductDetails.aspx?ProductId="+o.ProductId,
					title:o.ProductName,
					rate:o.TaxRate+'%',
					price:o.SalePrice,
					img:o.ThumbnailUrl160,
					fastbuy_skuid:o.fastbuy_skuid,
				}
				//是否存在
				var m = list.where({"gid":obj.gid});
				var selected = false;
				if(m && m.length){
					selected = true;
				}				
				str = '<tr data-gid="'+obj.gid+'"><td class="title"><div class="td-cont">';
				str += '<a target="_blank" class="new_window clearfix" href="'+obj.alink+'">';
				str += '<span class="img"><img src="'+obj.img+'" /></span>';
				str += '<span class="good-name">'+obj.title+'</span></a> </div></td>';				
                str += '<td class="time"><div class="td-cont"> <span>2015-04-17<br>16:52:03</span></div></td>';
                str += '<td class="opts"><div class="td-cont">';
				str += '<button class="btn js-choose '+(selected?"btn-primary":"")+'" href="javascript:void(0);">选取</button>';
				str += '</div></td></tr>';
				var $tr = $(str).appendTo($(this.el));
				$tr.data("model",obj);
			}						
		},
		selected:function(e){			
			var model = this.view.model;
			//获取选中的	
			var selectList = eval(setMyCookie("selectList_"+this.view.cid));
			var list = new collection(selectList);
			var $ele = $(e.currentTarget);
			var $tr = $ele.closest("tr");
			var gid = $tr.attr("data-gid");
			var theme = "btn-primary";			
			if($ele.hasClass(theme)){
				$ele.removeClass(theme);
				var m = list.where({"gid":gid});
				if(m){
					list.remove(m);
				}
			}else{
				$ele.addClass(theme);
				var m = list.where({"gid":gid});
				if(!m.length){					
					list.add($tr.data("model"));
				}
			}
			setMyCookie("selectList_"+this.view.cid,list);
		},
		ok:function(){
			var model = this.view.model;
			var data = model.get("goodList");
			//获取选中的	
			var selectList = eval(setMyCookie("selectList_"+this.view.cid));
			var list = new collection(selectList);			
			model.set({"goodList":list});		
			this.view.buildTem(true);
		}
	});
	//添加 内容工具按钮
	var addFieldView = Backbone.View.extend({			
		initialize: function(){	
			this.render();		
		},
		render: function(context) {
			//使用underscore这个库，来编译模板
			var template = _.template($("#add_field_template").html(), context);
			//加载模板到对应的el属性中
			$(this.el).html(template);
			//表示 是右测添加
			if($(this.el).parent().hasClass("js-sidebar-region")){
				this.sidebar = true;
			}
			this.bindEvents();	
		},		
		bindEvents:function(){
			var self = this;
			$(this.el).find("a[data-field-type=goods]").off("click").on("click",function(e){
				var ele = this;
				self.addGoods(e,ele);
			});
			$(this.el).find("a[data-field-type=image_ad]").off("click").on("click",function(e){
				var ele = this;
				self.addBanner(e,ele);
			});	
			$(this.el).find("a[data-field-type=title]").off("click").on("click",function(e){
				var ele = this;
				self.addTitle(e,ele);
			});
			$(this.el).find("a[data-field-type=nav_link]").off("click").on("click",function(e){
				var ele = this;
				self.addNav(e,ele);
			});			
		},
		addGoods:function(ev){
			var sidebar = this.sidebar;				
			var gfv = new goodFieldView({el: $("#app-fields"),sidebar:sidebar});
			gfv.render({model:new goodPanelModel});
			viewList.push(gfv);
		},
		addBanner: function(ev){
			var sidebar = this.sidebar;			
			var bfv = new bannerFieldView({el: $("#app-fields"),sidebar:sidebar});			
			bfv.render({model:new bannerPanelModel});
			viewList.push(bfv);
		},
		addTitle: function(ev){
			var sidebar = this.sidebar;
			var tfv = new titleFieldView({el: $("#app-fields"),sidebar:sidebar});
			tfv.render({model:new titleModel});
			viewList.push(tfv);
		},
		addNav: function(ev){
			var sidebar = this.sidebar;
			var bfv = new navFieldView({el: $("#app-fields"),sidebar:sidebar});
			bfv.render({model:new bannerPanelModel});
			viewList.push(bfv);
		}
	});
	//var viewListStr = setMyCookie("viewList");
	var viewListStr;
	function getviewListStr(){
		var param = {action: "LoadDesignTemplate"};
		$.ajax({
			url: "/API/VshopDesignHandler.ashx",
			type: 'post', dataType: 'json', timeout: 10000,
			data: param,
			async: false,
			success: function(resultData) {
				viewListStr=resultData.Content;
				if(resultData.InUse){
					flag=1;
					$('#activate-btn').val('取消启用');
				}
			}
		});
	};
	//初始化
	getviewListStr();
	if(viewListStr){
		
		function jsondecode(data){ 
			return (new Function("return " + data))();
		}
		viewList = 	jsondecode(viewListStr);
	}
	var addFieldView1;
	if(viewList){
		var list = [];
		
		for(var i=0;i<viewList.length;i++){
			var view = $.extend({},viewList[i]);
			var option = $.extend({},view.model);			
			var opt = {el: $("#app-fields"),sidebar:false};			
			var model = {};
			if(view.proto == "goodFieldView"){
				option.goodList = new collection(view.model.goodList);				
				model = new goodPanelModel(option);
				view = new goodFieldView(opt);
			}
			if(view.proto == "bannerFieldView"){
				option.bannerList = new collection(view.model.bannerList);				
				model = new bannerPanelModel(option);
				view = new bannerFieldView(opt);
			}
			if(view.proto == "titleFieldView"){										
				model = new bannerPanelModel(option);
				view = new titleFieldView(opt);
			}
			if(view.proto == "navFieldView"){
				option.bannerList = new collection(view.model.bannerList);				
				model = new bannerPanelModel(option);
				view = new navFieldView(opt);
			}
			view.render({model:model});
			list.push(view);			
		}
		viewList = [];
		/*for(var i=0;i<list.length;i++){
			viewList.push(list[i]);
		}*/
		//viewList = list;
		viewList = list;
		/*viewList.push({});
		viewList.pop();*/
		addFieldView1 = new addFieldView({el: $(".js-add-region div")});
	}else{
		addFieldView1 = new addFieldView({el: $(".js-add-region div")});	
		viewList = [];
	}
})