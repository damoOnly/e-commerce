// JavaScript Document
var Core = Core || {};
(function(NS){
	var isTouch = "ontouchstart" in window?true:false;
	var source = {
		isTouch:isTouch,
		MOUSEDOWN:isTouch?"touchstart":"mousedown",
		MOUSEMOVE:isTouch?"touchmove":"mousemove",
		MOUSEUP:isTouch?"touchend":"mouseup",
		CLICK:isTouch?"touchend":"mouseup",
		getEventPosition:function(e){
			try{
			var touch = e;			
			if(isTouch){				
				touch = e.touches?e.touches[0]:e.originalEvent.touches[0];
				//jquery: touch = e.originalEvent && e.originalEvent.touches ? (e.originalEvent.touches[0] || e.originalEvent.changedTouches[0]) : null;  
				//touch = e.originalEvent && e.originalEvent.touches ? (e.originalEvent.touches[0] || e.originalEvent.changedTouches[0]) : null;        				
			}			
			return {x:touch.pageX,y:touch.pageY};
			}catch(ex){
				alert(ex.message);
			}
		},
		//判断 滑动的方向
		getDir:function (startPos,endPos){
			if(!startPos || !endPos){
				return "";
			}
			var dirs = [];
			var coorX = endPos.x - startPos.x;
			var coorY= endPos.y- startPos.y;
			var coorH = 30;
			var coorV = 50;	
			if(coorX > 0 && Math.abs(coorX) >= coorH && Math.abs(coorX) > Math.abs(coorY)){//向右滑动
				dirs.push("right");
			}else if(coorX < 0 && Math.abs(coorX) >= coorH && Math.abs(coorX) > Math.abs(coorY)){//向左滑动
				dirs.push("left");
			}else if(coorY > 0 && Math.abs(coorY) >= coorV && Math.abs(coorX) < Math.abs(coorY)){//向下滑动
				dirs.push("down");
			}else if(coorY < 0 && Math.abs(coorY) >= coorV && Math.abs(coorX) < Math.abs(coorY)){//向上滑动
				dirs.push("up");
			}else{
				
			}	
			return dirs.join("");
		}
	}
	NS.source = source;
})(Core)