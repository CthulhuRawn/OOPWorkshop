$(document).ready(function(){
                        $('.dropdown-menu a').on('click', function(){ 
                                $('.dropdown-toggle').html($(this).html() + '  <span class="caret"></span>');    
                        })

                         $('.nav-tabs a').on('click', function (e) {
                                e.preventDefault();
                                $(this).tab('show');
                        })
                })
				
addRow = function(tableName, button){
	$(button).addClass('disabled');
	$(button).attr("disabled", true);
	var date = Date.now();
	var newRow = '<tr><td><button class="btn btn-primary" onclick="saveRow(this,' + date + ','+ $(button).attr('id')+ ')">Save</button></td><td><input id="' + date + '" type="text" class="form-control"></input></td><td></td></tr>';
	$('#'+tableName +' > tbody:last-child').append(newRow);
}

saveRow = function(saveButton,textBoxId,addButton){
	$(addButton).removeClass('disabled');
	$(addButton).attr("disabled", false);
	var value = $('#' +textBoxId).val()
	var parent = $('#' +textBoxId).parent();
	$('#' +textBoxId).remove();
	$(parent).append(value);
	$(saveButton).remove();

}