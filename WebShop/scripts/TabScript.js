$(document).ready(function () {
    tabObj.AddTabEffect();
    SetSelectedTab();
    $(document).bind('keydown', 'ctrl+left', SwitchToPrevTab);
    $(document).bind('keydown', 'ctrl+right', SwitchToNextTab);
});

var tabObj =
{
    AddTabEffect: function () {
        //$("#tabzs").tab();

        // hide all contents of tab
        $('#TabContent .tab:not(:first)').hide();
        // remove selected tab class
        $('#tabz .tab').removeClass("tab");
        // add for first one
        $('#tabz div:first').addClass("tab");
        // add click event
        $('#tabz div').click(this.AddClick).eq(0); //.addClass('current');
    },

    AddClick: function (e) {
        $('#TabContent .tab').hide();
        $('#tabz .tab').removeClass("tab");
        // add class for this (ie. clicked div)
        $(this).addClass('tab');

        var clicked = $(this).find('a:first').attr('href');
        $('#TabContent ' + clicked).fadeIn('fast');
        e.preventDefault();
        // save selected tab
        $("[id$='SelectedTabHiddenField']").val(clicked);
    }
};

function SetSelectedTab() {
    var clicked = $("[id$='SelectedTabHiddenField']").val(clicked);
    if (clicked != undefined) {
        $('#TabContent .tab').hide();
        $('#tabz .tab').removeClass("tab");
        // add class for this (ie. clicked div)
        var tab = clicked.replace("-", "");

        $('#tabz ' + clicked.replace("-", "")).addClass("tab");
        $('#TabContent ' + clicked).fadeIn('fast');
    }
    //clientActiveTabChanged(clicked);
}

function SwitchToPrevTab() {
    var nextIdNr = parseInt($("[id$='SelectedTabHiddenField']").val().split("-")[1]) - 1;

    if (nextIdNr > 0) {
        var nextId = "#tab-" + (nextIdNr);
        $("[id$='SelectedTabHiddenField']").val(nextId)

        SetSelectedTab();
    }
}

function SwitchToNextTab() {
    var nextIdNr = parseInt($("[id$='SelectedTabHiddenField']").val().split("-")[1]) + 1;

    if (nextIdNr < 4) {
        var nextId = "#tab-" + (nextIdNr);
        $("[id$='SelectedTabHiddenField']").val(nextId)

        SetSelectedTab();
    }
}
