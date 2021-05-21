function confirmDelete(isClicked) {
    var deleteSpan = 'deleteSpan';
    var confirmDeleteSpan = 'confirmDeleteSpan';

    if (isClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

function toonDetails(showDetails, id) {
    var detailSpan = 'detailSpan_' + id;
    var toonDetailSpan = 'toonDetailSpan_' + id;

    if (showDetails) {
        $('#' + detailSpan).hide();
        $('#' + toonDetailSpan).show();
    }
    else {
        $('#' + detailSpan).show();
        $('#' + toonDetailSpan).hide();
    }
}