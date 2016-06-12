// Write your Javascript code.
$(function () {
    var $j = jQuery.noConflict();
    // This will make every element with the class "date-picker" into a DatePicker element
    $j('.date-picker').datepicker();
});

$(".deletelink").click(function(event) {
    event.preventDefault();
    $('<div title="Confirm Box"></div>').dialog({
        open: function (event, ui) {
            $(this).html("Do you want to delete?");
        },
        close: function () {
            $(this).remove();
        },
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            'Yes': function () {
                $(this).dialog('close');
                $.post(true);

            },
            'No': function () {
                $(this).dialog('close');
                $.post(false);
            }
        }
    });
});