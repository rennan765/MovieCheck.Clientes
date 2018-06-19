//JAVASCRIPT + JQUERY
function showInfo(number){
	$('.movie').addClass('hidden');
	$('#'.concat(number)).removeClass('hidden');
}

//JQUERY
$(document).ready(function(){
	//UPDATE USER INFORMATION
	$('#infoButton').click(function(){
		$('.info').addClass('hidden');
		$('.registerContainer').removeClass('hidden');
    });
    //HIDE SUCESS AND FAILURE MESSAGE BOX
    $('.hideMainMessages').click(function () {
        $('.message').addClass('hidden');
        $('.info').removeClass('hidden');
    });
	//SEARCH ENGINE - SHOW ADVANCED SEARCH
	$('#pSearchBox').click(function(){
		$('.searchBox').addClass('hidden');
		$('.advancedSearchBox').removeClass('hidden');
	});
	//SEARCH ENGINE - HIDE ADVANCED SEARCH
	$('#pAdvancedSearchBox').click(function(){
		$('.advancedSearchBox').addClass('hidden');
		$('.searchBox').removeClass('hidden');
	});
	//SHOW NEW GUEST FORM
	$('#newGuestForm').click(function(){
		$('.registerGuestContainer').removeClass('hidden');
		$('.message').addClass('hidden');
		$('.editGuestContainer').addClass('hidden');
		$('.info').addClass('hidden');
	});
	//EDIT GUEST INFORMATION
	$('#infoButton').click(function(){
		$('.info').addClass('hidden');
		$('.editGuestContainer').removeClass('hidden');
	});
	//HIDE SUCESS AND FAILURE MESSAGE BOX
	$('.hideMessages').click(function(){
		$('.hideMainMessages').add('hidden');
		$('.info').removeClass('hidden');
	});
});