/// Created by Hassan El-Saabi


/// Adds table row hover effect
function InitTableHoverEffect(tableName)
{
    // add color for mouse over rows
//    $("[id$='_HoursSheetTable'] tr").mouseover(function(){

    $("[id$='" + tableName + "'] tbody tr").mouseover(function(){
        
        $(this).addClass('hover');
     });
     // remove bg-color for mouseout rows
     $("[id$='" + tableName + "'] tbody tr").mouseout(function(){
            $(this).removeClass('hover');
        });
}

function InitCalendarPopup(controlName)
{
    // add a click event to DateTextBox
    $("[id$='" + controlName + "']").click(function(){
        // add datepicker
        $(this).datepicker({ dateFormat: 'yy-mm-dd' })
        // show datepicker
        .datepicker("show");
    });

}

function InitFocus(controlName) {
    // add focus to input
    // $("#divOne :input[type='text']:first").focus();
    $("[id$='" + controlName + "']").focus();

}


//function InitTableHoverEffect2(tableName)
//{
//    // add color for mouse over rows
//    $('#'+ tableName +' tbody tr').mouseover(function(){
//        $(this).addClass('hover');
//     });
//     // remove bg-color for mouseout rows
//     $('#' + tableName + ' tbody tr').mouseout(function(){
//            $(this).removeClass('hover');
//        });
//}


function ShowViewStateSize() {
    var buf = document.forms[0]["__VIEWSTATE"].value;
    var size = buf.length / 1000;
    alert("View state is " + size + " kbytes");
}