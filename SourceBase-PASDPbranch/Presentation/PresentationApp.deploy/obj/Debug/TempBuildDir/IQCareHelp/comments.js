/*! 
User comments script functions for Premium Pack Version 1.55 for Help & Manual 6
Support for Disqus and IntenseDebate commenting systems
Copyright (c) 2008 - 2013 by Tim Green. 
All rights reserved. 
*/
var comments_off=false;var comments_delay=10;var cBox,cLink1;var cCount="0";var pCount=0;var iOx=/ipad|iphone/gi;var comments_intensedebate=comments_type.toLowerCase()=="disqus"?false:true;var comments_disqus=comments_type.toLowerCase()=="disqus"?true:false;var comments_online=comments_path.substr(0,comments_path.lastIndexOf("/"))==location.href.substr(0,location.href.lastIndexOf("/"));if(!comments_online){comments_off=true}$(document).ready(function(){cBox=$("div#commentsBox");cLink1=$("#commentToggle1");if((iOx.test(navigator.platform)&&comments_disqus)||comments_off){$("p#commentLink1, div#commentsBox").hide()}else{if(comments_disqus){var c="";var d=window.setInterval(function(){if($("iframe#dsq1").length>0){c=window.comments_link;$(cLink1).html(c).attr("title",window.comments_showtip);window.clearInterval(d)}else{if(pCount>comments_delay*5){window.clearInterval(d);$(cLink1).html(c).attr("title",window.comments_showtip);cBox.innerHTML="<h4>"+window.comments_offline+"</h4>"}}pCount++},200)}else{if(comments_intensedebate){var a="idc-commentcount";var b="idc-commentcount_label";var d=window.setInterval(function(){if($("#"+b).length>0){cCount=document.getElementById(a)?document.getElementById(a).innerHTML:"0";$(cLink1).html(document.getElementById(a)?window.comments_link+' (<span class="comments">'+cCount+"</span>)":window.comments_link+' (<span class="comments">0</span>)');window.clearInterval(d)}else{if(pCount>comments_delay*5){cCount="?";$(cLink1).html(window.comments_link+' (<span class="comments">?</span>)');$(cBox).html("<h4>"+window.comments_offline+"</h4>");window.clearInterval(d)}}pCount++},200)}}}});function commentCaption(){var a=$("div#printheader").length>0;if(a){$("#idcontent").scrollTo($("#commentsBox"),300,{offset:-3,axis:"y"})}else{$.scrollTo($("#commentToggle2"),300,{offset:-3,axis:"y"})}return false}function doDisqus(){document.write('<div id="disqus_thread"></div>');if(!iOx.test(navigator.platform)&&!comments_off){var a=document.createElement("script");a.type="text/javascript";a.async=true;a.src="http://"+disqus_shortname+".disqus.com/embed.js";(document.getElementsByTagName("head")[0]||document.getElementsByTagName("body")[0]).appendChild(a);document.write('<noscript>Please enable JavaScript to view the <a href="http://disqus.com/?ref_noscript" target="_blank">comments powered by Disqus.</a></noscript>');document.write('<a href="http://disqus.com" class="dsq-brlink" target="_blank">Commenting system powered by <span class="logo-disqus">Disqus</span></a>')}}function doIntenseDebate(){if(!comments_off){var a=document.createElement("script");a.setAttribute("type","text/javascript");a.setAttribute("src","http://www.intensedebate.com/js/genericCommentWrapperV2.js");document.write('<span id="IDCommentsPostTitle" style="display:none"></span>');document.getElementById("commentsBox").appendChild(a)}};