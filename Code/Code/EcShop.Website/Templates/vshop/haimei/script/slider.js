var Core = Core || {};
var source = Core.source;
(function(NS){	
	//移动的 方法
	function _translate(ele,tx,dur){
		var $ele = $(ele);
		if(typeof dur == "undefined" || isNaN(dur)){
			dur = 0;
		}
		$ele.css({
			"-webkit-transition":"left "+dur+"ms linear 0s",
			"-moz-transition":"left "+dur+"ms linear 0s",
			"-o-transition":"left "+dur+"ms linear 0s",
			"transition":"left "+dur+"ms linear 0s",
			"left":tx+"px"
		});		
	}
	function getTx($ele){
		var ml = $ele.css("left");
		var arr = ml.split("px");
		return Number(arr[0]);
	}
	function _bindEvent(self,settings){
		var startPos,endPos,nameSpace = ".slider";
		var MOUSEDOWN = source.MOUSEDOWN + nameSpace;
		var MOUSEMOVE = source.MOUSEMOVE + nameSpace;
		var MOUSEUP = source.MOUSEUP + nameSpace;
		var CLICK = source.CLICK;
		settings.$sliderCon.find("a").bind(MOUSEDOWN,function(e){
			e.preventDefault();
			e.stopPropagation();
			startPos =  source.getEventPosition(e);
			var activeIndex = self.getActiveIndex();
			var ml = getTx(settings.$sliderCon);
			if(settings.autoPlay){
				self.stopAutoPlay();
			}			
			$(document).unbind(MOUSEMOVE).bind(MOUSEMOVE,function(e){
				e.preventDefault();
				e.stopPropagation();
				
				var lastEndPos = endPos;
				endPos =  source.getEventPosition(e);
				var dirs = source.getDir(startPos,endPos);
				//表示没有移动 跳出
				if(dirs == ""){
					return;
				}
				//表示 上下滑动
				if(dirs.indexOf("up") != -1 || dirs.indexOf("down") != -1){
					if($.isFunction(settings.upDownCallBack)){
						settings.upDownCallBack(lastEndPos,endPos);
					}
				}else{
					var coorX = endPos.x - startPos.x;								
					_translate(settings.$sliderCon,(coorX+ml),0);
				}
			});
			$(document).unbind(MOUSEUP).bind(MOUSEUP,function(e){
				e.preventDefault();
				e.stopPropagation();
				$(document).unbind(MOUSEMOVE);
				$(document).unbind(MOUSEUP);
				
				var dirs = source.getDir(startPos,endPos);
				if(dirs.indexOf("left") != -1){
					activeIndex += 1;
				}else if(dirs.indexOf("right") != -1){
					activeIndex -= 1;
				}else{				
					if($(e.target).is("img")){
						window.location.href = $(e.target).parent().attr("href");
						return;
					}
					if($(e.target).is("a")){
						window.location.href = $(e.target).attr("href");
						return;
					}
					return;
				}				
				//左右 滑到 尽头 处理		
				if(activeIndex < 0){
					activeIndex = 0;
				}
				if(activeIndex > settings.$sliderEle.length-1){
					activeIndex = settings.$sliderEle.length-1;
				}		
				self.setActive(activeIndex);
				if(settings.autoPlay){
					self.autoPlay();
				}
			});
		})
		/*settings.$sliderEle.find("a").bind(CLICK,function(e){
			var href = $(this).attr("href");
			window.location.href = href;
		});*/
		settings.container.find(".slider-bar li").bind(CLICK,function(e){
			if(settings.autoPlay){
				self.stopAutoPlay();
			}
			var index = $(this).index();
			self.setActive(index);
			if(settings.autoPlay){
				self.autoPlay();
			}
		})
	}
	function _init(self){			
		var settings = self._settings;
		settings.$sliderWrap = settings.container.find(".slider-wrap");
		var $sliderCon = settings.$sliderCon = settings.container.find(".slider-con");
		var $sliderEle = settings.$sliderEle = settings.container.find(".slider-ele");		
		var width = settings.width = self._settings.container.width();
		$sliderEle.css({width:width});		
		$sliderCon.css({
			width:($sliderEle.length > 0?$sliderEle.length:1)*width
		})	
		if(settings.createBar){
			var $ul = settings.$sliderWrap.find(".slider-bar ul");
			var str = '';
			for(var i=0;i<$sliderEle.length;i++){
				str += '<li><a class="slider-icon"></a></li>';
			}
			$ul.html(str);
		}
		self.setActive(0);
		_bindEvent(self,settings);
		if(settings.autoPlay){
			self.autoPlay();
		}
	}
	function slider(options){
		var self = this;
		this._settings = $.extend({
			container:document.body,
			delay:0,
			animate:200,
			autoPlay:true,
			createBar:false,
			upDownCallBack:null
		},options || {});		
		_init(self);
		var settings = this._settings;
		window.onresize = function(){			
			_init(self);
			if(settings.autoPlay){
				self.stopAutoPlay();
			}
			var activeIndex = self.getActiveIndex();
			self.setActive(activeIndex);			
			if(settings.autoPlay){
				self.autoPlay();
			}
		}
	}
	slider.prototype = {		
		getActiveIndex:function(){
			var self = this;
			var settings = self._settings;
			return settings.$sliderCon.find(".slider-ele.cur").index();
		},
		setActive:function(index){
			var self = this;
			var settings = self._settings;
			if(index < 0 || index > settings.$sliderEle.length-1){
				return;
			}
			settings.$sliderEle.eq(index).addClass("cur").siblings().removeClass("cur");
			settings.container.find(".slider-bar li").eq(index).addClass("cur").siblings().removeClass("cur");
			var tx = settings.width * index;
			_translate(settings.$sliderCon,-tx,settings.animate);
		},
		autoPlay:function(interTime){
			var self = this;
			var index = self.getActiveIndex();
			var settings = self._settings;
			var lenth = settings.$sliderEle.length;
			if(typeof interTime == "undefined" || isNaN(interTime)){
				interTime = 3000;
			}
			if(settings.autoplayInter){
				window.clearInterval(settings.autoplayInter);
			}
			settings.autoplayInter = window.setInterval(function(){
				index++;
				if(index >= lenth){
					index = 0;
				}
				self.setActive(index);
			},interTime);
		},
		stopAutoPlay:function(){
			var self = this;
			var settings = self._settings;
			if(settings.autoplayInter){
				window.clearInterval(settings.autoplayInter);
			}
		},
	}
	NS.slider = slider;	
	//jq
	$.fn.slider = function(options){
		var $this = this;		
		options = $.extend(options,{
			container:$this
		});
		return new slider(options);
	}
})(Core)