
jQuery(document).ready(function() {

jQuery('.navbar-toggle').click(function(){
jQuery('.navbar-right').toggle();
});

/*jQuery('#head-profile').click(function(){
jQuery('.view-profile').toggle();
});

jQuery('#head-journey').click(function(){
jQuery('.view-journeys').toggle();
});

jQuery('#head-teams').click(function(){
jQuery('.view-teams').toggle();
});

jQuery('#head-add').click(function(){
jQuery('.view-add-more').toggle();
});

jQuery('#head-notification').click(function(){
jQuery('.view-notifications').toggle();
});*/

jQuery('#head-info').click(function(){
jQuery('.view-info').toggle();
});

jQuery('#create-team-button').click(function(){
jQuery('.createnewteam').show();
});

jQuery('#head-team-create').click(function(){
jQuery('.createnewteam').show();
jQuery('.view-teams').hide();
});

});



$(document).ready(function(){
    $('#head-profile').click(function(event){
        event.stopPropagation();
         $(".view-profile").slideToggle();
    });
    $(".view-profile").on("click", function (event) {
        event.stopPropagation();
    });
	
	
	 $('#head-journey').click(function(event){
        event.stopPropagation();
         $(".view-journeys").slideToggle();
    });
    $(".view-journeys").on("click", function (event) {
        event.stopPropagation();
    });
	
	 $('#head-teams').click(function(event){
        event.stopPropagation();
         $(".view-teams").slideToggle();
    });
    $(".view-teams").on("click", function (event) {
        event.stopPropagation();
    });
	
	 $('#head-add').click(function(event){
        event.stopPropagation();
         $(".view-add-more").slideToggle();
    });
    $(".view-add-more").on("click", function (event) {
        event.stopPropagation();
    });
	
	 $('#head-notification').click(function(event){
        event.stopPropagation();
         $(".view-notifications").slideToggle();
    });
    $(".view-notifications").on("click", function (event) {
        event.stopPropagation();
    });
	
	
});

$(document).on("click", function () {
    $(".view-profile").hide();
	$(".view-journeys").hide();
	$(".view-teams").hide();
	$(".view-add-more").hide();
	$(".view-notifications").hide();
});
