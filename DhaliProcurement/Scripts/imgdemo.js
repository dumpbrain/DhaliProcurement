//----Coading by S.P----//
$.fn.mycaptcha = function(options)
{
	var gallerystuff = [];
	var inicio = 0;
	var itensPagina = 12;
	var getContainer = $(this);
	var cPage = 0;
	var divTotal;
	var slideShow = 'next';
	var defaults = {
		type: 'default',
		width: 'auto',
		navHolder: 'NEXT',
		nextText: '',
		prevText: '',
	
		gutterSpace: 10,
		coloum: 5,
		activate: function(){}
	}
	var options = $.extend(defaults, options);            
    var opt = options, jtype = opt.type, navHolder = opt.navHolder, jwidth = opt.width, jnextText = opt.nextText, 
	jprevText = opt.prevText, jgutterSpace = opt.gutterSpace, jcoloum = opt.coloum;
	var total = $(this).find('ul> li').size();
	$(this).find('ul> li').each(function(index) {
		var imageSrc = $(this).find('img').attr('src');
		//var thisHtml = $(this).text();
		var imageHref = $(this).find('a').attr('href');
		var imageCaption = $(this).find('img').attr('alt');
		gallerystuff.push( {'imageSrc':imageSrc, 'imageHref':imageHref, 'imageCaption':imageCaption} );
	});
	$(this).html('');
	$(this).prepend('<div id="sscsworld"></div>');
	$(this).append('<div id="navHolder"></div>');
	$(this).append('<div class="spacer"></div>');
	if(opt.navHolder != 'none')
	{
		$(this).find(opt.navHolder).html('<div id="next-swa" class="swa-nav">'+opt.nextText+'</div><div id="prev-swa" class="swa-nav">'+opt.prevText+'</div>');
		$(this).find('#next-swa').click(function(e) {
			e.preventDefault();
			
			showPhotoNext();
			return false;
	   });
	   //
	   $(this).find('#prev-swa').click(function(e) {
			e.preventDefault();
			showPhotoPrev();
			return false;
	   });
	}
	////////////////////////////////////////////////////////////
	setGallery();
	///////////////////////////////////////
	function setGallery()
	{
		//alert(opt.coloum);
		getContainer.find('#sscsworld').html('');
		var letCount = 0;
		var letCountId = 0;
		var getGal = '<div class="gallary-row" id="gallary-row-'+letCountId+'" style="display:none;">';
		//getGal += '<ul>';
		for (var i = 0; i<total; i++) {
			getGal += '<a class="group_'+letCountId+'" href="'+gallerystuff[i].imageHref+'" title="'+gallerystuff[i].imageCaption+'"><img src="'+gallerystuff[i].imageSrc+'" alt="'+gallerystuff[i].imageCaption+'" /></a>';
			if(letCount == opt.coloum-1)
			{
				getGal += '<div class="spacer"></div>';
				getGal += '</div>';
				letCountId++;
				letCount = 0;
				getGal += '<div class="gallary-row" id="gallary-row-'+letCountId+'" style="display:none;">';
			}
			else
			{
				letCount++;
			}
		}
		getGal += '<div class="spacer"></div>';
		getGal += '</div>';
		getGal += '<div class="spacer"></div>';
		getContainer.find('#sscsworld').html(getGal);
		////////
		$('.gallary-row a').magnificPopup({
		  //delegate: 'a', // child items selector, by clicking on it popup will open
		  gallery:{enabled:true},
		  type: 'image'
		  // other options
		});
		/*getContainer.find('#sscsworld #gallary-row-0').each(function(index) {
			//alert($(this).html());
			$(this).magnificPopup({
			  delegate: 'a', // child items selector, by clicking on it popup will open
			  type: 'image'
			  // other options
			});
		});*/
		///////////
		updateView();
		
	}
	function updateView()
	{
		updateGaleryDisplay();
		divTotal = getContainer.find('#sscsworld > .gallary-row').size();
		getContainer.find('#sscsworld #gallary-row-'+cPage).fadeIn('slow');
	}
	////////////////////////////////////////////////////////////
	function showPhotoNext()
	{
		if(cPage < divTotal-1)
		{
			getContainer.find('#sscsworld #gallary-row-'+cPage).fadeOut( "slow", function() {
				cPage++;
				showCurrent();
			});
		}
		
	}
	function showCurrent()
	{
		getContainer.find('#sscsworld #gallary-row-'+cPage).fadeIn('slow');
	}
	function showPhotoPrev()
	{
		if(cPage > 0)
		{
			getContainer.find('#sscsworld #gallary-row-'+cPage).fadeOut( "slow", function() {
				cPage--;
				showCurrent();
			});
		}
	}
	//////////////////////////////////////////////////////
	function updateGaleryDisplay()
	{
		var nn = 0;
		var getDocW = getContainer.innerWidth();
		var myStyle = '';
		//alert('getDocW: '+getDocW)
		if(getDocW <= 320)
		{
			//alert(1);
			myStyle = 'single';
		}
		else if(getDocW > 320 && getDocW <= 480)
		{
			myStyle = 'double';
		}
		else if(getDocW > 480 && getDocW <= 768)
		{
			myStyle = 'three';
		}
		else
		{
			myStyle = 'four';
		}
		////////////////////////////////////////////////
		getContainer.find('#sscsworld .gallary-row a').each(function(index) {
			var imageSrc = $(this).find('img').attr('src');
			var Height = 0;
			if(myStyle == 'single')
			{
				$(this).css({'width':'100%', 'display':'block', 'height':'auto', 'margin':'0 0 '+opt.gutterSpace+'px 0'});
			}
			else if(myStyle == 'double')
			{
				var getTotalSpace = opt.gutterSpace;
				var newW = (getContainer.width()-getTotalSpace)/2;
				if(nn == 0)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 '+opt.gutterSpace+'px '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 1;
				}
				else
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 0 '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 0;
				}
			}
			else if(myStyle == 'three')
			{
				var getTotalSpace = opt.gutterSpace*2;
				var newW = (getContainer.width()-getTotalSpace)/3;
				if(nn == 0)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 0 '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 1;
				}
				else if(nn == 1)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 '+opt.gutterSpace+'px '+opt.gutterSpace+'px '+opt.gutterSpace+'px', 'float':'left'});
					nn = 2;
				}
				else if(nn == 2)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 0 '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 0;
				}
			}
			else if(myStyle == 'four')
			{
				var getTotalSpace = opt.gutterSpace*3;
				var newW = (getContainer.width()-getTotalSpace)/4;
				if(nn == 0)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 0 '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 1;
				}
				else if(nn == 1)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 '+opt.gutterSpace+'px '+opt.gutterSpace+'px '+opt.gutterSpace+'px', 'float':'left'});
					nn = 2;
				}
				else if(nn == 2)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 '+opt.gutterSpace+'px '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 3;
				}
				else if(nn == 3)
				{
					$(this).css({'width':newW+'px', 'height':'auto', 'margin':'0 0 '+opt.gutterSpace+'px 0', 'float':'left'});
					nn = 0;
				}
			}
		});
		////////////////////////////////////////////////
		getContainer.find('#sscsworld a img').css({'width':'100%', 'height':'auto', 'display':'block'});
		getContainer.find('#sscsworld').fadeIn('slow');
		getContainer.find('#sscsworld').css({'min-height':'100px'});
		var getHH = getContainer.find('#sscsworld').height();
		//$(".group2").colorbox({rel:'group2', transition:"fade"});
		/*$('.group2').magnificPopup({ 
		  type: 'image'
			// other options
		});*/
		setTimeout(updateGaleryDisplay, 100);
	}
	$( window ).resize(function() {
		updateGaleryDisplay();
	});
	
}